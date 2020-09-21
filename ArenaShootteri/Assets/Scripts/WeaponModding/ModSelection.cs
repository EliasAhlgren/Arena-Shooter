using System;
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

    public GameObject currentMod;
    
    // jos siirtää modia
    private bool placingObject;
    
    [SerializeField] public LineRenderer line;
    
    private Vector3 firstPosition;

    public Transform ModLocalPos;

    private int childCountAtStart;
    
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
    public void OnSelectNew(String buttonName)
    {
        ModLocalPos.localPosition = Vector3.zero;

        currentModSlot.GetComponent<Image>().sprite = selectedMods[int.Parse(buttonName)].Icon;
        
        //tuhoa vanha modi 
        if (ModRail.transform.childCount > childCountAtStart)
        {
            Destroy(ModRail.transform.GetChild(childCountAtStart).gameObject);
        }
        
        // Luo uusi modi ModDefaultPosiin
        GameObject newMod = Instantiate(selectedMods[int.Parse(buttonName)].Prefab, ModRail.transform.GetChild(0).position,
            Quaternion.identity, ModRail.transform);
        newMod.transform.rotation = ModRail.transform.rotation;
        
        currentMod = newMod;

        placingObject = true;
    }

    //OnSelectEmpty kutsutaan kun painaa tyhjää UI nappia
    public void OnSelectEmpty()
    {
        line.SetPosition(0,Vector3.zero);
        line.SetPosition(1,Vector3.zero);

        
        currentModSlot.GetComponent<Image>().sprite = emptySlot.GetComponent<Button>().image.sprite;
        ModLocalPos.localPosition = Vector3.zero;
        if (ModRail.transform.childCount > childCountAtStart)
        {
            Destroy(ModRail.transform.GetChild(childCountAtStart).gameObject);
        }
    }

    
    
    // Update is called once per frame
    void Update()
    {
       
        
        if (placingObject)
        {
           // railPos on Railin y ja x ja hiiren z positio
            Vector3 railPos = ModRail.transform.GetChild(1).position;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray,out hit))
                    {
                        railPos.z = hit.point.z;

                        ModLocalPos.position = hit.point;

                        if (isTwoSided)
                        {
                            if (ModLocalPos.localPosition.x > 0)
                            {
                                Vector3 v3 = ModLocalPos.position;
                                v3.z = -ModLocalPos.position.z;
                                v3.z += 5f;
                                ModLocalPos.position = v3;
                            }
                        }
                        
                        // jos hiiren position z käännettynä aseen local spaceen on kiskon colliderin sisällä
                        if (ModRail.transform.GetChild(1).gameObject.GetComponent<Collider>().bounds.Contains(railPos))
                        {
                            
                            Vector3 newPos = ModLocalPos.position;
                            newPos.x = 0;
                            newPos.y = 1.27f;
                            currentMod.transform.localPosition = newPos;
                        }
                        
                    }
                }

        if (Input.GetMouseButtonDown(0))
        {
            // Lopettaa modin liikuttamisen
            placingObject = false;
        }
        }
        
}