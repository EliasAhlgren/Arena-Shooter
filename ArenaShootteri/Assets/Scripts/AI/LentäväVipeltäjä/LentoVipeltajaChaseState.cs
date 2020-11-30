using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Chase logic for Vipeltaja's AI
/// </summary>
public class LentoVipeltajaChaseState : BaseState
{
    private LentoVipeltaja Lvipeltaja;

    public LentoVipeltajaChaseState(LentoVipeltaja _Lvipeltaja) : base(_Lvipeltaja.gameObject)
    {
        Lvipeltaja = _Lvipeltaja;
    }

    public override void OnStateEnter()
    {
        Lvipeltaja.agent.SetDestination(Lvipeltaja.target.transform.position);
        Lvipeltaja.animator.Play("Walk");
    }

    public override void OnStateExit()
    {

    }


    public override Type Tick()
    {
        if (Lvipeltaja.target == null || Lvipeltaja == null)
        {
            return typeof(VipeltajaDoNothingState); 
        }

        Lvipeltaja.agent.SetDestination(Lvipeltaja.target.transform.position);

        float distance = Vector3.Distance(Lvipeltaja.transform.position, Lvipeltaja.target.transform.position);

        if(distance < Lvipeltaja.attackRange && Lvipeltaja.readyToAttack)
        {
            return typeof(LentoVipeltajaAttackState);
        }

        return null;
    }

}
