using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class VipeltajaAttackState : BaseState
{
    private Vipeltaja vipeltaja;

    private float _attackCooldown = 2f;
    private float _attackCounter = 0f;

    public VipeltajaAttackState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;
    }

    public override void OnStateEnter()
    {
        throw new NotImplementedException();
        // Play attack animation

    }

    public override void OnStateExit()
    {
        throw new NotImplementedException();
    }

    public override Type Tick()
    {
        if(vipeltaja == null)
        {
            return typeof(VipeltajaDoNothingState);
        }

        if (!vipeltaja.animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            vipeltaja.readyToAttack = false;
            return typeof(VipeltajaChaseState);
        }
        return null;
    }

}

