using System;
using System.Collections;
//using UnityEditor.Callbacks;
using UnityEngine;


public class GunAttributes : MonoBehaviour
{
    // Combined stats of all currently attached mods
    public float totalErgonomy;
    public float totalRecoil;
    public float totalDamage;

    public int ammoInMag;
    public int totalAmmo;

    public float rateOfFire = 0.041f;
    
    public Recoil recoilScript;
    
    [NonSerialized] public GameObject[] canvases;

    // camera used for rendering when in modding mode
    public GameObject moddingCamera;

    public Vector3 shootyPosition;
    public Vector3 moddingPositio;
    
    public Vector3 shootyRotation;
    public Vector3 moddingRotation;

    private PlayerCharacterControllerRigidBody _controller;

    private float cameraStartPos;

    private AnimationClip adsClip;

    public WeaponRotationScript _WeaponRotationScript;

    public bool isModding;

    public GameObject mainCamera;

    public GameObject wallHackCamera;
    
    // Start is called before the first frame update
    void Start()
    {

        
        cameraStartPos = mainCamera.transform.position.y;
        
        
        _controller = GameObject.FindWithTag("Player").GetComponent<PlayerCharacterControllerRigidBody>();
        
        canvases = GameObject.FindGameObjectsWithTag("Canvas");

        StartCoroutine(CheckDefaultStats());
        
        
    }

    
    
    // Changes to Modding mode
    public void ChangeUI()
    {
        
        //_controller.enabled = !_controller.enabled;
        _WeaponRotationScript.enabled = !_WeaponRotationScript.enabled;
        
        foreach (var canvas in canvases)
        {
            canvas.SetActive(!canvas.activeInHierarchy);
        }
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            //This is for enabling modding
            isModding = true;
            moddingCamera.SetActive(true);
            transform.parent = null;
            Cursor.lockState = CursorLockMode.None;
            transform.position = moddingPositio;
            transform.rotation = Quaternion.Euler(moddingRotation);
            //Time.timeScale = 0.3f;
        }
        else
        {
            isModding = false;
            //this is for disabling modding
            moddingCamera.SetActive(false);
            transform.parent = null;
            //transform.parent = Camera.main.transform;
            Cursor.lockState = CursorLockMode.Locked;
            //transform.localPosition = shootyPosition;
            //transform.localRotation = Quaternion.Euler(shootyRotation);
            //Time.timeScale = 1f;
            
        }
        
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
        isModding = false;
        //transform.parent = Camera.main.transform;
        transform.parent = null;
        Cursor.lockState = CursorLockMode.Locked;
        /*transform.localPosition = shootyPosition;
        transform.localRotation = Quaternion.Euler(shootyRotation);*/
        //_controller.enabled = !_controller.enabled;
        _WeaponRotationScript.enabled = !_WeaponRotationScript.enabled;
        //Time.timeScale = 1f;
        transform.localPosition = shootyPosition;
        recoilScript.enabled = true;

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeUI();
    }
    
    // called everytime a mod is changed in any of the rails
    void UpdateStats()
    {
        
        
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
                
                if (_modSelection.currenModStats.rateOfFire > 0.01f)
                {
                    rateOfFire = _modSelection.currenModStats.rateOfFire;
                }
                else
                {
                    rateOfFire = 0.041f;
                }
            }
            
        }
        
        recoilScript.UpdateStats();
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.timeScale);
        
        //Recoil();

       
        
        // Disables or enables the Mod selection screen
        
    }
}
