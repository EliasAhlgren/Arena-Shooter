using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManHandler : MonoBehaviour
{
    public bool empty = true;
    public int count;

    private void OnTriggerEnter(Collider other)
    {
        count++;
        if(count > 0)
        {
            empty = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        count--;
        if(count == 0)
        {
            empty = true;
        }
    }



}
