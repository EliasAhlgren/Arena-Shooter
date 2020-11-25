﻿using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float verticalFrequency = 1f;
    public float vertical = .1f;
    public float rotation = 20f;

    public GameObject platform;
    protected PickupPlatform platformSript;
    protected bool platfromIS = false;

    protected GameObject player;
    protected PlayerCharacterControllerRigidBody playerScript;
    public Rigidbody rb;

    public float respawnTime = 5;

    Collider col;
    Vector3 position;


    void Start()
    {
        if (platform != null)
        {
            if (platformSript = platform.GetComponent<PickupPlatform>())
            {
                platfromIS = true;
            }
        }
        
             

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerCharacterControllerRigidBody>();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.isKinematic = true;
        col.isTrigger = true;

        if (platfromIS) transform.position = platform.transform.position + Vector3.up * 1.5f;

        position = transform.position;
    }

    private void Update()
    {
        float verticalMovement = ((Mathf.Sin(Time.time * verticalFrequency) * 0.5f) + 0.5f) * vertical;
        transform.position = position + Vector3.up * verticalMovement;

        transform.Rotate(Vector3.up, rotation * Time.deltaTime, Space.Self);
    }
}