using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : BaseState
{
    private EnemyConcept _enemyConcept;

    public ChargeState(EnemyConcept enemyConcept) : base(enemyConcept.gameObject)
    {
        _enemyConcept = enemyConcept;
    }

    public override Type Tick()
    {
        // Wind up animation
        return typeof(ChaseConcept);
    }
}
