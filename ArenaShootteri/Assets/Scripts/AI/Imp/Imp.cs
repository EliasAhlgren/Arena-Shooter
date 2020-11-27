using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.AI;

public class Imp : MonoBehaviour, IDamage
{
    /// <summary>
    /// Imp's target
    /// </summary>
    public GameObject target { get; set; }

    // IDamage variable
    public float IHealth { get; set; } = 100f;

    public bool canAttack = true;

    public float walkSpeedBase = 5f;
    public float speed;

    public float damage = 5;
    /// <summary>
    /// Imp's attack range
    /// </summary>
    public float attackRange = 3;
    public float attackCounter = 0f;
    /// <summary>
    /// <c>Imp</c> attack cooldown
    /// </summary>
    public float attackCooldown = 2f;
    /// <summary>
    /// <c>Imp</c> jumping distance 
    /// </summary>
    public float jumpDistance;
    /// <summary>
    /// <c>Imp</c> Jump cooldown
    /// </summary>
    public float jumpCooldown;
    /// <summary>
    /// is <c>Imp</c> ready to attack?
    /// </summary>
    public bool readyToAttack = true;
    /// <summary>
    /// Reference to NavMeshAgent component
    /// </summary>
    public NavMeshAgent agent;
    /// <summary>
    /// Reference to Animator compontent
    /// </summary>
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        // Initialize State Machine
        InitStateMachine();

        // Assign the target to be player object
        target = GameObject.FindGameObjectWithTag("Player");

        // what for... Not really sure. 
        agent.SetDestination(target.transform.position);

        Debug.Log("Imp is awake.");
    }

    private void InitStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            // All states for 
            {typeof(ImpChaseState), new ImpChaseState(_imp: this) },
            {typeof(ImpAttackState), new ImpAttackState(_imp: this) }
        };
        GetComponent<Imp_StateMachine>().SetStates(states);
    }

    // Update is called once per frame
    void Update()
    {
        if(!readyToAttack)
        {
            attackCounter += Time.deltaTime;
            if(attackCounter >= attackCooldown)
            {
                readyToAttack = true;
                canAttack = true;
                attackCounter = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        animator.SetTrigger("TakesDamage");
        IHealth -= damage;

        if(IHealth <= 0)
        {
            StartCoroutine(Die());
        }
        animator.ResetTrigger("TakesDamage");
    }

    public IEnumerator Die()
    {

        animator.SetBool("Dead", true);
        SetColliderState(true);

        
        agent.enabled = false;
        GetComponent<Imp_StateMachine>().enabled = false;

        // Change layer for enemy and all of it's children to "Dead Enemy" layer.
        // This layer doesnt interact with anything else than the Map itself.
        int layerMask = (int)Mathf.Log(LayerMask.GetMask("DeadEnemy"), 2);
        SetLayerRecursively(transform.gameObject, layerMask);

        yield return new WaitForSeconds(2);
        SetColliderState(false);
        animator.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    //void SetRigidbodyState(bool state)
    //{
    //    Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

    //    foreach (Rigidbody rb in rigidbodies)
    //    {
    //        rb.isKinematic = state;
    //    }
    //}

    void SetColliderState(bool state)
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
}
