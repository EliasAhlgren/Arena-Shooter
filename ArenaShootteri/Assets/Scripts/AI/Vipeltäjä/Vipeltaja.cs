using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vipeltaja : MonoBehaviour
{
    public GameObject target { get; private set; }
    public float speed = 10;
    public float attackRange = 3;
    public NavMeshAgent agent;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();


        // These might not be necessary
        SetRigidbodyState(true);
        setColliderState(false);
    }

    private void Awake()
    {
        InitStateMachine();
        target = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Vipeltäjä is awake");
    }

    private void InitStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
       {
            /// Add more states if necessary.
            {typeof(VipeltajaChaseState), new VipeltajaChaseState(_vipeltaja: this) },
            {typeof(VipeltajaAttackState), new VipeltajaAttackState(_vipeltaja: this) },
            {typeof(VipeltajaDoNothingState), new VipeltajaDoNothingState(_vipeltaja: this) }
        };
        GetComponent<Vipeltaja_StateMachine>().SetStates(states);
    }

    /// <summary>
    /// method <c>SetTarget</c> set target for Vipeltaja
    /// </summary>
    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public void Attack()
    {
        Debug.Log("Vipeltäjä melee attack");
        // Implement what enemy does when attack happens
        target.GetComponent<PlayerCharacterControllerRigidBody>().killPlayer();
    }

    public IEnumerator Die()
    {
        // Disable rigidbody and enable Colliders for each body part
        // for rigidbody death "animation"
        SetRigidbodyState(false);
        setColliderState(true);

        // Disable all AI components for the Vipeltäjä.
        animator.enabled = false;                           // Stop animator
        agent.enabled = false;                              // Stop Nav Mesh Agent
        GetComponent<Vipeltaja_StateMachine>().enabled = false;       // Stop AI
        Destroy(transform.Find("Hitbox").gameObject);       // Destroy Hitbox

        // Enemy stays on ground for 2 seconds.
        // After that set all colliders back to false 
        // and let body sink throught the floor
        // Then destroy the whole GameObject
        yield return new WaitForSeconds(2);
        setColliderState(false);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }


    void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
            if (collider.name.Equals("Hitbox") || collider.name.Equals("VisionCone"))
            {
                collider.enabled = !state;
            }
        }
    }
}
