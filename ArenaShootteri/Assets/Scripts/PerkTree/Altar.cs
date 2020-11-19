using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour
{
    public float verticalFrequency = 1f;
    public float vertical = 1f;
    Vector3 position;

    Camera cam;
    GameObject player;

    GameObject playerCanvas;
    public GameObject perkCanvas;

    Text text;

    bool isText = false;
    bool isAltarActive = false;
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
        float verticalMovement = ((Mathf.Sin(Time.time * verticalFrequency) * 0.5f) + 0.5f) * vertical;
        transform.position = position + Vector3.up * verticalMovement;

        //Debug.Log(cameraDot);

        if (Vector3.Distance(player.transform.position, position) < 2)
        {
            float cameraDot = Vector3.Dot(cam.transform.forward, Vector3.Normalize(cam.transform.position - position));

            if (cameraDot < -.8)
            {
                if (!isText)
                {
                    text.text = "press E to activate";
                    isText = true;
                }
                
                if (Input.GetKeyDown(KeyCode.E) || isAltarActive && Input.GetKeyDown(KeyCode.E))
                {
                    if (!isAltarActive)
                    {
                        Cursor.lockState = CursorLockMode.Confined;
                        player.GetComponent<Player>().PlayerControl(false);
                        perkCanvas.SetActive(true);
                        isAltarActive = true;
                        //Debug.Log("Activate Altar");
                    }
                    else
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        player.GetComponent<Player>().PlayerControl(true);
                        perkCanvas.SetActive(false);
                        isAltarActive = false;
                        //Debug.Log("Deactivate Altar");
                    }

                }
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
