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
        // Play attack animation
        vipeltaja.animator.Play("Hit");
        vipeltaja.readyToAttack = false;
    }

    public override void OnStateExit()
    {

    }

    public override Type Tick()
    {
        Vector3 targetDir = vipeltaja.target.transform.position - vipeltaja.transform.position;
        float turn = 2 * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(vipeltaja.transform.forward, targetDir, turn, 0.0f);
        vipeltaja.transform.rotation = Quaternion.LookRotation(newDirection);

        if(vipeltaja == null)
        {
            return typeof(VipeltajaDoNothingState);
        }

        if (!vipeltaja.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        { 
            return typeof(VipeltajaChaseState);
        }

        return null;
    }

}

