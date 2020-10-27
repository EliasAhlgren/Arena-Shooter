using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeScript : MonoBehaviour
{
    public List<GameObject> objectsInView;
    public List<GameObject> GetColliders() { return objectsInView; }

    public bool CheckColliders(GameObject target)
    {
        if(objectsInView.Contains(target))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnTriggerEnter(Collider col)
    {
        if (!objectsInView.Contains(col.gameObject))
        {
            objectsInView.Add(col.gameObject);
        }
        Debug.Log(col.name + " added to list");
        
    }  

    private void OnTriggerExit(Collider col)
    {
        objectsInView.Remove(col.gameObject);
        Debug.Log(col.name + " removed from list");
    }
}

