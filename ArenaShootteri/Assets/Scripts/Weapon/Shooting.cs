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

    public VisualEffect sparks;
    public ParticleSystem blood;

    public Transform shootPosition;
    void Start()
    {
        Attributes = gameObject.GetComponent<GunAttributes>();
    }

    public void Shoot(float Damage, GameObject target)
    {
        Debug.Log(target + " takes " + Damage + " Damage");
    }

    public void Shoot()
    {
         DamageStat = Attributes.totalDamage;
        var shootRay = new Ray(shootPosition.position, transform.forward);
                    RaycastHit hit;
                    if (Physics.Raycast(shootRay, out hit))
                    {
                        var Damageable = hit.transform.parent.GetComponent<IDamage>();
                        if (Damageable == null)
                        {
                            sparks.transform.position = hit.point;
                            sparks.transform.up = hit.normal;
                            sparks.Play();
                            Debug.Log("Not damageable " + hit.transform.parent);
                            return;
                        }

                        blood.transform.position = hit.point;
                        blood.transform.forward = hit.normal;

                        Damageable.TakeDamage(DamageStat);
                        Debug.Log(hit.transform.parent + " Has taken " + DamageStat + "DMG " + Damageable.IHealth + " Left");
                    }
    }

    private void Update()
    {
        /*
        DamageStat = Attributes.totalDamage;
        Debug.DrawRay(shootPosition.position, transform.forward * 100,Color.red);
        if (Input.GetButton("Fire1") && !aiming.isReloading && Attributes.ammoInMag > 0)
        {
            Ray shootRay = new Ray(shootPosition.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(shootRay, out hit))
            {
                var Damageable = hit.transform.root.GetComponent<IDamage>();
                if (Damageable == null)
                {
                    Debug.Log("Not damageable " + hit.transform.root.name);
                    return;
                }
                Damageable.TakeDamage(DamageStat);
                Debug.Log(hit.transform.name + " Has taken " + DamageStat + "DMG " + Damageable.IHealth + " Left");
            }
        }
    */
    }
}
