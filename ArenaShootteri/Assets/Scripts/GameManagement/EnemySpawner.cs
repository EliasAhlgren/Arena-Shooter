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

    private spawnPoints pacmanSpawns;
    private int randomPacman1;
    private Vector3 pacman1pos;
    private int randomPacman2;
    private Vector3 pacman2pos;
    private int randomPacman3;
    private Vector3 pacman3pos;
    private int randomPacman4;
    private Vector3 pacman4pos;

    public GameObject[] pacmanList;

    private GameObject randomEnemy;
    public GameObject Grunt;
    public GameObject LentoVipelt;
    public GameObject Imp;
    public GameObject Vipeltaja;
    public GameObject LentoDemon;

    public bool spawning = false;
    public bool enemyWaiting = false;
    private Vector3 spawnPos;
    public static int enemyCount = 0;
    public static int wave = 1;
    public static bool spawnWave = false;
    public bool onCooldown = false;
    public static List<GameObject> enemies;
    public bool spawned = false;

    // Start is called before the first frame update
    void Start()
    { 
        enemies = new List<GameObject>();
        spawns = GameObject.FindGameObjectWithTag("spawnPoints").GetComponent<spawnPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!onCooldown)
        {
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            StartCoroutine(Cooldown());
        }
        
        //Debug.Log("Enemies left: " + enemyCount);
        if (spawnWave == true)
        {
            spawning = true;
            spawnWave = false;
            if (wave == 1)
            {
                Debug.Log("LOL");
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 0, 10, 0, 0);
                
            }
            else if (wave == 2)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 0, 20, 0, 0);
            }
            else if (wave == 3)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 10, 0, 0, 0);
            }
            else if (wave == 4)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 0, 0, 5, 0);
            }
            else if (wave == 5)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 0, 0, 0, 3);
            }
            else if (wave == 6)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(3, 0, 0, 0, 0);
            }
            else if (wave == 7)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 5, 5, 5, 0);
            }
            else if (wave == 8)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(1, 10, 10, 5, 0);
            }
            else if (wave == 9)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 0, 10, 10, 3);
            }
            else if (wave == 10)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(5, 0, 10, 0, 5);
            }
            else if (wave == 11)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 20, 0, 10, 0);
            }
            else if (wave == 12)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 10, 0, 20, 0);
            }
            else if (wave == 13)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 0, 30, 10, 5);
            }
            else if (wave == 14)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(0, 10, 0, 30, 0);
            }
            else if (wave == 15)
            {
                spawning = true;
                spawnWave = false;
                SpawnWave(10, 0, 20, 0, 10);
            }
            else
            {
                int grunts = UnityEngine.Random.Range(0, 6) + (int)Math.Round(wave * 0.15, MidpointRounding.AwayFromZero);
                int lentovilpe = UnityEngine.Random.Range(5, 16) + (int)Math.Round(wave * 0.8, MidpointRounding.AwayFromZero);
                int imps = UnityEngine.Random.Range(5, 21) + (int)Math.Round(wave * 0.8, MidpointRounding.AwayFromZero);
                int vipe = UnityEngine.Random.Range(5, 11) + (int)Math.Round(wave * 0.5, MidpointRounding.AwayFromZero);
                int lentod = UnityEngine.Random.Range(0, 4) + (int)Math.Round(wave * 0.1, MidpointRounding.AwayFromZero);
                spawning = true;
                spawnWave = false;
                SpawnWave(grunts, lentovilpe, imps, vipe, lentod);
            }
        }
        if (spawnWave == false && spawning == false)
        {
            if (enemyCount == 0 && spawned == true)
            {
                spawned = false;
                GameManager.waveEnd = true;

            }
        }
    }

    void SpawnWave(int gruntNumber, int lentoVipeltNumber, int impNumber, int vipeltNumber, int lentoNumber) // määritetään vihollisten määrät ja luodaan niitä oikea määrä
    {
        int gruntit = gruntNumber;
        int lentoVipelt = lentoVipeltNumber;
        int impit = impNumber;
        int vipelt = vipeltNumber;
        int lento = lentoNumber;

        //int random;
        //int enemyRng;
        //int count = pacmanList.Count;

        List<Transform> spawnlista = new List<Transform>(spawns._spawnPoints);

        while (impit > 0 || lentoVipelt > 0 || vipelt > 0 || gruntit > 0)
        {
            if (impit > 0)
            {
                enemies.Add(Imp);
                impit -= 1;
            }
            if (lentoVipelt > 0)
            {
                enemies.Add(LentoVipelt);
                lentoVipelt -= 1;
            }
            if (vipelt > 0)
            {
                enemies.Add(Vipeltaja);
                vipelt -= 1;
            }
            if (gruntit > 0)
            {
                enemies.Add(Grunt);
                gruntit -= 1;
            }
        }
        Debug.Log("Hello spawning enemies. Enemies left: " + enemies.Count);
        SpawnEnemies(enemies);

        for (int x = 0; x < lento; x++)
        {
            spawnlista = SpawnEnemy(LentoDemon, spawnlista);
        }

        // enemies.Clear();
        spawning = false;
        spawned = true;
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

    void PacmanSpawn(GameObject _enemy, Transform spawnPos)
    {
        var pos = spawnPos.Find("Spawn point");
        Instantiate(_enemy, pos.position, pos.rotation);
        enemyCount += 1;
    }

    public void Spawn(GameObject _enemy, Transform spawnPos)
    {
        Instantiate(_enemy, spawnPos.position, spawnPos.rotation);
        enemyCount += 1;
    }

    private IEnumerator Cooldown()
    {
        // Start cooldown
        onCooldown = true;
        // Wait for time you want
        yield return new WaitForSeconds(5.0f);
        // Stop cooldown
        onCooldown = false;
        //Debug.Log("Cooldown Ended");
        //GameManager.waveStart = true;
        //GameManager.waveEnd = false;
    }


    private GameObject FindEmptyPacman()
    {
        foreach (GameObject pacman in pacmanList)
        {
            if (pacman.GetComponent<PacManHandler>().empty)
            {
                return pacman;
            }
        }
        return null;
    }





    private static void Spawning(GameObject enemy, Transform pacman)
    {
        var pos = pacman.Find("Spawn point");
        Instantiate(enemy, pos.position, pos.rotation);
        enemies.Remove(enemy);
        enemyCount += 1;
    }

    private void SpawnEnemies(List<GameObject> enemies)
    {
        Debug.Log("Enemies: " +enemies.Count+" Pacmans: " +pacmanList.Length);
        int i = enemies.Count;
        foreach(var pacman in pacmanList)
        {
            if (enemies.Count > 0)
            {
                Debug.Log(pacman.transform.name);
                Spawning(enemies[0], pacman.transform);
                i++;
            }
        }
    }

    public static void SpawnNext(Transform pacman)
    {
        Spawning(enemies[0], pacman.transform);
    }
}
