using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VipeltajaChaseState : BaseState
{
    private Vipeltaja vipeltaja;

    public VipeltajaChaseState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;
    }

    public override void OnStateEnter()
    {
        vipeltaja.agent.SetDestination(vipeltaja.target.transform.position);
        Debug.Log("Chase enter");
        vipeltaja.animator.Play("Idle");
    }

    public override void OnStateExit()
    { 
        Debug.Log("Chase exit");
    }

    
    public override Type Tick()
    {
        vipeltaja.agent.SetDestination(vipeltaja.target.transform.position);
        if (vipeltaja.target != null)
        {

            float distance = Vector3.Distance(vipeltaja.transform.position, vipeltaja.target.transform.position);

            if(distance < vipeltaja.attackRange)
            {
                if (vipeltaja.readyToAttack)
                {
                    return typeof(VipeltajaAttackState);
                }
            }

            if(distance < vipeltaja.jumpDistance+3 && distance > vipeltaja.jumpDistance-3)
            {
                return typeof(VipeltajaJumpState);
            }

            if(Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("Escaping");
                return typeof(VipeltajaEscapeState);
            }
        }


        return null;
    }

}
