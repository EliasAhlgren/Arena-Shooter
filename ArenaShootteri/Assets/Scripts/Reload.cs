using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Reload : MonoBehaviour
{
    public Aiming aiming;
    public bool isReloading;
    public GunAttributes attributes;
    public Transform reloadPos;
    public Recoil recoil;
    public Transform target;
    
    private float i;
    private void Start()
    {
        aiming = gameObject.GetComponent<Aiming>();
    }

    private void Update()
    {
        
    }

    public IEnumerator ReloadGun()
    {
        aiming.isAiming = false;
        recoil.DisableLazyGun = true;
        target.transform.position = new Vector3(0,0,0);
        yield return i;
    }
}
