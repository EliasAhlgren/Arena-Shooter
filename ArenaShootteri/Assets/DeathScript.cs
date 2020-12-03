using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{

    public GameObject deathPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ConfirmExit()
    {

        DIsableRail[] dIsableRails = FindObjectsOfType<DIsableRail>();

        foreach (var VARIABLE in dIsableRails)
        {
            VARIABLE._shouldEnable = false;
        }

        SceneManager.LoadScene(0);
        deathPanel.SetActive(false);
    }

    public void Retry()
    {

        DIsableRail[] dIsableRails = FindObjectsOfType<DIsableRail>();

        foreach (var VARIABLE in dIsableRails)
        {
            VARIABLE._shouldEnable = false;
        }

        SceneManager.LoadScene(1);
        deathPanel.SetActive(false);
    }
}
