using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public enum SpawnedPickups {none, health, rifle, shotgun, grenade };

    //public enum AmmoType {rifle, shotgun, grenade};
    private static PickupSpawner _instance;
    public static PickupSpawner Instance
    {
        get
        {
            return _instance;
        }
        set
        {
        }
    }

    public List<GameObject> pickupSpawnPoints;

    public GameObject[] pickups;

    private bool useShotgun;
    private bool useGrenade;

    public int healthRate = 50;

    public int shogunRate = 10;
    public int grenadeRate = 10;

    public float spawnTimer = 5;

    private IEnumerator spawner;

    public bool spawningPaused = true;
    public bool spawningStarted = false;

    public int perkLevel = 0;

    float perkEffectMod;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        perkLevel = PerkTreeReader.Instance.IsPerkLevel(18);
        perkEffectMod = 1 - 0.05f * perkLevel;

        spawner = Spawner(spawnTimer * perkEffectMod);
        StartCoroutine(spawner);
        spawningPaused = false;
        spawningStarted = true;
    }

    public void ReStartSpawner()
    {
        spawningPaused = false;
        StartCoroutine(spawner);
    }

    public void UpdatePickupSpawns(bool shotgun, bool grenade)
    {
        if (useShotgun && !shotgun)
        {
            UpdateSpawnedPickups(AmmoType.shotgun, shotgun, grenade);
            
        }
        useShotgun = shotgun;

        if (useGrenade && !grenade)
        {
            UpdateSpawnedPickups(AmmoType.grenade, shotgun, grenade);
        }
        useGrenade = grenade;
    }

    private void UpdateSpawnedPickups(AmmoType ammoType, bool shotgun, bool grenade)
    {
        AmmoPickup[] ammoPickups = FindObjectsOfType<AmmoPickup>();

        foreach (AmmoPickup ammoPickup in ammoPickups)
        {
            if (ammoPickup.ammoType == ammoType)
            {
                if (ammoType == AmmoType.shotgun)
                {
                    if (grenade)
                    {
                        ammoPickup.ChageAmmoType(AmmoType.grenade);
                    }
                    else
                    {
                        ammoPickup.ChageAmmoType(AmmoType.rifle);
                    }
                }
                else
                {
                    if (shotgun)
                    {
                        ammoPickup.ChageAmmoType(AmmoType.shotgun);
                    }
                    else
                    {
                        ammoPickup.ChageAmmoType(AmmoType.rifle);
                    }
                }
            }
        }
    }

    public void UpdatePerkLevel(int level)
    {
        perkLevel = level;

        if (spawningStarted)
        {
            if (!spawningPaused)
            {
                StopCoroutine(spawner);

                
                StartCoroutine(spawner);
            }
            perkEffectMod = 1 - 0.05f * perkLevel;
            spawner = Spawner(spawnTimer * perkEffectMod);

        }
    }

    private void SpawnPickup()
    {
        int roll = Random.Range(1, 100);
        int a;// = Random.Range(0, pickups.Length);

        if (roll <= (healthRate - 1))
        {
            a = 0;
        }
        else if (!useShotgun && !useGrenade)
        {
            a = 1;
        }
        else
        {
            if (useShotgun)
            {
                if (roll < 99 - shogunRate)
                {
                    a = 1;
                }
                else
                {
                    a = 2;
                }
            }
            else
            {
                if (roll < 99 - grenadeRate)
                {
                    a = 1;
                }
                else
                {
                    a = 3;
                }
            }
        }


        int b = Random.Range(0, pickupSpawnPoints.Count);

        var pickup = Instantiate(pickups[a], pickupSpawnPoints[b].transform.position, Quaternion.identity);
        pickup.transform.parent = pickupSpawnPoints[b].transform;

        pickupSpawnPoints.RemoveAt(b);
    }

    private IEnumerator Spawner(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            //Debug.Log(pickups.Length + "" + pickupSpawnPoints.Count);

            if (pickups.Length > 0 && pickupSpawnPoints.Count > 0)
            {
                if (perkLevel > 0 && pickupSpawnPoints.Count > 1)
                {
                    SpawnPickup();
                }

                SpawnPickup();
            }
            else
            {
                if (pickups.Length < 1)
                {
                    Debug.Log("Can't spawn: nothing to spawn");
                }
                else
                {
                    Debug.Log("Can't spawn: no awailable spawn points");

                    spawningPaused = true;
                    StopCoroutine(spawner);
                }

                
                
            }
            
        }
    }
}
