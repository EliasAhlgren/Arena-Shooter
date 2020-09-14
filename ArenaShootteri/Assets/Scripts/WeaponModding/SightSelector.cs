using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SightSelector : MonoBehaviour
{
    public Sight[] availableSights;

    public GameObject[] buttons;

    public GameObject emptySlot;
    
    public Button currentSightSlot;

    public GameObject sightRail;

    public GameObject currentSight;
    
    [SerializeField] private bool placingObject;

    public LineRenderer line;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < availableSights.Length; i++)
        {
            buttons[i].GetComponent<Image>().sprite = availableSights[i].Icon;
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
        currentSightSlot.GetComponent<Image>().sprite = availableSights[int.Parse(buttonName)].Icon;
        if (sightRail.transform.childCount > 2)
        {
            Destroy(sightRail.transform.GetChild(2).gameObject);
        }

        GameObject newSight = Instantiate(availableSights[int.Parse(buttonName)].Prefab, sightRail.transform.GetChild(0).position,
            Quaternion.identity, sightRail.transform);

        currentSight = newSight;

        placingObject = true;
    }

    public void OnSelectEmpty()
    {
        
        currentSightSlot.GetComponent<Image>().sprite = emptySlot.GetComponent<Button>().image.sprite;
        if (sightRail.transform.childCount > 2)
        {
            Destroy(sightRail.transform.GetChild(2).gameObject);
        }
    }

    
    
    // Update is called once per frame
    void Update()
    {
        if (placingObject)
        {
            line.SetPosition(0,currentSightSlot.transform.position);
            line.SetPosition(1,currentSight.transform.position);
        }
        
        if (placingObject)
        {
            Vector3 railPos = sightRail.transform.GetChild(1).position;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray,out hit))
                    {
                        railPos.z = hit.point.z;
                        
                        if (sightRail.transform.GetChild(1).gameObject.GetComponent<Collider>().bounds.Contains(railPos))
                        {
                            Vector3 newPos = currentSight.transform.position;
                                                    newPos.z = hit.point.z;
                                                    currentSight.transform.position = newPos;
                        }
                        
                    }
                }

        if (Input.GetMouseButtonDown(0))
        {
            placingObject = false;
        }
        }
        
}
