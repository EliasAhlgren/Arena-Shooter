using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vipeltaja : MonoBehaviour
{
    public GameObject target { get; private set; }
    public GameObject spitPrefab;
    public Transform spitPosition;
    public Rigidbody rb;
    public float speed = 1;
    public float attackRange = 3;
    public float attackCounter = 0f;
    public float attackCooldown = 2f;
    public float jumpDistance;
    public float jumpSpeed;
    public float jumpCooldown;
    public bool readyToAttack = true;
    public NavMeshAgent agent;
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
            {typeof(VipeltajaDoNothingState), new VipeltajaDoNothingState(_vipeltaja: this) },
            {typeof(VipeltajaEscapeState), new VipeltajaEscapeState(_vipeltaja: this) },
            {typeof(VipeltajaSpitState), new VipeltajaSpitState(_vipeltaja: this) },
            {typeof(VipeltajaJumpState), new VipeltajaJumpState(_vipeltaja: this) }
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

    public void SpawnSpit()
    {
        Vector3 ballisticVelocity = BallisticVelocity(target.transform, 15);
        animator.Play("Spit");
        GameObject spit = Instantiate(spitPrefab, spitPosition.position, Quaternion.identity);
        spit.GetComponent<Rigidbody>().velocity = ballisticVelocity;
        
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
}
