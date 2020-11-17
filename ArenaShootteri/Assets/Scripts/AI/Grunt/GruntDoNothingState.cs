using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntDoNothingState : BaseState
{
    private Grunt grunt;

    public GruntDoNothingState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override void OnStateEnter()
    {

    }

    public override void OnStateExit()
    {

    }

    public override Type Tick()
    {
        return null;
    }
}

