using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPlatform : MonoBehaviour
{
    public GameObject pickup;
    Pickup pickupScript;

    bool pickupActive = true;
    float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        pickupScript = pickup.GetComponent<Pickup>();
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
