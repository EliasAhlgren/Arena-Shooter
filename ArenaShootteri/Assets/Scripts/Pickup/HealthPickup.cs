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
                //[SOUND] health pickup sound (pickup sound?) (One Shot)


                playerScript.Heal(healthRecovered);
                Debug.Log("healed " + player.name + " for "+ healthRecovered +" hp");

                if (platfromIS)
                {
                    platformSript.PickupRespawn(respawnTime * playerScript.spawnRateModifier);
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }
                
                
                
            }
            else
            {
                //[SOUND] pickup failed sound (One Shot)

                Debug.Log("health full");
            }
        }
    }
}
