using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingState : BaseState
{
    private Grunt grunt;

    public DoNothingState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override Type Tick()
    {
        return null;
    }
}

