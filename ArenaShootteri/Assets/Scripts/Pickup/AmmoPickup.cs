using UnityEngine;

public enum AmmoType { rifle, shotgun, grenade };

public class AmmoPickup : Pickup
{
    public AmmoType ammoType;

    public int ammoRecovered = 100;

    public GameObject rifleAmmoPickup;
    public GameObject shotgunAmmoPickup;
    public GameObject grenadeAmmoPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            //[SOUND] ammo pickup sound (pickup sound?) (One Shot)

            if (ammoType == AmmoType.rifle)
            {
                gun.totalAmmo += ammoRecovered;
            }
            else if (ammoType == AmmoType.shotgun)
            {
                if (FindObjectOfType<ShotgunScript>())
                {
                    FindObjectOfType<ShotgunScript>().shellsLeft += ammoRecovered;
                }
                
                //gun.totalAmmo += ammoRecovered;
            }
            else if (ammoType == AmmoType.grenade)
            {
                if (FindObjectOfType<GrenadeLauncher>())
                {
                    FindObjectOfType<GrenadeLauncher>().grenadesLeft += ammoRecovered;
                }
                
                //gun.totalAmmo += ammoRecovered;
            }

            DeSpawn();
        }
    }

    public void ChageAmmoType(AmmoType ammoType)
    {
        if (ammoType == AmmoType.grenade)
        {
            Instantiate(grenadeAmmoPickup, position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (ammoType == AmmoType.shotgun)
        {
            Instantiate(shotgunAmmoPickup, position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(rifleAmmoPickup, position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
