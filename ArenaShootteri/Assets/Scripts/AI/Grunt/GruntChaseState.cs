using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntChaseState : BaseState
{
    private Grunt grunt;

    private float chargeCooldown = 5f;
    private float timeSinceLastCharge = 5f;

    public GruntChaseState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override void OnStateEnter()
    {
        grunt.animator.SetBool("IsWalking", true);
        grunt.animator.Play("Walk");
        Debug.Log("Chase in.");
    }

    public override void OnStateExit()
    {
        Debug.Log("Chase Out.");
        grunt.animator.SetBool("IsWalking", false);

    }

    public override Type Tick()
    {
        if (grunt.target == null)
        {
            Debug.Log("Unit doesnt have a target to chase");
            // return typeof(IdleState);
        }


        // Implement what to do when enemy is chasing 
        if (grunt.target != null)
        {
            grunt.agent.SetDestination(grunt.target.transform.position);

            float distance = Vector3.Distance(grunt.transform.position, grunt.target.transform.position);

            timeSinceLastCharge += Time.deltaTime;
            if (timeSinceLastCharge > chargeCooldown)
            {
                if (distance > grunt.attackRange + 2 && distance < grunt.attackRange + 10)
                {                   
                    timeSinceLastCharge = 0f;
                    return typeof(GruntWindUpState);
                    
                }
            }

            if (distance < grunt.attackRange)
            {
                return typeof(GruntAttackState);
            }
        }


        return null;
    }
}


