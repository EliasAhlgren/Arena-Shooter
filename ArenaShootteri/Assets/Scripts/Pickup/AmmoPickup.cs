using UnityEngine;


public class AmmoPickup : Pickup
{
    public enum AmmoType {rifle, shotgun, grenade};

    public AmmoType ammoType;

    public int ammoRecovered = 100;

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
                //gun.totalAmmo += ammoRecovered;
            }
            else if (ammoType == AmmoType.grenade)
            {
                //gun.totalAmmo += ammoRecovered;
            }

            DeSpawn();
        }
    }
}
