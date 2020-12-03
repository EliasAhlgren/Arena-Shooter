using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.VFX;

public class Shooting : MonoBehaviour
{
    public GunAttributes Attributes;
    public float DamageStat;
    public Aiming aiming;

    private PlayerCharacterControllerRigidBody _controller;

    public VisualEffect sparks;
    public ParticleSystem blood;

    public Transform shootPosition;
    void Start()
    {
        Attributes = gameObject.GetComponent<GunAttributes>();
        _controller = GameObject.FindWithTag("Player").GetComponent<PlayerCharacterControllerRigidBody>();
    }

    public void Shoot(float Damage, GameObject target)
    {
        //debug.Log(target + " takes " + Damage + " Damage");
    }

    public void Shoot()
    {
        DamageStat = Attributes.totalDamage * _controller.rageDamageModifier * _controller.damageModifier;
        var shootRay = new Ray(shootPosition.position, transform.forward);
                    RaycastHit hit;
                    if (Physics.Raycast(shootRay, out hit))
                    {
                        var Damageable = hit.transform.root.GetComponent<IDamage>();
                        if (Damageable == null)
                        {
                            sparks.transform.position = hit.point;
                            sparks.transform.up = hit.normal;
                            sparks.Play();
                            //debug.Log("Not damageable " + hit.transform.parent);
                            return;
                        }

                        blood.transform.position = hit.point;
                        blood.transform.forward = hit.normal;

                        Damageable.TakeDamage(DamageStat);
                        //debug.Log(hit.transform.parent + " Has taken " + DamageStat + "DMG " + Damageable.IHealth + " Left");
                    }
    }

    private void Update()
    {
        /*
        DamageStat = Attributes.totalDamage;
        //debug.DrawRay(shootPosition.position, transform.forward * 100,Color.red);
        if (Input.GetButton("Fire1") && !aiming.isReloading && Attributes.ammoInMag > 0)
        {
            Ray shootRay = new Ray(shootPosition.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(shootRay, out hit))
            {
                var Damageable = hit.transform.root.GetComponent<IDamage>();
                if (Damageable == null)
                {
                    //debug.Log("Not damageable " + hit.transform.root.name);
                    return;
                }
                Damageable.TakeDamage(DamageStat);
                //debug.Log(hit.transform.name + " Has taken " + DamageStat + "DMG " + Damageable.IHealth + " Left");
            }
        }
    */
    }
}
