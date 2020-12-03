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

    public Text text;

    bool isText = false;

    // Start is called before the first frame update
    void Start()
    {
        gunAttributes = GameObject.Find("GUN2 1").GetComponent<GunAttributes>();
        player = GameObject.FindWithTag("Player");
        cam = Camera.main;
        player = GameObject.FindWithTag("Player");

        

        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(GetComponent<Collider>().bounds.center, transform.position);

        if (dist < 0.8f)

            //Debug.Log(cameraDot);

            if (Vector3.Distance(player.transform.position, position) < 4)
            {

                float cameraDot = Vector3.Dot(cam.transform.forward,
                    Vector3.Normalize(cam.transform.position - position));

                if (cameraDot < -.8)
                {
                    text.enabled = true;
                    if (!isModding)
                    {
                        text.text = "wörk";
                        isText = true;
                    }
                    else if (isModding)
                    {
                        text.text = "";
                        isText = false;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        gunAttributes.ChangeUI();
                    }


                }


                //canvas.SetActive(true);
            }

    }
}

