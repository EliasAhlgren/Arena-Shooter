using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.VFX;

public class missileScript : MonoBehaviour, IDamage
{
    public Vector3 forward;
    private VisualEffect _visualEffect;
    private Transform player;

    public float damage = 2f;   
    public float speed = 8f;
    
    public GameObject Explpoposion;

    public float IHealth { get; set; } = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //[SOUND]tähän joku looppava rakettiääni
        _visualEffect = gameObject.GetComponentInChildren<VisualEffect>();
        player = GameObject.FindWithTag("Player").transform;
    }

    public void TakeDamage(float damage)
    {
        
        Explode(GameObject.FindWithTag("Level"));
    }

    private void OnCollisionEnter(Collision other)
    {
        Explode(other.gameObject);
    }

    private void Explode(GameObject other)
    {
        if (!other.GetComponent<missileScript>())
        {
            GameObject boom = Instantiate(Explpoposion, transform.position, Quaternion.identity);
            if (other == player.gameObject)
            {
                player.GetComponent<PlayerCharacterControllerRigidBody>().TakeDamage(damage, true);
            }


            var dmg = other.transform.root.GetComponent<IDamage>();
            if (dmg != null)
            {
                other.transform.root.GetComponent<IDamage>().TakeDamage(damage);
            }


            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        _visualEffect.Play();
        forward = transform.forward;

        Vector3 direction = (player.position - transform.position).normalized;
        Debug.DrawRay(transform.position, direction,Color.red, 20f);
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
    }
}
