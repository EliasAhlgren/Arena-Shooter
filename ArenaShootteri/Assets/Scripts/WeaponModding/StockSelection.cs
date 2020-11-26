using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// Tää scripti on melkein sama kun ModSelection tästä puuttuu vain kaikki modin liikuttamiseen liittyvä
// ja tässä ei ole mahdollisuutta valita tyhjää modia
// tää on tukin, piipun ja muiden joita ei tarvi liikuttaa asettamista vartren

//turha
public class StockSelection : MonoBehaviour
{
   public string RailName;
    
    public bool isTwoSided;
    
    public Mod[] availableMods;

    public Mod[] selectedMods;

    
    
    public GameObject[] buttons;

    // public GameObject emptySlot;
    
    public Button currentModSlot;

    public GameObject ModRail;

    private GameObject currentMod;
    
    public Mod currenModStats;
    
    // defaultModiin voi laittaa modin valmiiksi niin se spawanataan Startissa
    public Mod defaultMod;
    
    private bool placingObject;

    [SerializeField] public LineRenderer line;

    private Vector3 firstPosition;
    

    private int childCountAtStart;
    
    public delegate void ModChosen();

    public event ModChosen OnModChosen;

    
    // Start is called before the first frame update
    void Start()
    {
        childCountAtStart = ModRail.transform.childCount;
        
        
        if (defaultMod)
        {
            currenModStats = defaultMod;
            Debug.Log("initializing with a mod");
            GameObject newMod = Instantiate(defaultMod.Prefab, ModRail.transform.GetChild(0).position,
                Quaternion.identity, ModRail.transform);
            currentMod = newMod;
        }
        
        selectedMods = new Mod[3];
        
        
        
        //ModLocalPos = ModRail.transform.GetChild(2);
        
        firstPosition = currentModSlot.transform.GetChild(0).transform.position;

        for (int i = 0; i < selectedMods.Length; i++)
        {
            selectedMods[i] = availableMods[Random.Range(0, availableMods.Length)];
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Image>().sprite = selectedMods[i].Icon;
        }
        
    }

    public void OnChangeCurrent()
    {
        foreach (var VARIABLE in buttons)
        {
            VARIABLE.SetActive(!VARIABLE.activeInHierarchy);
        }
        
       // emptySlot.SetActive(!emptySlot.activeInHierarchy);
        Debug.Log("ChangeCurrent");
    }

    public void OnSelectNew(String buttonName)
    {
        //ModLocalPos.localPosition = Vector3.zero;
        if (currentMod)
        {
            Debug.Log("Already has a mod");
            Destroy(currentMod);
        }
        
        currentModSlot.GetComponent<Image>().sprite = selectedMods[int.Parse(buttonName)].Icon;
        if (ModRail.transform.childCount > childCountAtStart)
        {
            Destroy(ModRail.transform.GetChild(childCountAtStart).gameObject);
        }

        GameObject newMod = Instantiate(selectedMods[int.Parse(buttonName)].Prefab, ModRail.transform.GetChild(0).position,
            Quaternion.identity, ModRail.transform);
        newMod.transform.rotation = ModRail.transform.rotation;
        
        currenModStats = selectedMods[int.Parse(buttonName)];
        
        currentMod = newMod;

        OnModChosen();

        //placingObject = true;
    }

    public void OnSelectEmpty()
    {
        line.SetPosition(0,Vector3.zero);
        line.SetPosition(1,Vector3.zero);

        Debug.Log("1");
        //currentModSlot.GetComponent<Image>().sprite = emptySlot.GetComponent<Button>().image.sprite;
        
        //ModLocalPos.localPosition = Vector3.zero;
        if (ModRail.transform.childCount > childCountAtStart)
        {
            Debug.Log("2");

            Destroy(ModRail.transform.GetChild(childCountAtStart).gameObject);
        }

        currenModStats = null;
        
        OnModChosen();

        
    }

    
    
    // Update is called once per frame
    void Update()
    {
       //line.SetPosition(2,ModRail.transform.GetChild(2).position);
        
        

        }
        
}