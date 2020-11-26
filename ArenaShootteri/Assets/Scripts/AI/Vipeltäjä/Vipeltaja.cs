using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.AI;

public class Vipeltaja : MonoBehaviour, IDamage
{    /// <summary>
     /// Vipeltaja's target
     /// </summary>
    public GameObject target { get; private set; }

    // IDamage variable
    public float IHealth { get; set; } = 100f;

    public float damage = 5;
    public bool canAttack = true;

    public GameObject spitPrefab;
    public Transform spitPosition;
    public Rigidbody rb;


    // Speed is based on current animation speed. Dont change this value.
    // Change animation speed instead. // Jump speed not yet implemented.
    public float walkSpeedBase = 6.0f, jumpSpeedBase = 20.0f;
    public float walkSpeed, jumpSpeed;
    public float speed;
    public float turnSpeed = 2f;



    /// <summary>
    /// Attack range of Vipeltaja
    /// </summary>
    public float attackRange = 3;
    public float attackCounter = 0f;
    /// <summary>
    /// <c>Vipeltaja</c> attack cooldown
    /// </summary>
    public float attackCooldown = 2f;
    /// <summary>
    /// <c>Vipeltaja</c> jumping distance 
    /// </summary>
    public float jumpDistance;
    /// <summary>
    /// <c>Vipeltaja</c> Jump cooldown
    /// </summary>
    public float jumpCooldown, jumpTimer = 5f;
    public bool jumpCoolingdown = false;
    /// <summary>
    /// is <c>Vipeltaja</c> ready to attack?
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
        rb = GetComponent<Rigidbody>();

        // These might not be necessary
        SetRigidbodyState(true);
        setColliderState(true);
    }

    private void Awake()
    {
        // Initialize State Machine
        InitStateMachine();

        // Assign the target to be player object
        target = GameObject.FindGameObjectWithTag("Player");

        // what for... Not really sure. 
        agent.SetDestination(target.transform.position);

        Debug.Log("Vipeltäjä is awake");
    }

    private void InitStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
       {
            // All States for Vipeltaja.
            {typeof(VipeltajaChaseState), new VipeltajaChaseState(_vipeltaja: this) },
            {typeof(VipeltajaAttackState), new VipeltajaAttackState(_vipeltaja: this) },
            {typeof(VipeltajaDoNothingState), new VipeltajaDoNothingState(_vipeltaja: this) },
            {typeof(VipeltajaEscapeState), new VipeltajaEscapeState(_vipeltaja: this) },
            {typeof(VipeltajaSpitState), new VipeltajaSpitState(_vipeltaja: this) },
            {typeof(VipeltajaJumpState), new VipeltajaJumpState(_vipeltaja: this) }
        };
        GetComponent<Vipeltaja_StateMachine>().SetStates(states);
    }

    public void Update()
    {
        animator.SetFloat("velocity", agent.velocity.magnitude / agent.speed);
        // Track if vipeltaja is ready to attack
        if (!readyToAttack)
        {
            attackCounter += Time.deltaTime;
            if (attackCounter > attackCooldown)
            {
                readyToAttack = true;
                canAttack = true;
                attackCounter = 0f;
            }
        }

        // Track if vipeltaja can jump
        if(jumpCoolingdown)
        {
            jumpTimer -= Time.deltaTime;
            if(jumpTimer <= 0)
            {
                jumpCoolingdown = false;
                jumpTimer = 5f;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Die();
        }

        walkSpeed = walkSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed;
        agent.speed = walkSpeed;
        jumpSpeed = jumpSpeedBase * animator.GetCurrentAnimatorStateInfo(0).speed;


    }

    /// <summary>
    /// method <c>SetTarget</c> set target for Vipeltaja
    /// </summary>
    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    /// <summary>
    /// Launch death logic when Vipeltaja dies.
    /// Runs Ragdoll death "animation". Disables AI
    /// and sets layer to "Dead Enemy".
    /// </summary>

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

        // Change layer for enemy and all of it's children to "Dead Enemy" layer.
        // This layer doesnt interact with anything else than the Map itself.
        int layerMask = (int)Mathf.Log(LayerMask.GetMask("DeadEnemy"), 2);
        SetLayerRecursively(transform.gameObject, layerMask);

        // tell other Vipeltaja enemies that this unit is dead. other should check if they are alone
        GameObject[] vipeltajat = GameObject.FindGameObjectsWithTag("Vipeltaja");
        foreach (GameObject vipeltaja in vipeltajat)
        {
            if (vipeltaja.gameObject != this.gameObject)
            {
                vipeltaja.GetComponent<Vipeltaja>().IsAlone();
            }
        }

        // Enemy stays on ground for 2 seconds.
        // After that set all colliders back to false 
        // and let body sink throught the floor
        // Then destroy the whole GameObject
                yield return new WaitForSeconds(2);
        setColliderState(false);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);


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
    /// force Vipeltaja to escape state
    /// </summary>
    public void GetFeared()
    {
        GetComponent<Vipeltaja_StateMachine>().CancelInvoke(GetComponent<Vipeltaja_StateMachine>().currentState.ToString());
        GetComponent<Vipeltaja_StateMachine>().SwitchToState(typeof(VipeltajaEscapeState));
    }
    /// <summary>
    /// Used to spawn Spit object. 
    /// </summary>
    /// !!!NEEDS MODIFICATIONS!!!

    public void SpawnSpit()
    {
        Vector3 ballisticVelocity = BallisticVelocity(target.transform, 15);
        animator.Play("Spit");
        GameObject spit = Instantiate(spitPrefab, spitPosition.position, Quaternion.identity);
        spit.GetComponent<Rigidbody>().velocity = ballisticVelocity;

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
    /// Count ballistics for spit gameobject.
    /// </summary>
    /// <param name="target">where spit should land</param>
    /// <param name="angle">angle used to launch spit</param>
    /// <returns>Velocity for spit so it lands on player</returns>
    public Vector3 BallisticVelocity(Transform target, float angle)
    {
        Vector3 direction = target.position - transform.position;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float rad = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(rad);
        distance += h / Mathf.Tan(rad);

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * rad));
        return velocity * direction.normalized;

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

    public void IsAlone()
    {
        int nearbyEnemies = 0;
        GameObject[] vipeltajat = GameObject.FindGameObjectsWithTag("Vipeltaja");
        
        foreach (GameObject vipeltaja in vipeltajat)
        {
            if(vipeltaja.gameObject != this.gameObject && Vector3.Distance(vipeltaja.transform.position, gameObject.transform.position) < 20)
            {
                nearbyEnemies++;
            }
        }

        if (nearbyEnemies < 2)
        {
            GetFeared();
        }
    }
}    



