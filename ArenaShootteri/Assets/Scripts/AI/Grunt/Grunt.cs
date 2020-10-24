using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : MonoBehaviour
{
    public GameObject target { get; private set; }
    public float speed = 10;
    public float attackRange = 3;
    public NavMeshAgent agent;
    public Transform cone;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cone = transform.Find("VisionCone");
        SetRigidbodyState(true);
        setColliderState(false);
    }

    private void Awake()
    {
        InitStateMachine();
        target = GameObject.Find("Player").gameObject;
        Debug.Log("Grunt is awake");
    }
    /// <summary>
    /// method <c>InitStateMachine</c> Initialize State machine for enemy
    /// </summary>
    private void InitStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
       {
            /// Add more states if necessary.
            {typeof(GruntChaseState), new GruntChaseState(_grunt: this) },
            {typeof(GruntAttackState), new GruntAttackState(_grunt: this) },
            {typeof(GruntChargeState), new GruntChargeState(_grunt: this) },
            {typeof(DoNothingState), new DoNothingState(_grunt:this) }
        };
        GetComponent<StateMachine>().SetStates(states);
    }
    
     /// <summary>
     /// method <c>SetTarget</c> set target for enemy
     /// </summary>
    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public void Attack()
    {
        // Implement what enemy does when attack happens
        Destroy(target);

    }


    public IEnumerator Die()
    {
        animator.enabled = false;
        agent.isStopped = true;
        GetComponent<StateMachine>().enabled = false;
        SetRigidbodyState(false);
        setColliderState(true);

        yield return new WaitForSeconds(2);
        setColliderState(false);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);



    }

    void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach(Collider collider in colliders)
        {
            collider.enabled = state;
            if (collider.transform.name.Equals("Hitbox"))
            {
                collider.enabled = !state;
            }
        }

        
    }

}
