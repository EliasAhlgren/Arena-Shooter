using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;

public class VipeltajaEscapeState : BaseState
{
    private Vipeltaja vipeltaja;
    private float timeToFlee = 4f;
    private float timer = 0f;
    private Vector3 direction;


    public VipeltajaEscapeState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;

    }

    public override void OnStateEnter()
    {
        timer = 0f;
        vipeltaja.agent.isStopped = true;
        Debug.Log("Escape Enter");
        
        vipeltaja.transform.rotation = Quaternion.LookRotation(vipeltaja.transform.position - vipeltaja.target.transform.position);
        direction = vipeltaja.target.transform.position - vipeltaja.transform.position;

        // Run animations
    }

    public override void OnStateExit()
    {
        vipeltaja.agent.isStopped = false; ;
        Debug.Log("Escape Exit");
        // Something Something
    }

    public override Type Tick()
    {
        vipeltaja.agent.ResetPath();
        timer += Time.deltaTime;
        // Debug.Log(timer);
        vipeltaja.transform.Translate(Vector3.forward.normalized * vipeltaja.speed * Time.deltaTime);
        if (timer >= timeToFlee)
        {
            return typeof(VipeltajaChaseState);
        }
        return null;        
    }
}
