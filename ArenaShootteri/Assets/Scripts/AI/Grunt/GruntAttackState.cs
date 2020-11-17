using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GruntAttackState : BaseState 
{
    private Grunt grunt;

    public GruntAttackState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override void OnStateEnter()
    {
        Debug.Log("Attack in.");
        grunt.agent.isStopped = true;
        grunt.animator.Play("Punch");
    }

    public override void OnStateExit()
    {
        grunt.agent.isStopped = false;
        Debug.Log("Attack out.");
        
    }

    public override Type Tick()
    {
        Vector3 targetDirection = grunt.target.transform.position - grunt.transform.position;
        float turn = 2 * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(grunt.transform.forward, targetDirection, turn, 0.0f);
        grunt.transform.rotation = Quaternion.LookRotation(newDirection);

        if (grunt == null)
        {
            return typeof(GruntDoNothingState);
        }
                
        if(!grunt.animator.GetCurrentAnimatorStateInfo(0).IsName("Punch") )
        {
            
            grunt.readyToAttack = false;
            return typeof(GruntChaseState);
        }

        return null;
    }


}
