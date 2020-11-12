using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VipeltajaDoNothingState : BaseState
{
    private Vipeltaja vipeltaja;

    public VipeltajaDoNothingState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
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
        return null;
    }
}

