using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScope : MonoBehaviour
{
    public GameObject _camera;

    public bool shouldDisable = true;
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GameObject.Find("GUN2 1").GetComponent<GunAttributes>().wallHackCamera;
        _camera.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        shouldDisable = false;
    }

    private void OnDestroy()
    {
        if (shouldDisable) return;
        _camera.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
