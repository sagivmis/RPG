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
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] PatrolPath patrolPath;

        Health health;
        GameObject player;
        Fighter fighter;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int waypointIndex = 0; // at start assign each one with random integer 

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

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

            if (InRange() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                Attack();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                Suspicious();
            }
            else
            {
                Patrol();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void Patrol()
        {
            Vector3 nextPos = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPos = GetCurrentWaypointPosition();
            }
            mover.StartMoveAction(nextPos);
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
    }

}