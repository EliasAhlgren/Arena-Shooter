using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
        
    public class LentoSaatana : MonoBehaviour
    {
        public float moveSpeed;
        
        public enum State
        {
            DoNothing,
            Searching,
            Travelling,
            Targeting,
            Shooting,
        }

        public State state;
        
        public Vector3 nextPosition;

        private Rigidbody _rigidbodyb;

        private CreateNodes positionObject;

        public Vector3[] positions;
        
        public GameObject player;
        private void Start()
        {
            Initialize();
            
            
        }

        private void Initialize()
        {
            player = GameObject.FindWithTag("Player");
            
            positionObject = FindObjectOfType<CreateNodes>();
            
            positions = positionObject.positions;

            _rigidbodyb = gameObject.GetComponent<Rigidbody>();
            
            if (positionObject.positions.Length > 0)
            {
                GetNewPosition();
            }
            else
            {
                Initialize();
            }
            
        }

        private void GetNewPosition()
        {
            Vector3 closestPoint = Vector3.zero;
            float closestDistance = 10000f;
            foreach (var position in positions)
            {
                if (!Physics.Linecast(transform.position,position))
                {
                    float dist = Vector3.Distance(transform.position, player.transform.position);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        closestPoint = position;
                    }

                }
                
            }
            nextPosition = closestPoint;
            if (closestPoint != Vector3.zero)
            {
                Debug.Log("New Pos found");
                state = State.Travelling;
            }
            else
            {
                Debug.Log("New Pos not found");
                state = State.DoNothing;
            }
            
        }

        private void FixedUpdate()
        {
            switch (state)
            {
                case State.DoNothing:
                    break;
                case State.Searching:
                    break;
                case State.Travelling:
                    state = State.DoNothing;
                    Vector3 move = (nextPosition - transform.position);
                    if (transform.position != nextPosition)
                    {
                        transform.LookAt(nextPosition);
                        transform.Translate(transform.forward * (moveSpeed / 10));
                        state = State.Travelling;
                    }
                    break;
            }
        }
    }
