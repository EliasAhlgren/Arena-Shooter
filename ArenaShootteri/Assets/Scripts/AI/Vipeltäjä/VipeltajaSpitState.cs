using UnityEngine;
using System.Collections;
using System;

public class VipeltajaSpitState : BaseState
{
    private Vipeltaja vipeltaja;

    public float distanceBetweenSpits = 3;
    public float distanceCounter;
    public float turnSpeed = 2;

    private int ballsSpat;
    public Vector3 startPosition;

    public VipeltajaSpitState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;
    }



    public override void OnStateEnter()
    {
        startPosition = vipeltaja.transform.position;
        distanceCounter = 0f;
        ballsSpat = 0;
        vipeltaja.animator.Play("Idle");
        Debug.Log("Spit enter");
    }

    public override void OnStateExit()
    {
        Debug.Log("Spit Exit");
    }

    public override Type Tick()
    {
        Vector3 targetDirection = vipeltaja.target.transform.position - vipeltaja.transform.position;
        float step = turnSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(vipeltaja.transform.forward, targetDirection, step, 0.0f);
        vipeltaja.transform.rotation = Quaternion.LookRotation(newDirection);

        float dot = Vector3.Dot(vipeltaja.transform.forward, (vipeltaja.target.transform.position - vipeltaja.transform.position).normalized);

        if (dot > 0.9f)
        {
            if (!vipeltaja.animator.GetCurrentAnimatorStateInfo(0).IsName("Spit"))
            {
                if (ballsSpat == 0)
                {
                    vipeltaja.SpawnSpit();
                    ballsSpat++;
                }
            }

            vipeltaja.agent.ResetPath();
            vipeltaja.agent.SetDestination(vipeltaja.target.transform.position);
            distanceCounter = Vector3.Distance(startPosition, vipeltaja.transform.position);
            Debug.Log("Distance is " + distanceCounter + " Start position was: " + startPosition + " Current position is: " + vipeltaja.transform.position);
            if (distanceCounter > distanceBetweenSpits)
            {

                if (ballsSpat == 1)
                {
                    vipeltaja.SpawnSpit();
                    ballsSpat++;
                }
            }
            if (!vipeltaja.animator.GetCurrentAnimatorStateInfo(0).IsName("Spit"))
            {
                if (ballsSpat == 2)
                {
                    return typeof(VipeltajaChaseState);
                }
            }
        }
        return null;
    }


}
