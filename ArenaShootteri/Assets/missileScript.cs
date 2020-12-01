using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.VFX;

public class missileScript : MonoBehaviour
{
    public Vector3 forward;
    private VisualEffect _visualEffect;
    private Transform player;

    public float damage = 2f;   
    public float speed = 8f;
    
    public GameObject Explpoposion;
    
    // Start is called before the first frame update
    void Start()
    {
        //[SOUND]tähän joku looppava rakettiääni
        _visualEffect = gameObject.GetComponentInChildren<VisualEffect>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (!other.gameObject.GetComponent<missileScript>())
        {
            speed = 0f;
            GameObject boom = Instantiate(Explpoposion, transform.position, Quaternion.identity);
            Debug.Log(other.gameObject);
            if (other.gameObject == player.gameObject)
            {
                
                player.GetComponent<PlayerCharacterControllerRigidBody>().TakeDamage(damage, true);
            }

            if (other.transform.parent.GetComponent<IDamage>() != null)
            {
                other.transform.parent.GetComponent<IDamage>().TakeDamage(damage);
            }
            
            //[SOUND] räjähdys tähän

            gameObject.GetComponentInChildren<Renderer>().enabled = false;
            gameObject.GetComponentInChildren<Collider>().enabled = false;
            this.enabled = false;
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
