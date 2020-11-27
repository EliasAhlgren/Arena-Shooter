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

    private GameObject randomEnemy;
    public GameObject Grunt;
    public GameObject Demon;
    public GameObject Imp;
    public GameObject Vipeltaja;

    private Vector3 spawnPos;
    public static int enemyCount = 0;
    public static int wave = 1;
    public static bool spawnWave = false;
    public bool onCooldown = false;
    private List<GameObject> enemies;

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
                SpawnWave(0, 0, 10, 0, 0);
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
        if (spawnWave == false)
        {
            if (enemyCount == 0)
            {
                GameManager.waveStart = true;
                GameManager.waveEnd = false;
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
        int enemyTypesLeft = 0;

        List<Transform> spawnlista = new List<Transform>(spawns._spawnPoints);

        List<Transform> pacmanlista = new List<Transform>(pacmanSpawns._spawnPoints);
        randomPacman1 = UnityEngine.Random.Range(0, pacmanlista.Count);
        pacman1pos = pacmanlista[randomPacman1].position;
        pacmanlista.RemoveAt(randomPacman1);

        randomPacman2 = UnityEngine.Random.Range(0, pacmanlista.Count);
        pacman2pos = pacmanlista[randomPacman2].position;
        pacmanlista.RemoveAt(randomPacman2);

        randomPacman3 = UnityEngine.Random.Range(0, pacmanlista.Count);
        pacman3pos = pacmanlista[randomPacman3].position;
        pacmanlista.RemoveAt(randomPacman3);

        randomPacman4 = UnityEngine.Random.Range(0, pacmanlista.Count);
        pacman4pos = pacmanlista[randomPacman4].position;
        pacmanlista.RemoveAt(randomPacman4);

        //pacman vihollisten spawnit
        while(impit > 0 && demonit > 0 && vipelt > 0)
        {
            enemyTypesLeft = 0;
            enemies.Clear();
            random = UnityEngine.Random.Range(0, 4);
            if (impit < 0)
            {
                enemyTypesLeft += 1;
                enemies.Add(Imp);
            } 
            if (demonit < 0)
            {
                enemyTypesLeft += 1;
                enemies.Add(Demon);
            }
            if (vipelt < 0)
            {
                enemyTypesLeft += 1;
                enemies.Add(Vipeltaja);
            }
            if (enemyTypesLeft > 1)
            {
                enemyRng = UnityEngine.Random.Range(0, enemyTypesLeft);
            }
            else
            {
                enemyRng = 0;
            }

            if (random == 0)
            {
                PacmanSpawn(enemies[enemyRng], pacman1pos);
            }
            else if (random == 1)
            {
                PacmanSpawn(enemies[enemyRng], pacman2pos);
            }
            else if (random == 2)
            {
                PacmanSpawn(enemies[enemyRng], pacman3pos);
            }
            else if (random == 3)
            {
                PacmanSpawn(enemies[enemyRng], pacman4pos);
            }
        }
        //while (x > 0)
        //{
        //    spawnlista = SpawnEnemy(Grunt, spawnlista);
        //    x--;
        //}
        //while (y > 0)
        //{
        //    spawnlista = SpawnEnemy(Grunt, spawnlista);
        //    y--;
        //}
        //while (z > 0)
        //{
        //    spawnlista = SpawnEnemy(Grunt, spawnlista);
        //    z--;
        //}
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

    void PacmanSpawn(GameObject _enemy, Vector3 spawnPos)
    {
        Instantiate(_enemy, spawnPos, Quaternion.identity);
        enemyCount += 1;
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
