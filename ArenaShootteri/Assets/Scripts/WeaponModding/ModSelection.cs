using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ModSelection : MonoBehaviour
{
    public string RailName;
    
    public bool isTwoSided;
    
    public Mod[] availableMods;

    public Mod[] selectedMods;
    
    public GameObject[] buttons;

    public GameObject emptySlot;
    
    public Button currentModSlot;

    public GameObject ModRail;

    public GameObject currentMod;
    
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
        
        emptySlot.SetActive(!emptySlot.activeInHierarchy);
        Debug.Log("ChangeCurrent");
    }

    public void OnSelectNew(String buttonName)
    {
        ModLocalPos.localPosition = Vector3.zero;

        currentModSlot.GetComponent<Image>().sprite = selectedMods[int.Parse(buttonName)].Icon;
        if (ModRail.transform.childCount > childCountAtStart)
        {
            Destroy(ModRail.transform.GetChild(childCountAtStart).gameObject);
        }

        GameObject newMod = Instantiate(selectedMods[int.Parse(buttonName)].Prefab, ModRail.transform.GetChild(0).position,
            Quaternion.identity, ModRail.transform);
        newMod.transform.rotation = ModRail.transform.rotation;
        
        currentMod = newMod;

        placingObject = true;
    }

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
       //line.SetPosition(2,ModRail.transform.GetChild(2).position);
        
        if (placingObject)
        {
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
            placingObject = false;
        }
        }
        
}