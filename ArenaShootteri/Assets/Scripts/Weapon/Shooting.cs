using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GunAttributes Attributes;
    public float DamageStat;

    void Start()
    {
        Attributes = gameObject.GetComponent<GunAttributes>();
    }

    public void Shoot(float Damage, GameObject target)
    {
        Debug.Log(target + " takes " + Damage + " Damage");
    }
    
    void Update()
    {
        DamageStat = Attributes.totalDamage;
        
        if (Input.GetButton("Fire1"))
        {
            Ray shootRay = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(shootRay, out hit))
            {
                var Damageable = hit.transform.parent.GetComponent<IDamage>();
                if (Damageable == null)
                {
                    Debug.Log("Not damageable " + hit.transform.parent);
                    return;
                }
                Damageable.TakeDamage(DamageStat);
                Debug.Log(hit.transform.parent + " Has taken " + DamageStat + "DMG " + Damageable.IHealth + " Left");
            }
        }
    }
}
