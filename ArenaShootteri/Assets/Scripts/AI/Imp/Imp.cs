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
    public float IHealth { get; set; } = 20f;
    public bool immune = true;

    public bool canAttack = true;

    public float walkSpeedBase = 1f;

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

        StartCoroutine("SpawnImmunity");
    }

    private void Awake()
    {
        // Initialize State Machine
        InitStateMachine();

        // Assign the target to be player object
        target = GameObject.FindGameObjectWithTag("Player");

        // what for... Not really sure. 
        agent.SetDestination(target.transform.position);
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

    private IEnumerator SpawnImmunity()
    {
        var immunityTime = 15 / (walkSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed);
        yield return new WaitForSeconds(immunityTime);
        immune = false;
        StopCoroutine("SpawnImmunity");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("velocity", agent.velocity.magnitude / agent.speed);
        if (!readyToAttack)
        {
            attackCounter += Time.deltaTime;
            if (attackCounter >= attackCooldown)
            {
                readyToAttack = true;
                canAttack = true;
                attackCounter = 0f;
            }
        }
        speed = walkSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed;
        agent.speed = speed;

    }

    public void Attack()
    {
        var distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < attackRange)
        {
            target.GetComponent<PlayerCharacterControllerRigidBody>().TakeDamage(damage, false);
        }

    }

    public void TakeDamage(float damage)
    {
        if (!immune)
        {
            animator.SetTrigger("TakesDamage");
            IHealth -= damage;
        }

        if (IHealth <= 0)
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

    void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (!collider.name.Equals("Identifier"))
            {
                collider.enabled = state;
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
