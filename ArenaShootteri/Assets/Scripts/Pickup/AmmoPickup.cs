using UnityEngine;


public class AmmoPickup : Pickup
{
    public enum AmmoType {rifle, shotgun};

    public AmmoType ammoType;

    public int ammoRecovered = 100;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            //[SOUND] ammo pickup sound (pickup sound?) (One Shot)

            if (ammoType == AmmoType.rifle)
            {
                GameObject gun = GameObject.Find("GUN2 1");
                gun.GetComponent<GunAttributes>().totalAmmo += ammoRecovered;
            }
            else if (ammoType == AmmoType.shotgun)
            {
                GameObject gun = GameObject.Find("GUN2 1");
                gun.GetComponent<GunAttributes>().totalAmmo += ammoRecovered;
            }
            

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
    }
}
