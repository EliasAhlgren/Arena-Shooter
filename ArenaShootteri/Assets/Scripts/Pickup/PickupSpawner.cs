using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
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

    public int healthRate = 50;
    public int rifleRate = 40;
    public int grenadeRate = 1;

    public float spawnTimer = 5;

    private IEnumerator spawner;

    public bool spawningPaused = true;
    bool spawningStarted = false;

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

    public void SpawnPickup()
    {
        int roll = Random.Range(1, 100);
        int a;// = Random.Range(0, pickups.Length);

        if (perkLevel == 5 && roll >= 100 - grenadeRate)
        {
            a = 3;
        }
        else
        {
            if (roll <= (healthRate - 1))
            {
                a = 0;
            }
            else if (roll <= healthRate + rifleRate)
            {
                a = 1;
            }
            else
            {
                a = 2;
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
