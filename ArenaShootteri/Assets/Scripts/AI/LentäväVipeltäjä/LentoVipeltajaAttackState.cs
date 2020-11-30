using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LentoVipeltajaAttackState : BaseState
{
    private LentoVipeltaja Lvipeltaja;
    public List<Collider> attackColliders = new List<Collider>();

    public LentoVipeltajaAttackState(LentoVipeltaja _Lvipeltaja) : base(_Lvipeltaja.gameObject)
    {
        Lvipeltaja = _Lvipeltaja;
    }

    public override void OnStateEnter()
    {
        SetAttackColliders(true);
        Lvipeltaja.animator.Play("Hit");
        Lvipeltaja.readyToAttack = false;
    }

    public override void OnStateExit()
    {
        SetAttackColliders(false);
    }

    public override Type Tick()
    {
        if (Lvipeltaja == null)
        {
            return typeof(VipeltajaDoNothingState);
        }

        // Rotate all the time towards player
        Vector3 targetDir = Lvipeltaja.target.transform.position - Lvipeltaja.transform.position;
        float turn = 2 * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(Lvipeltaja.transform.forward, targetDir, turn, 0.0f);
        Lvipeltaja.transform.rotation = Quaternion.LookRotation(newDirection);

        if(Lvipeltaja.drainPlayer)
        {
            Lvipeltaja.target.GetComponent<PlayerCharacterControllerRigidBody>().
                TakeDamage(Lvipeltaja.damage, true);
            Lvipeltaja.drainPlayer = false;

        }

        float distance = Vector3.Distance(Lvipeltaja.transform.position, Lvipeltaja.target.transform.position);
        if (distance > Lvipeltaja.attackRange)
        {
            return typeof(LentoVipeltajaChaseState);
        }
        return null;    
    }

    void SetAttackColliders(bool state)
    {
        foreach (Collider collider in attackColliders)
        {
            collider.enabled = state;
        }
    }
}