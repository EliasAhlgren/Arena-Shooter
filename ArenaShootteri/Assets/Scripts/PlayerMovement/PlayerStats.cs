using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //[SerializeField]

    public float currentHealth;
    public float defaultHealth = 100f;
    public float health = 100f;
    public float perkHealth = 0;

    public float dRS = 12f;
    public float runSpeed = 12f;

    public float dWS = 6f;
    public float walkSpeed = 6f;

    public float dSPC = 14f;
    public float slideSpeedControl = 14f;

    public float dSD = 1f;
    public float slideDuration = 1f;

    public float dJH = 3f;
    public float jumpHeight = 3f;

    public float dAMM = .2f;
    public float airMoveModifier = .2f;

    public Perk[] perks;
    private Perk[] previousPerks;

    void Start()
    {
        health = defaultHealth;

        CheckPerks();

        currentHealth = health;
        previousPerks = perks;
    }

    private void Update()
    {
        if (previousPerks != perks)
        {
            Debug.Log("perks cheked");
            CheckPerks();
            previousPerks = perks;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            CheckPerks();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            currentHealth -= 10f;
        }

    }

    void CheckPerks()
    {
        perkHealth = 0;
        runSpeed = dRS;
        walkSpeed = dWS;
        slideSpeedControl = dSPC;
        slideDuration = dSD;
        jumpHeight = dJH;
        airMoveModifier = dAMM;

        foreach(var Perk in perks)
        {
            Perk.SetStat(this.gameObject);
        }

        health = defaultHealth + perkHealth;

        if (currentHealth > health)
        {
            currentHealth = health;
        }
    }

    private void LateUpdate()
    {
        if (currentHealth <= 0)
        {
            if (gameObject.GetComponent<PlayerCharacterControllerRigidBody>().isAlive)
            {
                gameObject.GetComponent<PlayerCharacterControllerRigidBody>().killPlayer();
            }
            else
            {
                currentHealth = 1;
            }
            
        }
    }

}
