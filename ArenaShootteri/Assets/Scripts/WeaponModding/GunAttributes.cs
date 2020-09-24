using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunAttributes : MonoBehaviour
{
    public int totalErgonomy;
    public int totalRecoil;
    public float totalDamage;

    private GameObject[] canvases;

    public GameObject moddingCamera;

    public Vector3 shootyPosition;
    public Vector3 moddingPositio;
    
    public Vector3 shootyRotation;
    public Vector3 moddingRotation;

    private PlayerCharacterController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GameObject.FindWithTag("Player").GetComponent<PlayerCharacterController>();
        
        canvases = GameObject.FindGameObjectsWithTag("Canvas");

        StartCoroutine(CheckDefaultStats());
        
        
    }

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
        moddingCamera.SetActive(false);
        transform.parent = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        transform.localPosition = shootyPosition;
        transform.localRotation = Quaternion.Euler(shootyRotation);
        _controller.enabled = !_controller.enabled;
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
    
    // Update is called once per frame
    void Update()
    {
        // Disables or enables the Mod selection screen
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeUI();
        }
    }
}
