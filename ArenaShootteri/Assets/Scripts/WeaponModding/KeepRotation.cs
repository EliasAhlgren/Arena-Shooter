using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour
{
    
    //tää on sitä varten että UI elementit osottaa aina kameraa
    
    private Transform cameraTransform;
    // Start is called before the first frame update
    private void Awake()
    {
        cameraTransform = gameObject.GetComponent<Canvas>().worldCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform);
    }
}
