using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private spawnPoints spawns;
    private int randomSpot;
    private Vector3 spawnPos;
    public GameObject _enemy;
    public int enemyCount = 0;
    public int waveMax = 3;
    public int wave = 1;
    private int last = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawns = GameObject.FindGameObjectWithTag("spawnPoints").GetComponent<spawnPoints>();
        StartCoroutine(EnemyWave(wave));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemyWave(int waveNumber)  
    {
        while (enemyCount < waveMax)
        {
            int _last = SpawnEnemy(last);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
            last = _last;
        }
    }

    int SpawnEnemy(int lastSpawn) // (EnemyType enemy)
    {
        randomSpot = Random.Range(0, spawns._spawnPoints.Length);
        while (randomSpot == lastSpawn)
        {
            randomSpot = Random.Range(0, spawns._spawnPoints.Length);
        }
        spawnPos = spawns._spawnPoints[randomSpot].position;
        Instantiate(_enemy, spawnPos, Quaternion.identity);
        return randomSpot;
    }
}
