using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkBench : MonoBehaviour
{
    GunAttributes gunAttributes;
    Transform player;

    public Text text;
    
    public bool isModding;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gunAttributes = GameObject.Find("GUN2 1").GetComponent<GunAttributes>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        
        if (dist < 0.8f)
        {
            if (!isModding)
            {
                text.enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                gunAttributes.ChangeUI();
                isModding = !isModding;
            }
        }
        else
        {
            text.enabled = false;
            if (isModding)
            {
                gunAttributes.ChangeUI();
                isModding = !isModding;
            }
        }        
    }
}
