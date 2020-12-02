using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class GrenadeScript : MonoBehaviour
{
    public float damage = 20f;
    public List<GameObject> collidingObjects;
    public GameObject explosion;
    
    
    void Start() {
        collidingObjects = new List<GameObject>();
    }
     
    void OnTriggerEnter(Collider collision) {
        if (!collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Add(collision.gameObject);
        }
    } 
     
    void OnTriggerExit(Collider collision) {
        if (collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        foreach (var VARIABLE in collidingObjects)
        {
            var dmg = other.transform.root.GetComponent<IDamage>();
            if (dmg != null)
            {
                other.transform.root.GetComponent<IDamage>().TakeDamage(damage);
            }
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
