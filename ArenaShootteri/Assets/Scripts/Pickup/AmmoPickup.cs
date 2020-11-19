using UnityEngine;


public class AmmoPickup : Pickup
{
    public float ammoRecovered = 100;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            Debug.Log(other.name);
            platformSript.PickupRespawn(respawnTime * playerScript.spawnRateModifier);
            gameObject.SetActive(false);
        }
    }
}
