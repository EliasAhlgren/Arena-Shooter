using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GruntWindUpState : BaseState
{
    private Grunt grunt;

    public float turnSpeed = 2;

    public GruntWindUpState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override void OnStateEnter()
    {
        grunt.agent.isStopped = true;
        grunt.animator.SetTrigger("WindUpTrigger");
        Debug.Log("Wind in.");
    }

    public override void OnStateExit()
    {
        Debug.Log("Wind out.");
        grunt.animator.ResetTrigger("WindUpTrigger");
        grunt.agent.isStopped = false;
    }

    public override Type Tick()
    {
        Vector3 targetDirection = grunt.target.transform.position - grunt.transform.position;
        float step = turnSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(grunt.transform.forward, targetDirection, step, 0.0f);
        grunt.transform.rotation = Quaternion.LookRotation(newDirection);


        if (!grunt.animator.GetCurrentAnimatorStateInfo(0).IsName("WindUp") && !grunt.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            return typeof(GruntChargeState);
        }

        return null;
    }

}
