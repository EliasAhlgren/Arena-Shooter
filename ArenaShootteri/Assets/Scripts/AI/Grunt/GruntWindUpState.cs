using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GruntWindUpState : BaseState
{
    private Grunt grunt;

    public GruntWindUpState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override void OnStateEnter()
    {
        grunt.agent.isStopped = true;
        grunt.animator.Play("WindUp");
        grunt.animator.SetBool("IsWindUp", true);
        Debug.Log("Wind in.");
    }

    public override Type Tick()
    {
        if (!grunt.animator.GetCurrentAnimatorStateInfo(0).IsName("WindUp"))
        {
            return typeof(GruntChargeState);
        }

        return null;
    }

    public override void OnStateExit()
    {
        Debug.Log("Wind out.");
        grunt.animator.SetBool("IsWindUp", false);
        grunt.agent.isStopped = false;
    }

    public IEnumerator WindUpDelay()
    {
        Debug.Log("waiting");
        yield return new WaitForSeconds(2);
    }


}
