using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScope : MonoBehaviour
{
    public GameObject _camera;
    
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GameObject.Find("AlienScopeCamera");
        _camera.SetActive(true);
    }

    private void OnDestroy()
    {
        _camera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
