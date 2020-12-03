using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotationScript : MonoBehaviour
{
    private float step = 2;
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        transform.Rotate(new Vector3(0, step, 0), Space.World);
    }
}
