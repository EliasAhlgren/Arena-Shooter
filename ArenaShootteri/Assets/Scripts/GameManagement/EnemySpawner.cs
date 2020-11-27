using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private spawnPoints spawns;
    private int randomSpot;
    private Vector3 spawnPos;
    public static int enemyCount = 0;
    public static int wave = 1;
    public GameObject Grunt;
    public static bool spawnWave = false;
    public bool onCooldown = false;
    private GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        spawns = GameObject.FindGameObjectWithTag("spawnPoints").GetComponent<spawnPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        //Debug.Log("Enemies left: " + enemyCount);
        if (spawnWave == true)
        {
            if (wave == 1)
            {
                spawnWave = false;
                SpawnWave(3, 0, 0);
            }
            else
            {
                int grunts = 1 + (int)Math.Round(wave * 0.5, MidpointRounding.AwayFromZero);
                int demons = 0; //2 + (int)Math.Round(wave * 1.5, MidpointRounding.AwayFromZero);
                int imps = 0; //5 + wave * 3;
                spawnWave = false;
                SpawnWave(grunts, demons, imps);
            }
        }
        if (spawnWave == false)
        {
            if (enemyCount == 0)
            {
                GameManager.waveStart = true;
                GameManager.waveEnd = false;
            }
        }
    }

    void SpawnWave(int gruntNumber, int demonNumber, int impNumber) // määritetään vihollisten määrät ja luodaan niitä oikea määrä
    {
        int x = gruntNumber;
        int y = demonNumber;
        int z = impNumber;
        List<Transform> spawnlista = new List<Transform>(spawns._spawnPoints);
        while (x > 0)
        {
            spawnlista = SpawnEnemy(Grunt, spawnlista);
            x--;
        }
        while (y > 0)
        {
            spawnlista = SpawnEnemy(Grunt, spawnlista);
            y--;
        }
        while (z > 0)
        {
            spawnlista = SpawnEnemy(Grunt, spawnlista);
            z--;
        }
    }

    List<Transform> SpawnEnemy(GameObject _enemy, List<Transform> spawnit) // satunnainen spawn kohta, spawnataan, nostetaan vihollismäärää ja poistetaan spawn listasta
    {
        randomSpot = UnityEngine.Random.Range(0, spawnit.Count);
        spawnPos = spawnit[randomSpot].position;
        Instantiate(_enemy, spawnPos, Quaternion.identity);
        enemyCount += 1;
        spawnit.RemoveAt(randomSpot);
        return spawnit;
    }

    private IEnumerator Cooldown()
    {
        // Start cooldown
        onCooldown = true;
        // Wait for time you want
        yield return new WaitForSeconds(1.0f);
        // Stop cooldown
        onCooldown = false;
        //Debug.Log("Cooldown Ended");
        //GameManager.waveStart = true;
        //GameManager.waveEnd = false;
    }

}
