using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTriggerContoller : MonoBehaviour
{ 
    private bool isCharging;

    private void Start()
    {
        isCharging = transform.parent.GetComponent<Grunt>().isCharging;
    }

    private void Update()
    {
        isCharging = transform.parent.GetComponent<Grunt>().isCharging;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCharging)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player killed by charge");
                other.GetComponent<PlayerCharacterControllerRigidBody>().killPlayer();
            }
        }
    }
}
