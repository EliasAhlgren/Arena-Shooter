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
    public int wave = 1;
    public int level = 1;
    public int shadowOrbs; //mahdollisen perk systeemin pointsit 
    //public WaveManager waveManager; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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


    }
}
