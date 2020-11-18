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
