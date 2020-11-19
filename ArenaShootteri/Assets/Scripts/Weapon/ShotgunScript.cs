using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class ShotgunScript : MonoBehaviour
{
    public Mod shotgunMod;

    public List<GameObject> collidingObjects;
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

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var VARIABLE in collidingObjects)
            {
                VARIABLE.GetComponent<IDamage>().TakeDamage(shotgunMod.Damage / Vector3.Distance(transform.position,VARIABLE.transform.position));
                Debug.Log( VARIABLE.gameObject + "" + shotgunMod.Damage / Vector3.Distance(transform.position,VARIABLE.transform.position));
            }
        }
    }
}
