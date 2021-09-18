using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{   
    
    NavMeshAgent agent;
    Animator animator;

    float playerAnimationMaxSpeed = 1.76f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
        CheckDistanceToTarget();

        UpdateAnimator();

    }

    private void CheckDistanceToTarget()
    {
        if(Vector3.Distance(agent.destination, transform.position) <= 3f)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speedRatio = localVelocity.z/agent.speed*2;
        if (speedRatio > playerAnimationMaxSpeed) speedRatio = playerAnimationMaxSpeed;
        animator.SetFloat("speed", speedRatio);
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            agent.SetDestination(hit.point);
        }

    }
}
