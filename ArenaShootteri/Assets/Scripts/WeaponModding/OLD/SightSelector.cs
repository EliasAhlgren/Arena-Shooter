using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SightSelector : MonoBehaviour
{
    
    /////////////////////
    /// tää scripti on turha ja olemassa vaan jos mun pitää kopioida tästä jotain käytä ModSelectionia
    ///////////////////  
    
    public Sight[] availableSights;

    public Sight[] selectedSights;
    
    public GameObject[] buttons;

    public GameObject emptySlot;
    
    public Button currentSightSlot;

    public GameObject sightRail;

    public GameObject currentSight;
    
    [SerializeField] private bool placingObject;

    public LineRenderer line;

    private Vector3 firstPosition;

    public Transform sightLocalPos;

    private int childCountAtStart;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedSights = new Sight[3];
        
        childCountAtStart = sightRail.transform.childCount;
        
        sightLocalPos = sightRail.transform.GetChild(2);
        
        firstPosition = currentSightSlot.transform.GetChild(0).transform.position;

        for (int i = 0; i < selectedSights.Length; i++)
        {
            selectedSights[i] = availableSights[Random.Range(0, availableSights.Length)];
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Image>().sprite = selectedSights[i].Icon;
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
        sightLocalPos.localPosition = Vector3.zero;

        currentSightSlot.GetComponent<Image>().sprite = selectedSights[int.Parse(buttonName)].Icon;
        if (sightRail.transform.childCount > childCountAtStart)
        {
            Destroy(sightRail.transform.GetChild(childCountAtStart).gameObject);
        }

        GameObject newSight = Instantiate(selectedSights[int.Parse(buttonName)].Prefab, sightRail.transform.GetChild(0).position,
            Quaternion.identity, sightRail.transform);
        newSight.transform.rotation = sightRail.transform.rotation;
        
        currentSight = newSight;

        placingObject = true;
    }

    public void OnSelectEmpty()
    {
        line.SetPosition(0,Vector3.zero);
        line.SetPosition(1,Vector3.zero);

        
        currentSightSlot.GetComponent<Image>().sprite = emptySlot.GetComponent<Button>().image.sprite;
        sightLocalPos.localPosition = Vector3.zero;
        if (sightRail.transform.childCount > childCountAtStart)
        {
            Destroy(sightRail.transform.GetChild(childCountAtStart).gameObject);
        }
    }

    
    
    // Update is called once per frame
    void Update()
    {
        line.SetPosition(1,currentSight.transform.position);
        if (placingObject)
        {
            
            line.SetPosition(0,firstPosition);
            
        }
        
        if (placingObject)
        {
            Vector3 railPos = sightRail.transform.GetChild(1).position;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray,out hit))
                    {
                        railPos.z = hit.point.z;

                        sightLocalPos.position = hit.point;
                        
                        if (sightLocalPos.localPosition.x > 0)
                        {
                           
                            Vector3 v3 = sightLocalPos.position;
                            v3.z = -sightLocalPos.position.z;
                            v3.z += 5f;
                            sightLocalPos.position = v3;
                        }
                        
                        if (sightRail.transform.GetChild(1).gameObject.GetComponent<Collider>().bounds.Contains(railPos))
                        {
                            
                            Vector3 newPos = sightLocalPos.position;
                            newPos.x = 0;
                            newPos.y = 1.27f;
                            currentSight.transform.localPosition = newPos;
                        }
                        
                    }
                }

        if (Input.GetMouseButtonDown(0))
        {
            placingObject = false;
        }
        }
        
}
