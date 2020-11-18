using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntChargeState : BaseState
{
    private Grunt grunt;
    private float distanceTravelled = 0;
    private Vector3 lastPosition;


    public GruntChargeState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override void OnStateEnter()
    {
        // grunt.animator.SetBool("IsRunning", true);
        // grunt.animator.Play("Run");
        Debug.Log("Charge in");
    }

    public override void OnStateExit()
    {
        // grunt.animator.SetBool("IsRunning", false);
        Debug.Log("Charge out.");
        
    }

    public override Type Tick()
    {
        lastPosition = grunt.transform.position;
        grunt.agent.ResetPath();
        
        grunt.transform.Translate(Vector3.forward.normalized * grunt.runSpeed * Time.deltaTime);
        
        if(!grunt.isCharging)
        {
            grunt.isCharging = true;
        }

        distanceTravelled += Vector3.Distance(grunt.transform.position, lastPosition);

        if (distanceTravelled >= grunt.attackRange + 10)
        {
            grunt.agent.ResetPath();
            grunt.isCharging = false;
            distanceTravelled = 0;
            Debug.Log("Charge complete");
            grunt.animator.SetTrigger("WalkTrigger");
            return typeof(GruntChaseState);
        }
       
        return null;
    }
    

}
