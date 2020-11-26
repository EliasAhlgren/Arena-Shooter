using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> collidingObjects;
    void Start() {
        collidingObjects = new List<GameObject>();
    }
     
    void OnTriggerEnter(Collider collision) {
        if (!collision.gameObject.CompareTag("Level"))
        {
            if (!collidingObjects.Contains(collision.gameObject) && collision.gameObject.GetComponent<IDamage>() != null)
            {
                collidingObjects.Add(collision.gameObject);
            }
        }
        else
        {
            Explode();
        }
        
    }

    public void Explode()
    {
        //
        // kranaatin räjähdys ääni tähän
        //
        foreach (var collidingObject in collidingObjects)
        {
            Debug.Log(collidingObject.gameObject + " Blew up");
            collidingObject.GetComponent<IDamage>().TakeDamage(50f);
        }
    }
    
    void OnTriggerExit(Collider collision) {
        if (collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject);
        }
    }

    IEnumerator KillTimer()
    {
        yield return new  WaitForSeconds(20f);
        Destroy(gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
