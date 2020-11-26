using UnityEngine;
using System.Collections;
using System;

public class ImpDoNothingState : BaseState
{
    private Imp imp;

    public ImpDoNothingState(Imp _imp) : base(_imp.gameObject)
    {
        imp = _imp;
    }
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {

    }
    public override Type Tick()
    {
        return null;
    }
}
