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
        [Tooltip("How far will the enemy chase")]
        [SerializeField] float chaseDistance = 5f;
        [Tooltip("How long will the enemy wait after chase")]
        [SerializeField] float suspicionTime = 5f;
        [Tooltip("How long will the enemy wait in waypoint")]
        [SerializeField] float dwellTime = 2f;
        [Tooltip("How close does the enemy has to get to the waypoint")]
        [SerializeField] float waypointTolerance = 1f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.5f;
        [SerializeField] PatrolPath patrolPath;

        [Header("Combat Configuration")]
        [Tooltip("Apply suspicion")]
        [SerializeField] bool suspicion = true;
        [Tooltip("Apply attack (for enemies)")]
        [SerializeField] bool attack = true;
        [SerializeField] float damage = 3f;
        public float damageBonus = 0;

        [Header("Behaviour Configuration")]
        [Tooltip("Deactivate 'Wander' to configure as patrol, deactivate both for standing still")]
        [SerializeField] bool patrol = true;
        [Tooltip("Deactivate 'Wander' to configure as patrol, deactivate both for standing still")]
        [SerializeField] bool wander = true;


        [Header("Prefab")]

        Health health;
        GameObject player;
        Fighter fighter;
        Mover mover;

        [Header("AI Memory")]
        public Vector3 guardPosition;

        public float timeSinceLastSawPlayer = Mathf.Infinity;
        public float timeSinceArriveToWaypoint = Mathf.Infinity;
        public int waypointIndex;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            if (patrolPath) waypointIndex = GetRandomWaypointIndex();
            else waypointIndex = 0;
            if (gameObject.tag == "Boss") damageBonus = 5;
            guardPosition = transform.position;
        }

        private int GetRandomWaypointIndex()
        {
            return UnityEngine.Random.Range(1, patrolPath.transform.childCount);
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
            if (timeSinceArriveToWaypoint > dwellTime)
            {
                mover.StartMoveAction(nextPos, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypointPosition()
        {
            return patrolPath.GetChildPosition(waypointIndex);
        }

        private void CycleWaypoint()
        {
            if (wander)
            {
                waypointIndex = GetRandomWaypointIndex();
                return;
            }
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
            player.GetComponent<Health>().TakeDamage(damage + damageBonus);
        }
        public void SpiderNormal()
        {
            player.GetComponent<Health>().TakeDamage(damage + damageBonus);
        }
        public void LarvaNormal()
        {
            //if contains BOSS component/tag increase dmg
            player.GetComponent<Health>().TakeDamage(damage + damageBonus);
        }
        public void HyenaNormal()
        {
            player.GetComponent<Health>().TakeDamage(damage + damageBonus);
        }
    }

}