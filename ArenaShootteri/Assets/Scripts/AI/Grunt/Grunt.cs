using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : MonoBehaviour, IDamage
{
    public GameObject target { get; private set; }

    // Speed is based on current animation speed. Dont change this value.
    // Change animation speed instead.
    public float walkSpeedBase = 6.0f, runSpeedBase = 20.0f;
    public float walkSpeed, runSpeed;

    public float attackRange = 3;
    public NavMeshAgent agent;
    public Transform cone;
    public Animator animator;
    public bool readyToAttack = true;
    public float attackCounter = 0f;
    public float attackCooldown = 2f;
    public bool isCharging = false;
    public float chargeForce = 10;

    public float IHealth { get; set; } = 100f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cone = transform.Find("VisionCone");
        // These might not be necessary
        SetRigidbodyState(true);
        SetColliderState(true);
    }

    private void Awake()
    {
        InitStateMachine();
        target = GameObject.FindGameObjectWithTag("Player");
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
            {typeof(GruntWindUpState), new GruntWindUpState(_grunt: this) },
            {typeof(GruntDoNothingState), new GruntDoNothingState(_grunt:this) }
        };
        GetComponent<Grunt_StateMachine>().SetStates(states);
    }

     /// <summary>
     /// method <c>SetTarget</c> set target for Grunt
     /// </summary>
    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public void Attack()
    {
        Debug.Log("Melee attack");
        // Implement what enemy does when attack happens
        // SetColliderState(true);
        animator.Play("Punch");
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {

        }

        // target.GetComponent<PlayerCharacterControllerRigidBody>().killPlayer();

    }

    public void Update()
    {
        if (!readyToAttack)
        {
            attackCounter += Time.deltaTime;
            Debug.Log(attackCounter);
            if(attackCounter > attackCooldown)
            {
                readyToAttack = true;
            }
        }
        // Keep speed updated based on current clip. 
        
        walkSpeed = walkSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed;
        agent.speed = walkSpeed;
        runSpeed = runSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed;

    }


    //IDamage void
    public void TakeDamage(float damage)
    {
        if (IHealth <= 0f)
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        // Disable rigidbody and enable Colliders for each body part
        // for rigidbody death "animation"
        SetRigidbodyState(false);
        SetColliderState(true);

        // Disable all AI components for the Grunt.
        animator.enabled = false;                           // Stop animator
        agent.enabled = false;                              // Stop Nav Mesh Agent
        GetComponent<Grunt_StateMachine>().enabled = false;       // Stop AI
        Destroy(transform.Find("Hitbox").gameObject);       // Destroy Hitbox
        Destroy(transform.Find("Vision").gameObject);       // Destory Vision

        // Enemt stays on ground for 2 seconds.
        // After that set all colliders back to false
        // and let body sink throught the floor
        // Then destroy the whole GameObject
        yield return new WaitForSeconds(2);
        SetColliderState(false);
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

    void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach(Collider collider in colliders)
        {
            collider.enabled = state;
            if (collider.name.Equals("Hitbox") || collider.name.Equals("VisionCone"))
            {
                collider.enabled = !state;
            }
        }
    }

}
