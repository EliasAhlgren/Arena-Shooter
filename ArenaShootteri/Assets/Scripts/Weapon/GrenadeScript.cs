using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
