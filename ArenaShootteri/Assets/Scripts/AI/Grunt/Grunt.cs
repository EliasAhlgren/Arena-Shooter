using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : MonoBehaviour, IDamage
    {
    /// <summary>
    /// Grunt's Target
    /// </summary>
    public GameObject target { get; private set; }

    // Speed is based on current animation speed. Dont change this value.
    // Change animation speed instead.
    public float walkSpeedBase = 6.0f, runSpeedBase = 20.0f;
    public float walkSpeed, runSpeed;

    // How far away Grunt will start to attack
    public float attackRange = 3;
    // References to NavMeshAgent compontent, "Cone" Transform and Animator component.
    public NavMeshAgent agent;
    public Transform cone;
    public Animator animator;

    
    public float attackCounter = 0f;
    public float attackCooldown = 2f;

    public bool canAttack = true;
    public bool readyToAttack = true;
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
        // Just in case rigidbody and colliders are not enabled
        SetRigidbodyState(true);
        SetColliderState(true);
    }

    private void Awake()
    {
        // Initialize State Machine
        InitStateMachine();

        // Assign the target to be player object
        target = GameObject.FindGameObjectWithTag("Player");

        Debug.Log("Grunt is awake");
    }
    /// <summary>
    ///  Initialize State machine for Grunt
    /// </summary>
    private void InitStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
       {
            // All States for Grunt.
            {typeof(GruntChaseState), new GruntChaseState(_grunt: this) },
            {typeof(GruntAttackState), new GruntAttackState(_grunt: this) },
            {typeof(GruntChargeState), new GruntChargeState(_grunt: this) },
            {typeof(GruntWindUpState), new GruntWindUpState(_grunt: this) },
            {typeof(GruntDoNothingState), new GruntDoNothingState(_grunt:this) }
        };
        GetComponent<Grunt_StateMachine>().SetStates(states);
    }

    /// <summary>
    /// Set target for Grunt
    /// </summary>
    /// <param name="_target">Target's game object</param>
    public void SetTarget(GameObject _target)
    {
        target = _target;
    }
    /// <summary>
    /// Runs Grunt's attack logic
    /// </summary>
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
                canAttack = true;
            }
        }
        // Keep speed updated based on current clip. 
        
        walkSpeed = walkSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed;
        agent.speed = walkSpeed;
        runSpeed = runSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed;

    }

    // AI.IDamage TakeDamage function
    public void TakeDamage(float damage)
    {
        IHealth -= damage;

        if (IHealth <= 0f)
        {
            StartCoroutine(Die());
        }
    }
    /// <summary>
    /// Launch death logic when Grunt dies.
    /// Runs Ragdoll death "animation". Disables AI
    /// and sets layer to "Dead Enemy".
    /// </summary>
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

        // Change layer for enemy and all of it's children to "Dead Enemy" layer.
        // This layer doesnt interact with anything else than the Map itself.
        SetLayerRecursively(transform.gameObject, 9);

        // Enemy stays on ground for 2 seconds.
        // After that set all colliders back to false
        // and let body sink throught the floor
        // Then destroy the whole GameObject
        yield return new WaitForSeconds(2);
        SetColliderState(false);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    /// <summary>
    /// Sets rigidbodys in children to <c>state</c> 
    /// </summary>
    /// <param name="state">Boolean state for the rigidbody </param>
    void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }
    }
    /// <summary>
    /// Sets colliders in children to <c>state</c> 
    /// </summary>
    /// <param name="state">Boolean state for the colliders</param>
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
    /// <summary>
    /// Recursively sets Layer of each Game Object to newLayer
    /// </summary>
    /// <param name="obj">Parent GameObject</param>
    /// <param name="newLayer">number for new layer</param>
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
