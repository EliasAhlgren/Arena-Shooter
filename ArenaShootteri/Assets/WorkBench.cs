using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkBench : MonoBehaviour
{
    public GunAttributes gunAttributes;

    public bool isModding;
    
    public float verticalFrequency = 1f;
    public float vertical = 1f;
    Vector3 position;

    Camera cam;
    GameObject player;

    GameObject playerCanvas;
    public GameObject perkCanvas;
    public GameObject tooltip;

    Text text;

    bool isText = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = GameObject.FindWithTag("Player");
        playerCanvas = player.transform.Find("PlayerCanvas").gameObject;

        text = playerCanvas.GetComponentInChildren<Text>();

        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

        //Debug.Log(cameraDot);

        if (Vector3.Distance(player.transform.position, position) < 4)
        {
            float cameraDot = Vector3.Dot(cam.transform.forward, Vector3.Normalize(cam.transform.position - position));

            if (cameraDot < -.8)
            {
                if (!isText && !isModding)
                {
                    text.text = "wörk";
                    isText = true;
                }
                else if (isText && isModding)
                {
                    text.text = "";
                    isText = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    gunAttributes.ChangeUI();
                }
                
                /*
                if (Input.GetKeyDown(KeyCode.E) || isModding && Input.GetKeyDown(KeyCode.Escape) || isModding && Input.GetKeyDown(KeyCode.E))
                {
                    if (!isModding)
                    {
                        //[SOUND] perk altar activation sound (One Shot)

                        Cursor.lockState = CursorLockMode.Confined;
                        player.GetComponent<PlayerCharacterControllerRigidBody>().PlayerControl(false);
                        gunAttributes.ChangeUI();
                        isModding = true;
                        //Debug.Log("Activate Altar");
                    }
                    else
                    {
                        //[SOUND] perk altar deactivation sound (One Shot)

                        Cursor.lockState = CursorLockMode.Locked;
                        player.GetComponent<PlayerCharacterControllerRigidBody>().PlayerControl(true);
                        gunAttributes.ChangeUI();
                        isModding = false;

                        //Debug.Log("Deactivate Altar");
                    }

                }
                
                */
            }
            else
            {
                if (isText)
                {
                    text.text = "";
                    isText = false;
                }
                
            }
            
            //canvas.SetActive(true);
        }
        else
        {
            if (isText)
            {
                text.text = "";
                isText = false;
            }
            //canvas.SetActive(false);
        }
    }
}
