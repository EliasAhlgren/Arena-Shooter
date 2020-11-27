using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// Tää scripti on aseen kääntämistä varten
public class WeaponRotationScript : MonoBehaviour
{
    public float rotationSpeed;
    public float mouseX;
    public float minValue, maxValue;

    public float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(2))
        {
            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, minValue, maxValue);
            transform.localRotation = Quaternion.Euler(0,yRotation,0);
        }
    }
}
