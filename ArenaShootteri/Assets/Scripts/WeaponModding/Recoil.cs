﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Recoil : MonoBehaviour
{
    
    public bool isRecoiling;
    
    public float recoilTime;

    public float RateOfFire = 1f;

    public float RecoilAmount = 1f;

    public float horizontalRecoil;
    
    // How much the head bobs when firing
    public float FeltRecoil = 1f;
    
    public GunAttributes _GunAttributes;
    
    // Gun position target
    public Transform target;

    // how fast the gun rotates to target rotation
    public float rotationLazyness;

    // how fast the recoil decays
    public float ReturnTime;
    
    // Aiming point
    public Transform lookatPoint;

    public GameObject debugCube;

    private Vector3 newpoint;

    public Transform cameraTransform;

    public float timer;
    
    private Vector3 defaultPos;

    public bool showRecoil;
    
    // Start is called before the first frame update
    void Start()
    {
        
        defaultPos = lookatPoint.localPosition;
        
        newpoint = lookatPoint.position;
        
        _GunAttributes = gameObject.GetComponent<GunAttributes>();

        transform.position = target.position;
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * RateOfFire;

        // resets the timer
        if (timer > 100000)
        {
            timer = 0;
        }
        
        LazyGun();

        if (Input.GetMouseButton(0) && !_GunAttributes.isModding)
        {
            Recoiling();
        }
        else
        {
            // return back to normal position
            lookatPoint.localPosition += (defaultPos - lookatPoint.localPosition) * ReturnTime;
        }
    }

    

    public void LazyGun()
    {
        if (!_GunAttributes.isModding && !_GunAttributes.isAiming)
        {
            
            transform.position = target.position;

            var position = lookatPoint.position;
            
            // newpoint += differnce between aiming point and itself * rotationLazyness ( around 0.5 in normal setup)
            newpoint += (position - newpoint) * rotationLazyness;

            newpoint.x = Mathf.Clamp(newpoint.x, position.x - 10f, position.x + 10f);

            if (showRecoil)
            {
                debugCube.transform.position = newpoint;
            }
            
            
            transform.LookAt(newpoint);
        }

        
        

    }
    
    public void Recoiling()
    {
        isRecoiling = true;
        
        if (recoilTime < 1)
        {
            recoilTime += Time.deltaTime / 2;    
        }
        else
        {
            recoilTime = 0;
        }
        
        
        
        var localRotation = cameraTransform.localRotation;
        
        // get a positive only sine wave and add perlin noise sample
        float sineValue =  Mathf.Sin(timer / 2);
        if (sineValue < 0)
        {
            sineValue += -sineValue;
            sineValue += Mathf.PerlinNoise(timer, Random.Range(0, 50)) * 2;
        }
        
        //headbobbipn when firing
        localRotation.eulerAngles = new Vector3(localRotation.eulerAngles.x + sineValue * FeltRecoil,localRotation.eulerAngles.y + Mathf.Sin(timer) + (Mathf.PerlinNoise(timer, Random.Range(0, 50)) * (FeltRecoil / 2.5f)),localRotation.eulerAngles.z);
        cameraTransform.localRotation = localRotation;
        
        // gun recoil
        
        Vector3 randomPos = lookatPoint.localPosition;

        randomPos.y += Mathf.Sin(timer) * RecoilAmount;
        randomPos.x += Mathf.Sin(timer) * horizontalRecoil * Random.Range(-0.5f, 0.5f);
        
        lookatPoint.localPosition = randomPos;
        
       // Debug.Log(Mathf.Sin(timer));
        
    }
}
