using UnityEngine;

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
                playerScript.TakeDamage(healthRecovered, false);
                Debug.Log("healed " + player.name + " for "+ healthRecovered +" hp");
                platformSript.PickupRespawn(respawnTime * playerScript.spawnRateModifier);
                gameObject.SetActive(false);
                
                //Destroy(gameObject);
            }
        }
    }
}
