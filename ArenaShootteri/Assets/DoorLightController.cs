using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLightController : MonoBehaviour
{
    public Light ovivalo;
    // Start is called before the first frame update
    void Start()
    {
        ovivalo = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.waveEnd == true)
        {
            ovivalo.enabled = true;
        }
        else
        {
            ovivalo.enabled = false;
        }
    }
}
