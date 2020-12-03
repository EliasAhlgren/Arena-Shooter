using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.AI;

public class LentoVipeltaja : MonoBehaviour, IDamage
{
     /// <summary>
     /// Vipeltaja's target
     /// </summary>
    public GameObject target { get; private set; }

    // IDamage variable
    public float IHealth { get; set; } = 10f;

    public float damage = 1;

    public Rigidbody rb;

    // Speed is based on current animation speed. Dont change this value.
    // Change animation speed instead. // Jump speed not yet implemented.
    public float walkSpeedBase = 6.0f, jumpSpeedBase = 20.0f;
    public float speed, turnSpeed = 2f;

    public float attackRange = 2;
    public float attackCounter = 0f;
    public float attackCooldown = 2f;
    public bool readyToAttack = true;
    public bool drainPlayer = true;

    public NavMeshAgent agent;
    public Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }

    private void Awake()
    {
        InitStateMachine();

        target = GameObject.FindGameObjectWithTag("Player");

        agent.SetDestination(target.transform.position);

    }

    private void InitStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            { typeof(LentoVipeltajaChaseState), new LentoVipeltajaChaseState(_Lvipeltaja: this) },
            { typeof(LentoVipeltajaAttackState), new LentoVipeltajaAttackState(_Lvipeltaja: this) }
        };
        GetComponent<LentoVipeltaja_StateMachine>().SetStates(states);
   
    }

    public void TakeDamage(float damage)
    {
        IHealth -= damage;

        if (IHealth <= 0f)
        {
            StartCoroutine(Die());
        }
    }

    public void Update()
    {
        if (!drainPlayer)
        {
            attackCounter += Time.deltaTime;
            if(attackCounter > attackCooldown)
            {
                drainPlayer = true;
                attackCounter = 0f;
            }
        }

    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public IEnumerator Die()
    {
        // Disable rigidbody and enable Colliders for each body part
        // for rigidbody death "animation"
        SetRigidbodyState(false);
        setColliderState(true);

        // Disable all AI components for the Vipeltäjä.
        animator.enabled = false;                           // Stop animator
        agent.baseOffset = 0.0f;
        agent.enabled = false;                              // Stop Nav Mesh Agent
        GetComponent<LentoVipeltaja_StateMachine>().enabled = false;       // Stop AI

        // Change layer for enemy and all of it's children to "Dead Enemy" layer.
        // This layer doesnt interact with anything else than the Map itself.
        int layerMask = (int)Mathf.Log(LayerMask.GetMask("DeadEnemy"), 2);
        SetLayerRecursively(transform.gameObject, layerMask);

        // Enemy stays on ground for 2 seconds.
        // After that set all colliders back to false 
        // and let body sink throught the floor
        // Then destroy the whole GameObject
        yield return new WaitForSeconds(2);
        setColliderState(false);
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

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }
    }
    /// <summary>
    /// Sets colliders in children to <c>state</c> 
    /// </summary>
    /// <param name="state">Boolean state for the colliders</param>
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
    /// <summary>
    /// Recursively sets Layer of each Game Object to newLayer
    /// </summary>
    /// <param name="obj">Parent GameObject</param>
    /// <param name="newLayer">number for new layer</param>
    public void SetLayerRecursively(GameObject obj, int newLayer)
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

    public bool IsFacingPlayer(float desiredAngle)
    {
        // Dot product used to determine if enemy is facing the player. return 1 if facing player, return 0 at 90 degree angle.
        float dot = Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized);

        if (dot > desiredAngle)
        {
            return true;
        }
        else return false;
    }
}