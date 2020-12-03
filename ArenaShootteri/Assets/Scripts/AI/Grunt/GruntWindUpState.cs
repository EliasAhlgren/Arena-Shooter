using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
///  Wind up logic for Grunt's AI 
/// </summary>
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
        Grunt.PlaySound("shout", grunt.GetComponent<AudioSource>());
        grunt.animator.SetTrigger("WindUpTrigger");
    }

    public override void OnStateExit()
    {
        grunt.animator.ResetTrigger("WindUpTrigger");
        grunt.agent.isStopped = false;
    }

    public override Type Tick()
    {
        // rotate grunt all the time towards the player
        Vector3 targetDirection = grunt.target.transform.position - grunt.transform.position;
        float step = turnSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(grunt.transform.forward, targetDirection, step, 0.0f);
        grunt.transform.rotation = Quaternion.LookRotation(newDirection);

        // After wind up animation is complete...
        // ...go to Charge state
        // Also tracks walk animation, so charge State is not returned before wind up animation even starts.
        if (!grunt.animator.GetCurrentAnimatorStateInfo(0).IsName("WindUp") && !grunt.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            return typeof(GruntChargeState);
        }

        return null;
    }

}
