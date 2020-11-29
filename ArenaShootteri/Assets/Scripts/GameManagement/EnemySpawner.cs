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

    public List<GameObject> pacmanList;

    private GameObject randomEnemy;
    public GameObject Grunt;
    public GameObject Demon;
    public GameObject Imp;
    public GameObject Vipeltaja;

    public bool spawning = false;
    private Vector3 spawnPos;
    public static int enemyCount = 0;
    public static int wave = 1;
    public static bool spawnWave = false;
    public bool onCooldown = false;
    private List<GameObject> enemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
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
            if (wave == 1)
            {
                spawning = true;
                SpawnWave(0, 0, 0, 10, 0);
                spawnWave = false;
            }
            else if (wave == 2)
            {
                spawnWave = false;
                SpawnWave(0, 0, 30, 0, 0);
            }
            else if (wave == 3)
            {
                spawnWave = false;
                SpawnWave(0, 5, 0, 0, 0);
            }
            else if (wave == 4)
            {
                spawnWave = false;
                SpawnWave(0, 0, 0, 0, 4);
            }
            else if (wave == 5)
            {
                spawnWave = false;
                SpawnWave(0, 0, 0, 5, 0);
            }
            else if (wave == 6)
            {
                spawnWave = false;
                SpawnWave(3, 0, 0, 0, 0);
            }
            else
            {
                int grunts = 1 + (int)Math.Round(wave * 0.5, MidpointRounding.AwayFromZero);
                int demons = 2 + (int)Math.Round(wave * 1.5, MidpointRounding.AwayFromZero);
                int imps = 5 + wave * 3;
                spawnWave = false;
                SpawnWave(grunts, demons, imps, 0, 0);
            }
        }
        if (spawnWave == false && spawning == false)
        {
            if (enemyCount == 0)
            {
                GameManager.waveEnd = true;
            }
        }
    }

    void SpawnWave(int gruntNumber, int demonNumber, int impNumber, int vipeltNumber, int lentoNumber) // määritetään vihollisten määrät ja luodaan niitä oikea määrä
    {
        int gruntit = gruntNumber;
        int demonit = demonNumber;
        int impit = impNumber;
        int vipelt = vipeltNumber;
        int lento = lentoNumber;

        int random;
        int enemyRng;
        int count = pacmanList.Count;

        List<Transform> spawnlista = new List<Transform>(spawns._spawnPoints);
        while (impit > 0 || demonit > 0 || vipelt > 0)
        {
            if (impit > 0)
            {
                enemies.Add(Imp);
                impit -= 1;
            }
            if (demonit > 0)
            {
                enemies.Add(Demon);
                demonit -= 1;
            }
            if (vipelt > 0)
            {
                enemies.Add(Vipeltaja);
                vipelt -= 1;
            }
        }

        for (int x = 0; x < enemies.Count; x++)
        {
            random = UnityEngine.Random.Range(0, count);
            enemyRng = UnityEngine.Random.Range(0, enemies.Count);

            bool isEmpty = pacmanList[random].GetComponent<PacManHandler>().empty;
            Debug.Log(isEmpty);
            while (!isEmpty)
            {
                random = UnityEngine.Random.Range(0, count);
                isEmpty = pacmanList[random].GetComponent<PacManHandler>().empty;
            }
            PacmanSpawn(enemies[enemyRng], pacmanList[random].transform);
            enemies.RemoveAt(enemyRng); 
        }

        enemies.Clear();
        spawning = false;
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

    public static void Spawn(GameObject _enemy, Transform spawnPos)
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

}
