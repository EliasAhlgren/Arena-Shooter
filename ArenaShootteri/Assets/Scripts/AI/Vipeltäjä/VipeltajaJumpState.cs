﻿using UnityEngine;
using System.Collections;
using System;


public class VipeltajaJumpState : BaseState
{
    private Vipeltaja vipeltaja;
    private float originalSpeed;

    public VipeltajaJumpState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;
    }

    public override void OnStateEnter()
    {
        //originalSpeed = vipeltaja.agent.speed;
        //vipeltaja.agent.speed = 0f;
        
        vipeltaja.animator.Play("Jump");
    }

    public override void OnStateExit()
    {
        
        // vipeltaja.agent.speed = originalSpeed;d      
    }

    public override Type Tick()
    {   
        if (vipeltaja.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        vipeltaja.transform.Translate(Vector3.forward.normalized * vipeltaja.jumpSpeed * Time.deltaTime);

        if (!vipeltaja.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            vipeltaja.jumpCoolingdown = true;
            return typeof(VipeltajaChaseState);
        }
        

        return null;
    }

}

