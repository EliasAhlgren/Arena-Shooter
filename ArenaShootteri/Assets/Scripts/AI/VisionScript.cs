using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = transform.parent.transform.rotation;
    }

    public bool LineOfSight(float visionLenght)
    {
        RaycastHit hit;
        Vector3 endPoint = transform.position + transform.forward;
        Debug.DrawLine(transform.position, endPoint, Color.red);
        Physics.Raycast(transform.position, endPoint, out hit);
        Debug.Log(hit.ToString());

        if (hit.transform.CompareTag("Player"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    

    


}
