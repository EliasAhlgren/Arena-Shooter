using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Chase logic for Vipeltaja's AI
/// </summary>
public class VipeltajaChaseState : BaseState
{
    private Vipeltaja vipeltaja;

    public VipeltajaChaseState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;
    }

    public override void OnStateEnter()
    {
        // Set path destination to target position
        vipeltaja.agent.SetDestination(vipeltaja.target.transform.position);

        // Start with Idle, and transition to Walk when unit actually moves.
        vipeltaja.animator.Play("Idle");
    }

    public override void OnStateExit()
    { 
        
    }

    
    public override Type Tick()
    {
        // go to nothing state if target or enemy is dead
        if (vipeltaja.target == null || vipeltaja == null)
        {
            return typeof(VipeltajaDoNothingState);
        }

        // Set path destination to target position
        vipeltaja.agent.SetDestination(vipeltaja.target.transform.position);
        
        // Track distance to target
        float distance = Vector3.Distance(vipeltaja.transform.position, vipeltaja.target.transform.position);
        
        // if distance to target is smaller than attack range -> try to attack...
        if(distance < vipeltaja.attackRange)
        {
            // ... But only if attack is ready
            if (vipeltaja.readyToAttack)
            {
                return typeof(VipeltajaAttackState);
            }
        }

        // If distance to target is approximately jumping distances -> Jump
        if(distance < vipeltaja.jumpDistance+3 && distance > vipeltaja.jumpDistance-3)
        {
            return typeof(VipeltajaJumpState);
        }

        // Fear state Debug
        // remember to delete
        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Escaping");
            return typeof(VipeltajaEscapeState);
        }
        
        return null;
    }

}
