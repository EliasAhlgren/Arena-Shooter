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
    }

    public override void OnStateExit()
    {
        throw new NotImplementedException();
    }

    public override Type Tick()
    {
        if(vipeltaja == null)
        {
            
        }

        _attackCounter -= Time.deltaTime;

        if(_attackCounter <= 0)
        {
            // Attack stuff
            vipeltaja.Attack();
        }

        float distance = Vector3.Distance(vipeltaja.transform.position, vipeltaja.target.transform.position);
        if(distance > vipeltaja.attackRange)
        {
            return typeof(VipeltajaChaseState); 
        }

        return null;
    }

}

