﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ModSelection : MonoBehaviour
{
    // Slotin nimi. ei käytetä tällä hetkellä mihinkään
    public string RailName;
    
    // pystyykö slottiin laittaa molemmilta puolilta (x ja -x)
    public bool isTwoSided;
    
    // tää muuttuu vielä johonki järkevämpään ratkasuun
    public Mod[] availableMods;

    // tää myös
    public Mod[] selectedMods;
    
    public GameObject[] buttons;

    // täs tulis näitä eri UI slotteja
    
    public GameObject emptySlot;
    
    public Button currentModSlot;

    // Koko slotin parent objecti
    public GameObject ModRail;

    // nykyisen modin GameObject
    public GameObject currentMod;
    
    // nykyisen modin Mod assetti statien ottamista varten
    public Mod currenModStats;
    
    // jos siirtää modia
    private bool placingObject;

    public Camera moddingCamera;
    
    
    
    private Vector3 firstPosition;

    public Transform ModLocalPos;

    private int childCountAtStart;
    
    public delegate void ModChosen();

    public event ModChosen OnModChosen;

    public bool isAutoPlaced;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
        selectedMods = new Mod[3];
        
        childCountAtStart = ModRail.transform.childCount;
        
        ModLocalPos = ModRail.transform.GetChild(2);
        
        firstPosition = currentModSlot.transform.GetChild(0).transform.position;

        //tää on vaan väliaikanen
        for (int i = 0; i < selectedMods.Length; i++)
        {
            selectedMods[i] = availableMods[Random.Range(0, availableMods.Length)];
        }

        // laita oikeat ikonit UI nappeihin
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Image>().sprite = selectedMods[i].Icon;
        }
        
    }

    // OnChangeCurrent kutsutaan jos painaa CurrentMod UI nappia
    public void OnChangeCurrent()
    {
        
        
        foreach (var button in buttons)
        {
            button.SetActive(!button.activeInHierarchy);
        }
        
        emptySlot.SetActive(!emptySlot.activeInHierarchy);
        Debug.Log("ChangeCurrent");
    }

    // OnSelectNew kutsutaan jos valitsee uuden modin 
    public void OnSelectNew(String buttonName) // buttonName on stringi jonka UI nappi passaa ku se kutsuu OnClick eventin
    {
        ModLocalPos.localPosition = Vector3.zero;

        if (selectedMods[int.Parse(buttonName)].Icon)
        {
            currentModSlot.GetComponent<Image>().sprite = selectedMods[int.Parse(buttonName)].Icon;
        }
        
        
        //tuhoa vanha modi 
        if (ModRail.transform.childCount > childCountAtStart)
        {
            Destroy(ModRail.transform.GetChild(childCountAtStart).gameObject);
        }
        
        // Luo uusi modi ModDefaultPosiin
        GameObject newMod = Instantiate(selectedMods[int.Parse(buttonName)].Prefab, ModRail.transform.GetChild(0).position,
            Quaternion.identity, ModRail.transform);
        newMod.transform.rotation = ModRail.transform.rotation;

        //valitse oikea modi currentModStatsiin
        currenModStats = selectedMods[int.Parse(buttonName)];
        
        currentMod = newMod;
         
        placingObject = true;
        
        OnModChosen();
    }

    //OnSelectEmpty kutsutaan kun painaa tyhjää UI nappia
    public void OnSelectEmpty()
    {
       

        
        currentModSlot.GetComponent<Image>().sprite = emptySlot.GetComponent<Button>().image.sprite;
        ModLocalPos.localPosition = Vector3.zero;
        if (ModRail.transform.childCount > childCountAtStart)
        {
            Destroy(ModRail.transform.GetChild(childCountAtStart).gameObject);
        }

        currenModStats = null;
        
        OnModChosen();

        
    }

    
    
    // Update is called once per frame
    void Update()
    {
       
        
        if (placingObject)
        {
            isAutoPlaced = GameObject.Find("AutoPlaceToggle").GetComponent<Toggle>().isOn;
            Debug.Log("Now Placing");
           // railPos on Railin y ja x ja hiiren z positio
            Vector3 railPos = ModRail.transform.GetChild(1).position;
            
            Ray ray = moddingCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray,out hit))
                    {
                        railPos.z = hit.point.z;

                        ModLocalPos.position = hit.point;

                        if (isTwoSided)
                        {
                            if (ModLocalPos.localPosition.x > 0)
                            {
                                Debug.Log("we fukin");
                                
                                Vector3 v3 = ModLocalPos.position;
                                v3.z = -ModLocalPos.position.z;
                                v3.z += 5f;
                                ModLocalPos.position = v3;
                            }
                        }
                        
                        // jos hiiren position z käännettynä aseen local spaceen on kiskon colliderin sisällä
                        if (ModRail.transform.GetChild(1).gameObject.GetComponent<Collider>().bounds.Contains(railPos))
                        {
                            Debug.Log("hittaa kovaa");
                            Vector3 newPos = ModLocalPos.localPosition;
                            newPos.x = 0;
                            newPos.y = 1.27f;
                            currentMod.transform.localPosition = newPos;
                        }    
                        
                    }
                }

        if (Input.GetMouseButtonDown(0) || isAutoPlaced)
        {
            // Lopettaa modin liikuttamisen
            placingObject = false;
           
            // tähän tulis modin valitsemis ääniefekti

           
        }
        }
        
}