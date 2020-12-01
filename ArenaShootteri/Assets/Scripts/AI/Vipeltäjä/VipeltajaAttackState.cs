using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Attack logic for Vipeltaja AI
/// </summary>
public class VipeltajaAttackState : BaseState
{
    private Vipeltaja vipeltaja;
    public List<Collider> attackColliders = new List<Collider>();

    public VipeltajaAttackState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;
        
        Collider[] AllColliders = vipeltaja.transform.GetComponentsInChildren<Collider>();

        foreach(Collider collider in AllColliders)
        {
            
            if (collider.transform.CompareTag("VipeltajaAttackHitbox"))
            { 
                attackColliders.Add(collider);
                collider.enabled = false;
            }
        }
    }

    public override void OnStateEnter()
    {
        
        SetAttackColliders(true);
        Vipeltaja.PlaySound("shout", vipeltaja.GetComponent<AudioSource>());

        // Play attack animation
        vipeltaja.animator.Play("Hit");
        vipeltaja.readyToAttack = false;
    }

    public override void OnStateExit()
    {
        SetAttackColliders(false);
    }

    public override Type Tick()
    {
        // if Vipeltaja dies, return Nothing state.
        if (vipeltaja == null)
        {
            return typeof(VipeltajaDoNothingState);
        }

        // Rotate vipeltaja all the time towards the player when attacking
        Vector3 targetDir = vipeltaja.target.transform.position - vipeltaja.transform.position;
        float turn = 2 * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(vipeltaja.transform.forward, targetDir, turn, 0.0f);
        vipeltaja.transform.rotation = Quaternion.LookRotation(newDirection);

        // Go back to chase state when Hit animation is over
        if (!vipeltaja.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        { 
            return typeof(VipeltajaChaseState);
        }

        return null;
    }

    void SetAttackColliders(bool state)
    {
        foreach(Collider collider in attackColliders)
        {
            collider.enabled = state;
        }
    }

    
    

}

