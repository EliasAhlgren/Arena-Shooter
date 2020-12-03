using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public float healthRecovered = 25;
    public bool onCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            Debug.Log(other.name);
            if (playerScript.health < playerScript.maxHealth)
            {
                //[SOUND] health pickup sound (pickup sound?) (One Shot)
                if (!onCooldown)
                {
                    SoundManager.PlaySound("HpPickup");
                    StartCoroutine(Cooldown());
                }
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

    private IEnumerator Cooldown()
    {
        // Start cooldown
        onCooldown = true;
        // Wait for time you want
        yield return new WaitForSeconds(5.0f);
        // Stop cooldown
        onCooldown = false;
        //Debug.Log("Cooldown Ended");
        //GameManager.waveStart = true;
        //GameManager.waveEnd = false;
    }
}

