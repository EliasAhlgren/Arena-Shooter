using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIsableRail : MonoBehaviour
{
    [NonSerialized] public GameObject targetRail;
    public string rail;

    public bool _shouldEnable = true;
    
    // Start is called before the first frame update
    void Start()
    {
        targetRail = GameObject.Find(rail);        
        targetRail.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        _shouldEnable = false;
    }

    
    
    private void OnDestroy()
    {
        if (!_shouldEnable) return;
        targetRail.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
