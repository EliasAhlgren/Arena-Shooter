using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Attack logic for Imp AI
/// </summary>
public class ImpAttackState : BaseState
{
    private Imp imp;
    public List<Collider> attackColliders = new List<Collider>();

    public ImpAttackState(Imp _imp) : base(_imp.gameObject)
    {
        imp = _imp;

        Collider[] allCollider = imp.transform.GetComponentsInChildren<Collider>();

        foreach(Collider collider in allCollider)
        {
            if (collider.transform.CompareTag("ImpAttackHitbox"))
            {
                attackColliders.Add(collider);
                collider.enabled = false;
            }
        }
    }


    public override void OnStateEnter()
    {
        SetAttackColliders(true);
        // Play attack animation
        // << Attack Animation >> 
        imp.animator.Play("Hit");
        imp.readyToAttack = false;
    }

    public override void OnStateExit()
    {
        SetAttackColliders(false);
    }

    public override Type Tick()
    { 
        // if Imp dies, return nothing state.
        if (imp == null)
        {
            return typeof(ImpDoNothingState);
        }

        // Rotate imp all the time towards the player when attacking
        Vector3 targetDir = imp.target.transform.position - imp.transform.position;
        float turn = 2 * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(imp.transform.forward, targetDir, turn, 0.0f);
        imp.transform.rotation = Quaternion.LookRotation(newDirection);

        // Go back to chase state when hit animation is over
        if (!imp.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return typeof(ImpChaseState);
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
