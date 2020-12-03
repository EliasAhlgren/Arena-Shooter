using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class WorkBench : MonoBehaviour
{
    public GunAttributes gunAttributes;

    private GameObject player;
    
    public static bool isModding;

    public List<GameObject> collidingObjects;

    public Text text;

    public GameManager gameManager;
    
    void Start() {
        collidingObjects = new List<GameObject>();
        player = GameObject.FindWithTag("Player");
        gameManager = GameObject.FindWithTag("GameManagement").GetComponent<GameManager>();
    }
     
    void OnTriggerEnter(Collider collision) {
        if (!collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Add(collision.gameObject);
        }
    } 
     
    void OnTriggerExit(Collider collision) {
        if (collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject);
            text.text = "";
        }
    }

    private void Update()
    {
        if (isModding)
        {
            player.GetComponent<PlayerCharacterControllerRigidBody>().playerControl = false;
        }
        else
        {
            player.GetComponent<PlayerCharacterControllerRigidBody>().playerControl = true;
        }
        
            foreach (var VARIABLE in collidingObjects)
            {
                if (VARIABLE == player)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        isModding = !isModding;
                        gunAttributes.ChangeUI();
                        text.text = "";
                    }

                    if (!isModding)
                    {
                         text.text = "E to interact";
                    }
                   
                }
                else 
                {
                    if (!gameManager.textChanged)
                    {
                        text.text = "";
                    }

                    
                    
                }
            }
        
    }
}

