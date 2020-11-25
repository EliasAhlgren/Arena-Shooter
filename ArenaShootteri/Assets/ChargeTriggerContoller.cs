using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Only used for 
/// </summary>
public class ChargeTriggerContoller : MonoBehaviour
{ 
    private bool isCharging;
    private Grunt parent;

    private void Start()
    {
        parent = transform.parent.GetComponent<Grunt>();
        isCharging = parent.isCharging;
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
                GetComponent<BoxCollider>().enabled = false;
                other.GetComponent<PlayerCharacterControllerRigidBody>().killPlayer();

            }
        }
    }
}
