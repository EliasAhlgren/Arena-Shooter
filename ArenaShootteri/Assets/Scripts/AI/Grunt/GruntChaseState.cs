using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Chase logic for Grunt's AI
/// </summary>
public class GruntChaseState : BaseState
{
    private Grunt grunt;
    
    // !!! Change these to Grunt.cs !!!
    private float chargeCooldown = 5f;
    private float timeSinceLastCharge = 5f;

    public GruntChaseState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override void OnStateEnter()
    {
        // Set path destination to target position
        grunt.agent.SetDestination(grunt.target.transform.position);
        grunt.animator.Play("Walk");
        Debug.Log("Chase in.");
    }

    public override void OnStateExit()
    {
        Debug.Log("Chase Out.");
        grunt.animator.ResetTrigger("WalkTrigger");

    }

    public override Type Tick()
    {
        // go to nothing state if target or enemy is dead
        if (grunt.target == null || grunt == null)
        {
            Debug.Log("Unit doesnt have a target to chase");
            return typeof(GruntDoNothingState);
        }

        // Set path destination to target position
        grunt.agent.SetDestination(grunt.target.transform.position);

        // Track distance to target
        float distance = Vector3.Distance(grunt.transform.position, grunt.target.transform.position);

        // Time since last charge !!! <TODO> Change to Grunt.cs file, better to track there. !!!
        timeSinceLastCharge += Time.deltaTime;

        // if time since last charge is longer than charge cooldown -> try to Charge...
        if (timeSinceLastCharge > chargeCooldown)
        {
            // ... But only if at certain distance -> if true, go to wind up state
            if (distance > grunt.attackRange + 2 && distance < grunt.attackRange + 10)
            {
                // Reset time since last charge
                timeSinceLastCharge = 0f;
                return typeof(GruntWindUpState);

            }
        }
        // if grunt's distance to target is smaller than attack range -> try to attack...
        if (distance < grunt.attackRange)
        {
            // ... But only if attack is ready
            if (grunt.readyToAttack)
            {
                return typeof(GruntAttackState);
            }
        }
        
        return null;
    }
}
