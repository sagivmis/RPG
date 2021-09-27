using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {
        [Header("Movement Configuration")]
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float dwellTime = 2f;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float speedMultiplier = 0.8f;
        [SerializeField] PatrolPath patrolPath;

        [Header("Combat Configuration")]
        [SerializeField] bool suspicion = true;
        [SerializeField] bool attack = true;
        [SerializeField] bool patrol = true;
        [SerializeField] float damage = 3f;


        [Header("Prefab")]

        Health health;
        GameObject player;
        Fighter fighter;
        Mover mover;

        [Header("AI Memory")]
        public Vector3 guardPosition;

        public float timeSinceLastSawPlayer = Mathf.Infinity;
        public float timeSinceArriveToWaypoint = Mathf.Infinity;
        public int waypointIndex; // at start assign each one with random integer 

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            if (patrolPath) waypointIndex = UnityEngine.Random.Range(1, patrolPath.transform.childCount);
            else waypointIndex = 0;
            guardPosition = transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
        private void Update()
        {
            if (health.IsDead()) return;

            if (InRange() && fighter.CanAttack(player) && attack)
            {
                Attack();
            }
            else if (timeSinceLastSawPlayer < suspicionTime && suspicion)
            {
                Suspicious();
            }
            else if (patrol) 
            {
                Patrol();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveToWaypoint += Time.deltaTime;
        }


        private void Patrol()
        {
            Vector3 nextPos = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArriveToWaypoint = 0;
                    CycleWaypoint();
                }
                nextPos = GetCurrentWaypointPosition();
            }
            if(timeSinceArriveToWaypoint > dwellTime)
            {
                mover.StartMoveAction(nextPos);
            }
        }

        private Vector3 GetCurrentWaypointPosition()
        {
            return patrolPath.GetChildPosition(waypointIndex);
        }

        private void CycleWaypoint()
        {
            waypointIndex = patrolPath.GetNextIndex(waypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypointPosition());
            return distanceToWaypoint < waypointTolerance;
        }

        private void Attack()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private void Suspicious()
        {
            GetComponent<Scheduler>().CancelCurrentAction();
        }

        private bool InRange()
        {
            return (Vector3.Distance(transform.position, player.transform.position) < chaseDistance);
        }

        //ANIMATIONS 

        public void BabyDragonNormal()
        {
            player.GetComponent<Health>().TakeDamage(damage);
        }
        public void SpiderNormal()
        {
            player.GetComponent<Health>().TakeDamage(damage);
        }
        public void LarvaNormal()
        {
            //if contains BOSS component/tag increase dmg
            player.GetComponent<Health>().TakeDamage(damage);
        }
        public void HyenaNormal()
        {
            player.GetComponent<Health>().TakeDamage(damage);
        }
    }

}