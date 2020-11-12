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
        throw new NotImplementedException();
    }

    public override void OnStateExit()
    {
        throw new NotImplementedException();
    }

    public override Type Tick()
    {

        if(vipeltaja.target != null)
        {
            vipeltaja.agent.SetDestination(vipeltaja.target.transform.position);

            float distance = Vector3.Distance(vipeltaja.transform.position, vipeltaja.target.transform.position);

            if(distance < vipeltaja.attackRange)
            {
                return typeof(VipeltajaAttackState);
            }
        }


        return null;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
