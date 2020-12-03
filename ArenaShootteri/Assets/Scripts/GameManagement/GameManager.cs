using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject exitMenu;
    public GameObject pauseCanvas;
    public bool acceptPlayerInput = true;
    public bool paused = false;
    public bool playerAlive = true;
    public int wave = 0;
    public int level = 1;
    public int shadowOrbs; //mahdollisen perk systeemin pointsit

    public BoxCollider checkBox;

    public static bool waveEnd = false;
    public static bool waveStart = true;

    private WaveIndicator waveIndicator;

    // Start is called before the first frame update
    void Start()
    {
        waveIndicator = FindObjectOfType<WaveIndicator>();
        player = GameObject.FindGameObjectWithTag("Player");
        waveEnd = false;
        waveStart = true;
        //waveManager = UnityEngine.GameObje<ct.FindGameObjectWithTag("GameManagement").GetComponent<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // PAUSE MENU //
        if (acceptPlayerInput || !playerAlive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!paused)
                {
                    paused = true;
                    pauseCanvas.SetActive(true);
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    paused = false;
                    pauseCanvas.SetActive(false);
                    pauseMenu.SetActive(false);
                    optionsMenu.SetActive(false);
                    exitMenu.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }

        if (waveStart == true && waveEnd == false) // aloittaa waven, jos waveStart boolean vaihdetaan arvoon "true"
        {
            wave += 1;

            checkBox.enabled = false;
            waveIndicator.UpdateWaveIndicator(wave);
            Debug.Log("Käynnistä uusi wave");
            EnemySpawner.spawnWave = true; // käynnistää EnemySpawner scriptin
            EnemySpawner.wave = wave;
            // Debug.Log("wave: " + wave);
            waveStart = false;
        }

        if (waveEnd == true)
        {
            waveIndicator.WaveEnd();
            checkBox.enabled = true;
        }
    }

    public static void StartWave()
    {
        waveEnd = false;
        waveStart = true;
        
    }


}
