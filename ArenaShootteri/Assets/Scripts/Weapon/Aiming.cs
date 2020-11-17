﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{

    private ModSelection SightSelection;
    
    public Transform playerCamera;

    public Transform target;
    
    public bool isAiming;

    public float positionDifferenceY;

    public float positionDifferenceZ;
    
    private Vector3 defaultPosition;

    private Recoil _recoil;

    public float defaultDifference;

    public bool isReloading;

    public Transform reloadPos;
    // Start is called before the first frame update
    void Start()
    {
        SightSelection = GameObject.Find("SightSelection").GetComponent<ModSelection>();
        _recoil = gameObject.GetComponent<Recoil>();
        defaultPosition = target.localPosition;    
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            _recoil.DisableLazyGun = true;
            //_recoil.RecoilAmount = _recoil.RecoilAmount / 2;
            StartCoroutine(ReloadDelay(2f));
        }
        if (isReloading)
        {
            Reload();
        }
        
        
        
        if (Input.GetButtonDown("Fire2"))
        {
            isAiming = !isAiming;
            _recoil.DisableLazyGun = true;
            //_recoil.RecoilAmount = _recoil.RecoilAmount / 2;
        }
        if (isAiming)
        {
            AimDownSights();
        }
        

        if (!isAiming && !isReloading)
        {
            _recoil.DisableLazyGun = false;
            target.localPosition = defaultPosition;

        }
        
    }

    private void Reload()
    {
        Debug.Log("is reloading");
        
        _recoil.DisableLazyGun = true;

        target.transform.localPosition = reloadPos.localPosition;
    }

    public IEnumerator ReloadDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (_recoil._GunAttributes.totalAmmo >= 30)
        {
            _recoil._GunAttributes.ammoInMag = 30;
            _recoil._GunAttributes.totalAmmo -= 30;
        }
        else
        {
            _recoil._GunAttributes.ammoInMag = _recoil._GunAttributes.totalAmmo;
            _recoil._GunAttributes.totalAmmo = 0;
        }
        
        isReloading = false;
    }
    
    private void AimDownSights()
    {
        _recoil.DisableLazyGun = true;

        if (SightSelection.currenModStats)
        {
            positionDifferenceY = SightSelection.currenModStats.AimHeight;
        }
        else
        {
            positionDifferenceY = defaultDifference;
        }
        
        
        //target.transform.localPosition = reloadPos.localPosition;

        target.localPosition = new Vector3(0, positionDifferenceY, defaultPosition.z + positionDifferenceZ);
        
    }
    
}