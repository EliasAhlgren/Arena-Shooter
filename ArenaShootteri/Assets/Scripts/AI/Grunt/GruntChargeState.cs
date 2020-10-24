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

    public override Type Tick()
    {
        lastPosition = grunt.transform.position;
        grunt.agent.ResetPath();
        // Wind up animation
        grunt.animator.speed = 3;
        
        grunt.transform.Translate(Vector3.forward.normalized * grunt.speed * Time.deltaTime);
        distanceTravelled += Vector3.Distance(grunt.transform.position, lastPosition);
        if (distanceTravelled >= grunt.attackRange + 10)
        {
            grunt.agent.ResetPath();

            distanceTravelled = 0;
            Debug.Log("Charge complete");
            grunt.animator.speed = 1;

            return typeof(GruntChaseState);
        }
       
        return null;
    }
    

}
