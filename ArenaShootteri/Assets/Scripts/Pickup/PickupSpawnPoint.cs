using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AddSpawnPoint();
    }

    public void AddSpawnPoint()
    {
        PickupSpawner.Instance.pickupSpawnPoints.Add(gameObject);

        if (PickupSpawner.Instance.spawningPaused)
        {
            PickupSpawner.Instance.ReStartSpawner();
        }
        
    }
}
