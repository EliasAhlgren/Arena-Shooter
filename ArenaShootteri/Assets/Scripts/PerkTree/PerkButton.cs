using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkButton : MonoBehaviour
{

    public int perkId;

    public Color availableColor;
    public Color lockedColor;
    public Color leveleableColor;
    public Color unlockedColor;

    public PerkHub perkHub;

    private Image _image;
    private Button _button;
    //private Text _text;
    public Text perkLevelText;
    public Text perkCostText;
    public Text perkStateText;

    public GameObject tooltip;
    Text tooltipText;
    Vector3 offset = new Vector3(170,-25,0);

    public bool isMouseHover = false;
    bool isUnlocked;


    void Start()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        tooltipText = tooltip.GetComponent<Text>();
        //_text = GetComponentInChildren<Text>();

        RefreshState();
    }

    private void Update()
    {
        if (isMouseHover)
        {
            OnMouseStay();
        }
    }

    public void RefreshState()
    {
        PerkType perkTypeCheck = PerkTreeReader.Instance.IsPerkType(perkId);

        // check perk type

        //leveleable perk
        if (perkTypeCheck == PerkType.leveleable)
        {
            int perkLevel = PerkTreeReader.Instance.IsPerkLevel(perkId);

            //_text.text = PerkTreeReader.Instance.IsPerkLevel(perkId).ToString();
            perkLevelText.text = PerkTreeReader.Instance.IsPerkLevel(perkId).ToString();
        }
        // active
        else if (perkTypeCheck == PerkType.active)
        {
            perkLevelText.text = "";
            //_text.text = "active";
        }
        // passive
        else
        {
            perkLevelText.text = "";
            //_text.text = "passive";
        }
        
        // check perk state

        // perk bought
        if (PerkTreeReader.Instance.IsPerkUnlocked(perkId))
        {
            if (perkTypeCheck == PerkType.leveleable && PerkTreeReader.Instance.IsPerkLevel(perkId) < 5)
            {
                if (!PerkTreeReader.Instance.CanPerkBeUnlockedCost(perkId))
                {
                    perkCostText.color = Color.red;
                    _button.interactable = false;
                }
                else
                {
                    perkCostText.color = Color.green;
                    _button.interactable = true;
                }
                perkCostText.text = PerkTreeReader.Instance.IsPerkCost(perkId).ToString();
                perkStateText.text = "LEVELEABLE";
                _image.color = leveleableColor;
            }
            else
            {
                perkStateText.text = "UNLOCKED";
                perkCostText.text = "";


                _image.color = unlockedColor;
            }
            
        }
        // perk locked
        else if (!PerkTreeReader.Instance.CanPerkBeUnlocked(perkId))
        {
            if (perkTypeCheck == PerkType.active && !PerkTreeReader.Instance.CanActivePerkBeUnlocked(perkId))
            {
                perkStateText.text = "LOCKED ACTIVE";
                perkCostText.text = "";
                //perkCostText.color = Color.red;
                //_image.color = Color.gray;
                _image.color = lockedColor;
                _button.interactable = false;
            }
            else if (!PerkTreeReader.Instance.CanPerkBeUnlockedDependencies(perkId))
            {
                perkStateText.text = "LOCKED DEPENDENCIES";
                perkCostText.text = PerkTreeReader.Instance.IsPerkCost(perkId).ToString();
                perkCostText.color = Color.gray;
                //_image.color = Color.gray;
                _image.color = lockedColor;
                _button.interactable = false;
            }
            else
            {
                perkStateText.text = "LOCKED";
                perkCostText.text = PerkTreeReader.Instance.IsPerkCost(perkId).ToString();
                perkCostText.color = Color.red;
                //_image.color = Color.gray;
                _image.color = lockedColor;
                _button.interactable = false;
            }
            
        }
        // perk available
        else
        {
            perkStateText.text = "AVAILABLE";
            perkCostText.text = PerkTreeReader.Instance.IsPerkCost(perkId).ToString();
            perkCostText.color = Color.green;
            //_image.color = Color.white;
            _image.color = availableColor;
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

    public void OnMouseEnter()
    {
        isMouseHover = true;
        tooltip.gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        isMouseHover = false;
        tooltip.gameObject.SetActive(false);
    }

    public void OnMouseStay()
    {
        tooltip.transform.position = Input.mousePosition + offset;
    }
}