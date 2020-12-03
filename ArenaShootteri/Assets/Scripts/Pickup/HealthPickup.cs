﻿using UnityEngine;

public class HealthPickup : Pickup
{
    public float healthRecovered = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            Debug.Log(other.name);
            if (playerScript.health < playerScript.maxHealth)
            {
                //[SOUND] health pickup sound (pickup sound?) (One Shot)


                playerScript.Heal(healthRecovered);
                //Debug.Log("healed " + player.name + " for "+ healthRecovered +" hp");

                DeSpawn();           
                
                
            }
            else
            {
                //[SOUND] pickup failed sound (One Shot)
                DeSpawn();
                //Debug.Log("health full");
            }
        }
    }
}
