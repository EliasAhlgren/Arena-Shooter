﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Charge logic for Grunt AI
/// </summary>
public class GruntChargeState : BaseState
{
    private Grunt grunt;
    private Collider chargeCollider;
    private float distanceTravelled = 0;
    private Vector3 startPosition;


    public GruntChargeState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;

        if (grunt.transform.root.Find("ChargeCollider") == null)
        {
            GameObject go = new GameObject("ChargeCollider");
            go.transform.SetParent(grunt.transform);

        }
        chargeCollider = grunt.transform.root.Find("ChargeCollider").GetComponent<Collider>();
        if (chargeCollider == null)
        { 
            SphereCollider sc = grunt.transform.root.Find("ChargeCollider").gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
            sc.radius = 1.2f;
            chargeCollider = sc;    
        }

        chargeCollider.enabled = false;
    }

    public override void OnStateEnter()
    {
        chargeCollider.enabled = true;
        // Store start position of Grunt at start of charge
        startPosition = grunt.transform.position;

        // Clear grunt's Navigation Path
        grunt.agent.ResetPath();

        grunt.isCharging = true;
        // grunt.animator.SetBool("IsRunning", true);
        // grunt.animator.Play("Run");
    }

    public override void OnStateExit()
    {
        chargeCollider.enabled = false;
        grunt.agent.ResetPath();
        grunt.isCharging = false;
        distanceTravelled = 0;
        grunt.animator.SetTrigger("WalkTrigger");
        
    }

    public override Type Tick()
    {
        // If Grunt dies, return Nothing state.
        if (grunt == null)
        {
            return typeof(GruntDoNothingState);
        }

        grunt.agent.ResetPath();

        grunt.transform.Translate((Vector3.forward.normalized * grunt.runSpeed + Physics.gravity * grunt.gravityScale) * Time.deltaTime);
        
        // Keep track of the distance travelled with charge
        // ( Could track time too ) ??? 
        distanceTravelled = Vector3.Distance(grunt.transform.position, startPosition);

        // Return back chase state when grunt has traveled enough distance
        if (distanceTravelled >= grunt.attackRange + 10)
        { 
            return typeof(GruntChaseState);
        }
       
        return null;
    }
    

}
