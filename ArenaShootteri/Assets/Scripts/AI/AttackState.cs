using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackState : BaseState 
{
    private float _attackCooldown = 0.5f;
    private float _attackCounter = 0f;
    private EnemyConcept _enemyConcept;

    public AttackState(EnemyConcept enemyConcept) : base(enemyConcept.gameObject)
    {
        _enemyConcept = enemyConcept;
    }

    public override Type Tick()
    {
        _attackCounter -= Time.deltaTime;

        if (_attackCounter <= 0)
        {
            
            // Do attack stuff
            _enemyConcept.Attack();
            _attackCounter = _attackCooldown;
            
        }

        float distance = Vector3.Distance(_enemyConcept.transform.position, _enemyConcept._target.transform.position);
        if(distance > _enemyConcept.attackRange)
        {
            return typeof(ChaseConcept);
        }

        return null;
    }


}
