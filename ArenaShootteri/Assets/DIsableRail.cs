using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIsableRail : MonoBehaviour
{
    public GameObject targetRail;
    
    // Start is called before the first frame update
    void Start()
    {
        targetRail = GameObject.Find("D_ModRail");        
        targetRail.SetActive(false);
    }

    private void OnDestroy()
    {
        targetRail.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
