using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPlatform : MonoBehaviour
{
    public GameObject pickup;
    Pickup pickupScript;

    public bool spawnAtStart = false;

    bool pickupActive = false;
    float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        pickupScript = pickup.GetComponent<Pickup>();

        if (spawnAtStart)
        {
            pickupActive = true;
            pickup.SetActive(true);
        }
        else
        {
            pickupActive = false;
            pickup.SetActive(false);
            cooldown = pickupScript.respawnTime;
        }
    }

    public void PickupRespawn(float respawnTime)
    {
        pickupActive = false;
        cooldown = respawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pickupActive)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                pickupActive = true;
                pickup.SetActive(true);
            }
        }
    }
}
