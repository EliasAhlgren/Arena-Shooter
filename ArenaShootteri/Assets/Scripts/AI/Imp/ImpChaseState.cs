using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpChaseState : BaseState
{
    private Imp imp;

    public ImpChaseState(Imp _imp) : base(_imp.gameObject)
    {
        imp = _imp;
        Debug.Log(imp);
        Debug.Log(_imp);
    }

    public override void OnStateEnter()
    {
        // Set path destination to target position
        imp.agent.SetDestination(imp.target.transform.position);

        // Start with Idle, and trasition to Walk when unit actually moves.
        // << Idle animation >>
    }

    public override void OnStateExit()
    {
        
    }

    public override Type Tick()
    {
        if (imp == null)
        {
            return typeof(ImpDoNothingState);
        }
        // Set path destination to target position
        imp.agent.SetDestination(imp.target.transform.position);

        // Track distance to target
        float distance = Vector3.Distance(imp.transform.position, imp.target.transform.position);

        // if distance to target is smaller than attack range ->  try to attack...
        if(distance < imp.attackRange)
        {
            // ... but only if attack is ready
            if(imp.readyToAttack)
            {
                return typeof(ImpAttackState);
            }
        }

        return null;
    }
}
