using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class EnemyConcept : MonoBehaviour
{
    public GameObject _target { get; private set; }
    public Path path;
    public float speed = 2;
    public float nextWaypointDistance = 3;
    public int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public Seeker seeker;
    public CharacterController controller;
    public float attackRange = 3;
    public NavMeshAgent agent;
    public Transform cone;


    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        cone = transform.Find("ConeShape");
    }
    // Start is called before the first frame update
    public void Awake()
    {
        InitStateMachine();
        _target = GameObject.Find("Player").gameObject;
        Debug.Log("Enemy is alive");

    }

    /// <summary>
    /// method <c>InitStateMachine</c> Initialize State machine for enemy
    /// </summary>
    private void InitStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
       {
            /// Add more states if necessary.
            {typeof(ChaseConcept), new ChaseConcept(enemyConcept: this) },
            {typeof(AttackState), new AttackState(enemyConcept: this) },
            {typeof(ChargeState), new ChargeState(enemyConcept: this) }
        };
        GetComponent<StateMachine>().SetStates(states);
    }
    
     /// <summary>
     /// method <c>SetTarget</c> set target for enemy
     /// </summary>
    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public void Attack()
    {
        // Implement what enemy does when attack happens
        Destroy(_target);

    }

    public void Die()
    {
        // Implement what happens when enemy dies

        Debug.Log(gameObject.ToString() + " died");
        GetComponent<StateMachine>().enabled = false;

    }
}
