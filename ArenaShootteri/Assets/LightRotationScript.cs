using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotationScript : MonoBehaviour
{
    public float step = 2;
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        transform.Rotate(new Vector3(step, 0,0), Space.World);
    }
}
