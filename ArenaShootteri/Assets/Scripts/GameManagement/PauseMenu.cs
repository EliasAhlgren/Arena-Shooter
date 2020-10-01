using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject exitPanel;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = UnityEngine.GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManager>();
    }

    public void Continue()
    {
        gameManager.paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    public void OpenOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ExitGame()
    {
        pausePanel.SetActive(false);
        exitPanel.SetActive(true);
    }

    public void CancelExit()
    {
        exitPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ConfirmExit()
    {
        gameManager.paused = false;
        // change scene to main menu
        exitPanel.SetActive(false);
    }
}
