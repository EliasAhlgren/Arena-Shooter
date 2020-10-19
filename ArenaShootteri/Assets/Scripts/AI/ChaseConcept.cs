using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChaseConcept : BaseState
{

    private EnemyConcept _enemyConcept;

    public ChaseConcept(EnemyConcept enemyConcept) : base(enemyConcept.gameObject)
    {
        _enemyConcept = enemyConcept;
    }

    public override Type Tick()
    {
        if (_enemyConcept._target == null)
        {
            Debug.Log("Unit doesnt have a target to chase");
            // return typeof(IdleState);
        }


        // Implement what to do when enemy is chasing 
        if (_enemyConcept._target != null)
        {
            _enemyConcept.agent.SetDestination(_enemyConcept._target.transform.position);

            float distance = Vector3.Distance(_enemyConcept.transform.position, _enemyConcept._target.transform.position);
            if (distance < _enemyConcept.attackRange)
            {
                return typeof(AttackState);
            }
        }


        return null;
    }
}
        
  
