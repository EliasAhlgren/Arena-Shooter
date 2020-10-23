﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunAttributes : MonoBehaviour
{
    // Combined stats of all currently attached mods
    public float totalErgonomy;
    public int totalRecoil;
    public float totalDamage;

    public ModSelection sightSelection;
    
    public bool isAiming;

    // animator that handles aiming animation
    private Animator _animator;
    
    public GameObject[] canvases;

    // camera used for rendering when in modding mode
    public GameObject moddingCamera;

    public Vector3 shootyPosition;
    public Vector3 moddingPositio;
    
    public Vector3 shootyRotation;
    public Vector3 moddingRotation;

    private PlayerCharacterController _controller;

    private float cameraStartPos;

    private AnimationClip adsClip;

    public AnimationCurve recoilCurve;
    
    // Start is called before the first frame update
    void Start()
    {

        cameraStartPos = Camera.main.transform.position.y;
        
        _animator = gameObject.GetComponent<Animator>();
        
        _controller = GameObject.FindWithTag("Player").GetComponent<PlayerCharacterController>();
        
        canvases = GameObject.FindGameObjectsWithTag("Canvas");

        StartCoroutine(CheckDefaultStats());
        
        
    }

    // Changes to Modding mode
    void ChangeUI()
    {
        
        _controller.enabled = !_controller.enabled;
        
        foreach (var canvas in canvases)
        {
            canvas.SetActive(!canvas.activeInHierarchy);
        }
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            moddingCamera.SetActive(true);
            transform.parent = null;
            Cursor.lockState = CursorLockMode.None;
            transform.localPosition = moddingPositio;
            transform.localRotation = Quaternion.Euler(moddingRotation);
        }
        else
        {
            moddingCamera.SetActive(false);
            transform.parent = Camera.main.transform;
            Cursor.lockState = CursorLockMode.Locked;
            transform.localPosition = shootyPosition;
            transform.localRotation = Quaternion.Euler(shootyRotation);
        }
        
    }

    public void AimDownSights()
    {
        if (isAiming)
        {
            Vector3 newCamPos = Camera.main.transform.position;
            newCamPos.y = cameraStartPos;
        }
        else 
        {
            if (sightSelection.currentMod)
            {
                Vector3 newCamPos = Camera.main.transform.position;
                newCamPos.y = sightSelection.currentMod.transform.position.y;
            }
        }
        isAiming = !isAiming;
        _animator.SetBool("isAiming", isAiming);
    }
    
    
    // Check stats from default mods
    IEnumerator CheckDefaultStats()
    {
        // wait 0.1 second to account for possible lag or delay when instatiating default mods
        yield return new  WaitForSeconds(0.1f);
        
        foreach (var _stockSelection in GetComponentsInChildren<StockSelection>())
        {
            Debug.Log("initialising with stats");
            _stockSelection.OnModChosen += UpdateStats;
            if (_stockSelection.currenModStats)
            {
                
                totalErgonomy += _stockSelection.currenModStats.Ergonomy;
                totalRecoil += _stockSelection.currenModStats.Recoil;
                totalDamage += _stockSelection.currenModStats.Damage;
            }
            
        }
        
        foreach (var _modSelection in GetComponentsInChildren<ModSelection>())
        {
            _modSelection.OnModChosen += UpdateStats;
            if (_modSelection.currenModStats)
            {
                totalErgonomy += _modSelection.currenModStats.Ergonomy;
                totalRecoil += _modSelection.currenModStats.Recoil;
                totalDamage += _modSelection.currenModStats.Damage;
            }
            
        }
        
        // For some reason the UI wont work if it starts disabled so I have to call ChangeUI to disable the UI and then manually set gun back to the shooting mode
        
        ChangeUI();
        //StartCoroutine("wait");
        
        moddingCamera.SetActive(false);
        transform.parent = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        transform.localPosition = shootyPosition;
        transform.localRotation = Quaternion.Euler(shootyRotation);
        _controller.enabled = !_controller.enabled;
        
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeUI();
    }
    
    // called everytime a mod is changed in any of the rails
    void UpdateStats()
    {

        
        
        
        // sets SightIndex parameter in animator to account for differences in sight heights
        if (sightSelection.currenModStats)
        {
            _animator.SetInteger("SightIndex",sightSelection.currenModStats.animIndex);
        }
        else
        {
            _animator.SetInteger("SightIndex", 0);
        }
        
        Vector3 totalStats;
        totalStats.x = totalErgonomy;
        totalStats.y = totalRecoil;
        totalStats.z = totalDamage;

        totalErgonomy -= Mathf.RoundToInt(totalStats.x);
        totalRecoil -= Mathf.RoundToInt(totalStats.y);
        totalDamage -= Mathf.RoundToInt(totalStats.z);
        
        Debug.Log("Updating Stats");
        
        foreach (var _modSelection in GetComponentsInChildren<ModSelection>())
        {
            if (_modSelection.currenModStats)
            {
                totalErgonomy += _modSelection.currenModStats.Ergonomy;
                totalRecoil += _modSelection.currenModStats.Recoil;
                totalDamage += _modSelection.currenModStats.Damage;
            }
        }
        
        foreach (var _modSelection in GetComponentsInChildren<StockSelection>())
        {
            if (_modSelection.currenModStats)
            {
                totalErgonomy += _modSelection.currenModStats.Ergonomy;
                totalRecoil += _modSelection.currenModStats.Recoil;
                totalDamage += _modSelection.currenModStats.Damage;
            }
        }
        
    }

    public void Recoil()
    {
       
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //Recoil();
        
        if (_animator.IsInTransition(0))
        {
            _animator.speed = 1f + totalErgonomy;
        }
        
        
        
        
        if (Input.GetMouseButtonDown(1) && transform.position != moddingPositio)
        {
            _animator.enabled = true;
            AimDownSights();
        }
        
        // Disables or enables the Mod selection screen
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _animator.enabled = false;
            ChangeUI();
        }
    }
}
