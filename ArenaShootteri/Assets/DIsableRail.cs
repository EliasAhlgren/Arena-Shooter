using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIsableRail : MonoBehaviour
{
    [NonSerialized] public GameObject targetRail;
    public string rail;
    
    // Start is called before the first frame update
    void Start()
    {
        targetRail = GameObject.Find(rail);        
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
