using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// Tää scripti on aseen kääntämistä varten
public class WeaponRotationScript : MonoBehaviour
{
    public float rotationSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(0,Input.GetAxis("Mouse X")* rotationSpeed,0);
        }
    }
}
