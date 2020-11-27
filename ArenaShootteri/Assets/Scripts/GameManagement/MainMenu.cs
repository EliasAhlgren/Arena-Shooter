using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject exitPanel;
    public GameObject controlsPanel;
    public GameObject highscorePanel;
    public GameObject timeline;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        Time.timeScale = 1;
        timeline.SetActive(true);
    }

    public void Play()
    {
        timeline.SetActive(false);
        SceneManager.LoadScene(1);
    }
    //Settings buttons
    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    //Credits buttons
    public void OpenCredits()
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
    //Controls buttons
    public void OpenControls()
    {
        mainPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void CloseControls()
    {
        mainPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }
    //Highscore buttons
    public void OpenHighScore()
    {
        mainPanel.SetActive(false);
        highscorePanel.SetActive(true);
    }
    public void CloseHighScore()
    {
        mainPanel.SetActive(true);
        highscorePanel.SetActive(false);
    }
    //QuitGame buttons
    public void OpenExit()
    {
        mainPanel.SetActive(false);
        exitPanel.SetActive(true);
    }
    public void CloseExit()
    {
        mainPanel.SetActive(true);
        exitPanel.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void PlayClick()
    {
        SoundManager.PlaySound("MenuClick");
    }
}
