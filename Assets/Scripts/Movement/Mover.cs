using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float maxSpeed = 9.3f;
        /*
         * SPIDER:: 3f
         * BABY DRAGON:: 4f
         * HYENA:: 6f
         * LARVA:: 2f
         * LARVA:: 3f
         */

        NavMeshAgent agent;
        Animator animator;
        Health health;

        [Header("ETC")]
        [SerializeField] float playerRunAnimationMaxSpeed = 1.76f;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            agent.enabled = !health.IsDead();
            UpdateAnimator();

        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<Scheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speedRatio = localVelocity.z;
            if (speedRatio > playerRunAnimationMaxSpeed) speedRatio = playerRunAnimationMaxSpeed;
            animator.SetFloat("speed", speedRatio);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            agent.isStopped = false;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.SetDestination(destination);
        }

    }
}
