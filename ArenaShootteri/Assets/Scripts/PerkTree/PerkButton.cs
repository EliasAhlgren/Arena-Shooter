using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkButton : MonoBehaviour
{

    public int perkId;

    public Sprite[] perkLevelSprites;
    public GameObject PerkCurrency;
    private Image perkCurrencyImage;
    public Image perkLevelImage;


    public Color availableColor;
    public Color lockedColor;
    public Color leveleableColor;
    public Color unlockedColor;

    public Color activePerkLocked;
    public Color activePerkAvailable;
    public Color activePerkUnlocked;

    public PerkHub perkHub;

    private Image _image;
    private Button _button;
    //private Text _text;
    public Text perkLevelText;
    public Text perkCostText;
    public Text perkStateText;

    public GameObject activePerk;
    private Image activePerkImage;

    public GameObject tooltip;
    public Text nameText;
    public Text explanationText;

    public string perkNameText;
    public string perkExplanationText;

    Text tooltipText;
    Vector3 offset = new Vector3(170,-25,0);

    public bool isMouseHover = false;
    bool isUnlocked;


    void Start()
    {
        perkCurrencyImage = PerkCurrency.GetComponent<Image>();
        activePerkImage = activePerk.GetComponent<Image>();
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
            //perkLevelImage.sprite = perkLevelSprites[PerkTreeReader.Instance.IsPerkLevel(perkId)];
            perkLevelImage.sprite = perkLevelSprites[perkLevel];
        }
        // active
        else if (perkTypeCheck == PerkType.active)
        {
            activePerk.SetActive(true);
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
                    perkCurrencyImage.color = Color.gray;
                    perkCostText.color = Color.gray;
                    _button.interactable = false;
                }
                else
                {
                    perkCurrencyImage.color = Color.white;
                    perkCostText.color = Color.white;
                    _button.interactable = true;
                }
                //PerkCurrency.SetActive(true);
                perkCostText.text = PerkTreeReader.Instance.IsPerkCost(perkId).ToString();
                perkStateText.text = "LEVELEABLE";

                //activePerkImage.color = lockedColor;
                perkCurrencyImage.color = Color.white;
                perkCostText.color = Color.white;
                perkLevelImage.color = leveleableColor;
                _image.color = leveleableColor;
            }
            else
            {
                //PerkCurrency.SetActive(false);
                perkStateText.text = "UNLOCKED";
                perkCostText.text = "-";

                activePerkImage.color = activePerkUnlocked;

                perkCurrencyImage.color = Color.white;
                perkCostText.color = Color.white;

                perkLevelImage.color = unlockedColor;
                _image.color = unlockedColor;
            }
            
        }
        // perk locked
        else if (!PerkTreeReader.Instance.CanPerkBeUnlocked(perkId))
        {
            if (perkTypeCheck == PerkType.active && !PerkTreeReader.Instance.CanActivePerkBeUnlocked(perkId))
            {
                PerkCurrency.SetActive(false);
                perkStateText.text = "LOCKED ACTIVE";
                perkCostText.text = "";
                //perkCostText.color = Color.red;
                //_image.color = Color.gray;

            }
            else if (!PerkTreeReader.Instance.CanPerkBeUnlockedDependencies(perkId))
            {
                //PerkCurrency.SetActive(true);
                perkStateText.text = "LOCKED DEPENDENCIES";
                perkCostText.text = PerkTreeReader.Instance.IsPerkCost(perkId).ToString();
                //perkCostText.color = Color.gray;
                //_image.color = Color.gray;

            }
            else
            {

                //PerkCurrency.SetActive(true);
                perkStateText.text = "LOCKED";
                perkCostText.text = PerkTreeReader.Instance.IsPerkCost(perkId).ToString();
                //perkCostText.color = Color.gray;
                //_image.color = Color.gray;
                
            }

            perkCurrencyImage.color = Color.gray;
            
            perkCostText.color = Color.gray;

            activePerkImage.color = activePerkLocked;
            perkLevelImage.color = lockedColor;
            _image.color = lockedColor;
            _button.interactable = false;

        }
        // perk available
        else
        {
            PerkCurrency.SetActive(true);
            perkStateText.text = "AVAILABLE";
            perkCostText.text = PerkTreeReader.Instance.IsPerkCost(perkId).ToString();

            activePerkImage.color = activePerkAvailable;

            //perkCostText.color = Color.white;
            perkCurrencyImage.color = Color.white;
            perkCostText.color = Color.white;
            //_image.color = Color.white;
            perkLevelImage.color = availableColor;
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
        nameText.text = perkNameText;
        explanationText.text = perkExplanationText;
    }

    public void OnMouseExit()
    {
        isMouseHover = false;
        tooltip.gameObject.SetActive(false);
        nameText.text = "";
        explanationText.text = "";
    }

    public void OnMouseStay()
    {
        tooltip.transform.position = Input.mousePosition + new Vector3(300 / 2 + 20 , - 25, 0);
    }
}