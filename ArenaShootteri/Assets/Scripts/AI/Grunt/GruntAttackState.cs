using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GruntAttackState : BaseState 
{
    private Grunt grunt;
    private float _attackCooldown = 0.5f;
    private float _attackCounter = 0f;
    

    public GruntAttackState(Grunt _grunt) : base(_grunt.gameObject)
    {
        grunt = _grunt;
    }

    public override Type Tick()
    {
        if(grunt == null)
        {
            return typeof(DoNothingState);
        }

        _attackCounter -= Time.deltaTime;

        if (_attackCounter <= 0)
        {
            
            // Do attack stuff
            grunt.Attack();
            _attackCounter = _attackCooldown;
            
        }

        float distance = Vector3.Distance(grunt.transform.position, grunt.target.transform.position);
        if(distance > grunt.attackRange)
        {
            return typeof(GruntChaseState);
        }

        return null;
    }


}
