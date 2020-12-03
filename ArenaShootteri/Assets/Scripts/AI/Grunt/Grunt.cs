using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Grunt : MonoBehaviour, IDamage
    {
    /// <summary>
    /// Grunt's Target
    /// </summary>
    public GameObject target { get; private set; }

    // Speed is based on current animation speed. Dont change this value.
    // Change animation speed instead.
    private protected float walkSpeedBase = 6.0f, runSpeedBase = 20.0f;
    public float walkSpeed, runSpeed;

    public float gravityScale;
    // How far away Grunt will start to attack
    public float attackRange = 3;
    // References to NavMeshAgent compontent, "Cone" Transform and Animator component.
    public NavMeshAgent agent;
    public Transform cone;
    public Animator animator;

    public float attackCounter = 0f;
    public float attackCooldown = 2f;

    public float damage = 5;
    public bool canAttack = true;
    public bool readyToAttack = true;

    public float chargeDamage = 20;
    public bool isCharging = false;
    public float chargeForce = 10;
    
    public float IHealth { get; set; } = 250f;
    public bool immune = true;

    //sounds
    public static AudioClip shout, footsteps, murina, hit, death;
    static AudioSource audioSrc;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cone = transform.Find("Vision");

        // These might not be necessary
        // Just in case rigidbody and colliders are not enabled
        SetRigidbodyState(true);
        SetColliderState(true);

        //sounds
        shout = Resources.Load<AudioClip>("MonsterShout1");
        footsteps = Resources.Load<AudioClip>("gruntfootstep1");
        murina = Resources.Load<AudioClip>("MonsterShout2");
        hit = Resources.Load<AudioClip>("MonsterHurt1");
        death = Resources.Load<AudioClip>("MonsterBreath1");

        audioSrc = GetComponent<AudioSource>();

        StartCoroutine("SpawnImmunity");
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

    private IEnumerator SpawnImmunity()
    {
        var immunityTime = 15 / (walkSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed);
        yield return new WaitForSeconds(immunityTime);
        immune = false;
        StopCoroutine("SpawnImmunity");
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
    /// </summary>   MAYBE OBSOLETE
    public void Attack()
    {
        Debug.Log("Melee attack");

        var distance = Vector3.Distance(transform.position, target.transform.position);
        Debug.Log("Distance is " + distance + " compared to attack range " + attackRange);
        if(distance < attackRange)
        {
            target.GetComponent<PlayerCharacterControllerRigidBody>().TakeDamage(damage, false);
        }

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
        if (!immune)
        {
            IHealth -= damage;
        }

        PlaySound("hit", GetComponent<AudioSource>());

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
        PerkTreeReader.Instance.AddPerkPoint(5);
        target.GetComponent<PlayerCharacterControllerRigidBody>().AddRageKill();

        // Disable rigidbody and enable Colliders for each body part
        // for rigidbody death "animation"
        SetRigidbodyState(false);
        SetColliderState(true);
        PlaySound("death", GetComponent<AudioSource>());

        // Disable all AI components for the Grunt.
        animator.enabled = false;                           // Stop animator
        agent.enabled = false;                              // Stop Nav Mesh Agent
        GetComponent<Grunt_StateMachine>().enabled = false;       // Stop AI
                                                                  // Destroy(transform.Find("Vision").gameObject);       // Destory Vision

        // Change layer for enemy and all of it's children to "Dead Enemy" layer.
        // This layer doesnt interact with anything else than the Map itself.
        int layerMask = (int)Mathf.Log(LayerMask.GetMask("DeadEnemy"), 2);
        SetLayerRecursively(transform.gameObject, layerMask);

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

    public static void PlaySound(string clip, AudioSource audioSorsa)
    {
        switch (clip)
        {
            case "shout":
                audioSorsa.Stop();
                audioSorsa.loop = false;
                audioSorsa.clip = shout;
                audioSorsa.Play();
                break;
            case "hit":
                audioSorsa.Stop();
                audioSorsa.loop = false;
                audioSorsa.clip = hit;
                audioSorsa.Play();
                break;
            case "murina":
                audioSorsa.Stop();
                audioSorsa.loop = false;
                audioSorsa.clip = murina;
                audioSorsa.Play();
                break;
            case "footsteps":
                audioSorsa.Stop();
                audioSorsa.loop = false;
                audioSorsa.clip = footsteps;
                Debug.Log("WalkStep");
                audioSorsa.pitch = UnityEngine.Random.Range(0.9f - 0.05f, 0.9f + 0.05f);
                audioSorsa.Play();
                break;
            case "death":
                audioSorsa.Stop();
                audioSorsa.loop = false;
                audioSorsa.clip = death;
                audioSorsa.Play();
                break;
        }
    }

}



