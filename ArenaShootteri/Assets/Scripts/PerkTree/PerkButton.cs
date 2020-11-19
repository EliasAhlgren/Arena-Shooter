using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkButton : MonoBehaviour
{

    public int perkId;

    public Color unlockedColor;

    public PerkHub perkHub;

    private Image _image;
    private Button _button;
    private Text _text;

    void Start()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<Text>();

        RefreshState();
    }

    public void RefreshState()
    {
        PerkType perkTypeCheck = PerkTreeReader.Instance.IsPerkType(perkId);

        if (perkTypeCheck == PerkType.leveleable)
        {
            int perkLevel = PerkTreeReader.Instance.IsPerkLevel(perkId);

            _text.text = PerkTreeReader.Instance.IsPerkLevel(perkId).ToString();
        }
        else if (perkTypeCheck == PerkType.active)
        {
            _text.text = "active";
        }
        else
        {
            _text.text = "passive";
        }
        
        if (PerkTreeReader.Instance.IsPerkUnlocked(perkId))
        {
            _image.color = unlockedColor;
        }
        else if (!PerkTreeReader.Instance.CanPerkBeUnlocked(perkId))
        {
            _image.color = Color.gray;
            _button.interactable = false;
        }
        else
        {
            _image.color = Color.white;
            _button.interactable = true;
        }
    }

    public void BuyPerk()
    {
        if (PerkTreeReader.Instance.UnlockPerk(perkId))
        {
            PlayerPrefs.SetInt("PerkPoints", PerkTreeReader.Instance.availablePoints);
            perkHub.RefreshButtons();
            PerkTreeReader.Instance.SavePerkTree();
        }
    }

    public void ResetPerk()
    {
        if (PerkTreeReader.Instance.ResetPerk(perkId))
        {
            PlayerPrefs.SetInt("PerkPoints", PerkTreeReader.Instance.availablePoints);
            perkHub.RefreshButtons();
            PerkTreeReader.Instance.SavePerkTree();
        }

    }
}