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

    public int shellsLeft = 10;
    
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
        //
        //Haulikon lataus ääni tähän
        SoundManager.PlaySound("ShotGunReload");
        //
        _canShoot = true;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(2) && _canShoot && shellsLeft > 0)
        {
            shellsLeft--;
            vfx.Play();
            //
            // Haulikon laukaus ääni tähän
            SoundManager.PlaySound("ShotGunShoot");
            //

            StartCoroutine(shooty());
            foreach (var VARIABLE in collidingObjects)
            {
                var Damageable = VARIABLE.transform.parent.GetComponent<IDamage>();
                if (Damageable != null)
                {
                    Damageable.TakeDamage(30f);
                    Debug.Log(VARIABLE.gameObject + "" + shotgunMod.Damage /
                        Vector3.Distance(transform.position, VARIABLE.transform.position));
                }
            }
        }
    }
}
