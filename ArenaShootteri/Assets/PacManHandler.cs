using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManHandler : MonoBehaviour
{
    public bool empty = true;
    public int count;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.CompareTag("Enemy"))
        {
            Debug.Log(other.transform.root.name);
            count++;
            empty = false;
            if (count == 1)
            {
                GetComponent<Animator>().Play("PacManAvaus");
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Debug.Log(other.transform.root.name);
            count--;
            if (count == 0)
            {
                empty = true;
                GetComponent<Animator>().Play("PacManSulku");
            }
        }
    }



}
