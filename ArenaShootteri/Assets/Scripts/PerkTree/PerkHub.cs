using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkHub : MonoBehaviour
{

    public PerkButton[] perkButtons;

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PerkTreeReader.Instance.availablePoints += 10;
            PlayerPrefs.SetInt("PerkPoints", PerkTreeReader.Instance.availablePoints);
            //PlayerPrefs.SetInt("PerkPoints", PlayerPrefs.GetInt("PerkPoints", 0) + 10);
            RefreshButtons();
        }
    }
    */
    public void RefreshButtons()
    {
        for (int i = 0; i < perkButtons.Length; ++i)
        {
            perkButtons[i].RefreshState();
        }
    }

    public void ResetButtons()
    {
        for (int i = 0; i < perkButtons.Length; ++i)
        {
            perkButtons[i].ResetPerk();
        }
    }
}
