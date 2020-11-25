using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class ShotgunScript : MonoBehaviour
{
    public Mod shotgunMod;
    public VisualEffect vfx;

    private bool _canShoot = true;
    
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

    IEnumerator shooty()
    {
        _canShoot = false;
        yield return new WaitForSeconds(3f);
        _canShoot = true;
    }
    
    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire3") && _canShoot)
        {
            vfx.Play();
            StartCoroutine(shooty());
            foreach (var VARIABLE in collidingObjects)
            {
                var Damageable = VARIABLE.transform.parent.GetComponent<IDamage>();
                if (Damageable != null)
                {
                    Damageable.TakeDamage(shotgunMod.Damage / Vector3.Distance(transform.position, VARIABLE.transform.position));
                    Debug.Log(VARIABLE.gameObject + "" + shotgunMod.Damage /
                        Vector3.Distance(transform.position, VARIABLE.transform.position));
                }
            }
        }
    }
}
