using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{

    public PerkHub perkHub;

    private void Start()
    {
        if (!PerkTreeReader.Instance.savePerks)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetPerkButtons()
    {
        perkHub.ResetButtons();
    }

}
