using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using Random = UnityEngine.Random;


public class LentoSaatana : MonoBehaviour, IDamage
    {
        public float moveSpeed;

        public bool atPosition;

        public bool canSee;
        
        public State state;

        public GameObject deathExplosion;

        public float IHealth { get; set; } = 30f;

        public enum State
        {
            DoNothing,
            Searching,
            Travelling,
            Targeting,
            Shooting,
            Waiting
        }

        public GameObject missile;
        
        public Transform[] spawns;
        
        
        
        public Vector3 nextPosition;

        private Rigidbody _rigidbodyb;

        private CreateNodes positionObject;

        public Vector3[] positions;
        
        public GameObject player;

        private Animator _animator;

        private int layerMask;

        private bool spawnIndex;

        public void TakeDamage(float damage)
        {
            IHealth -= damage;

            if (IHealth <= 0f)
            {
                Die();
            }
        }

        
        public void Die()
        {
            PerkTreeReader.Instance.AddPerkPoint(3);
            player.GetComponent<PlayerCharacterControllerRigidBody>().AddRageKill();
            Instantiate(deathExplosion, transform.position, Quaternion.identity);
            state = State.DoNothing;
            Destroy(gameObject);
        }
        
        private void Start()
        {
            Initialize();
            //[SOUND] tähä se demoninen humina loopilla
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Point"))
            {
                if (other.transform.position == nextPosition)
                {
                    //debug.Log("At position");
                    atPosition = true;
                }
            }
        }

        private void Initialize()
        {
            player = GameObject.FindWithTag("Player");
            
            positionObject = FindObjectOfType<CreateNodes>();
            
            positions = positionObject.positions;

            _rigidbodyb = gameObject.GetComponent<Rigidbody>();

            _animator = gameObject.GetComponent<Animator>();

            layerMask = LayerMask.GetMask("Enemy");

            layerMask = ~layerMask;
            
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
                if (!Physics.Linecast(transform.position,position) && !Physics.Raycast(position, Vector3.up, 40f))
                {
                    float dist = Vector3.Distance(position, player.transform.position);
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
                atPosition = false;
                //debug.Log("New Pos found");
                state = State.Travelling;
            }
            else
            {
                //debug.Log("New Pos not found");
                state = State.Waiting;
            }
            
        }

        private void FixedUpdate()
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.transform.position, out hit) )
            {
                if (hit.collider.gameObject == player)
                {
                    canSee = true;
                }
                else
                {
                    canSee = false;
                }
            }

            
            
            switch (state)
            {
                case State.DoNothing:
                    transform.LookAt(player.transform);
                    break;
                case State.Searching:
                    GetNewPosition();
                    break;
                case State.Travelling:
                    Travelling();
                    break;
                case State.Targeting:
                    //debug.Log(state);
                    StartCoroutine(Targeting());
                    break;
                case State.Shooting:
                    StartCoroutine(Shooting());
                    state = State.DoNothing;
                    break;
                case State.Waiting:
                    if (canSee)
                    {
                        state = State.Searching;
                    }
                    break;
            }
        }

        private void Travelling()
        {
            state = State.DoNothing;
            Vector3 move = (nextPosition - transform.position);
            if (!atPosition)
            {
                transform.Translate(move * (moveSpeed / 10) ,Space.World);
                state = State.Travelling;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, player.transform.position, out hit) )
                {
                    if (hit.collider.gameObject == player)
                    {
                        //debug.Log("targeting");
                        state = State.Targeting;
                    }else
                    {
                        //debug.Log("Njet comrade" + hit.collider.gameObject);
                        nextPosition = positions[Random.Range(0, positions.Length)];
                        atPosition = false;
                        state = State.Travelling;
                        
                    }
                    
                }
                
            }
        }
        
        
        private IEnumerator Targeting()
        {
            transform.LookAt(player.transform);
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Targeting"))
            {
                _animator.Play("Targeting");
            }
            state = State.DoNothing;
            yield return new  WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            //debug.Log("1");
            RaycastHit hit;
            if (canSee)
            {
                state = State.Shooting;
            }
            else
            {
                state = State.Searching;
            }
        }

        private IEnumerator Shooting()
        {
            _animator.Play("Shooting");
            yield return new WaitForSeconds(0.2f);
            spawnIndex = !spawnIndex;
            Instantiate(missile, spawns[System.Convert.ToInt32(spawnIndex)].position, Quaternion.identity);
            SoundManager.PlaySound("RocketShot");
            yield return new WaitForSeconds(0.2f);
            spawnIndex = !spawnIndex; 
            Instantiate(missile, spawns[System.Convert.ToInt32(spawnIndex)].position, Quaternion.identity);
            SoundManager.PlaySound("RocketShot");
            yield return new WaitForSeconds(0.2f);
            spawnIndex = !spawnIndex;
            Instantiate(missile, spawns[System.Convert.ToInt32(spawnIndex)].position, Quaternion.identity);
            SoundManager.PlaySound("RocketShot");
            yield return new WaitForSeconds(0.2f);
            spawnIndex = !spawnIndex;
            Instantiate(missile, spawns[System.Convert.ToInt32(spawnIndex)].position, Quaternion.identity);
            SoundManager.PlaySound("RocketShot");
            _animator.Play("Idle");
            state = State.Searching;
        }
        
        
    }
