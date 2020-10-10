using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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


    public void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Awake()
    {
        InitStateMachine();
        _target = GameObject.Find("Player").gameObject;
        Debug.Log("Enemy is alive");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// method <c>InitStateMachine</c> Initialize State machine for enemy
    /// </summary>
    private void InitStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
       {
            /// Add more states if necessary.
            {typeof(ChaseConcept), new ChaseConcept(enemyConcept: this) }
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

    }

    public void Die()
    {
        // Implement what happens when enemy dies

        Debug.Log(gameObject.ToString() + " died");
        GetComponent<StateMachine>().enabled = false;

    }
}
