using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController playerController;

    [Header("Player Stats")]
    public bool isAlive = true;
    public float health = 100f;
    public float defaultMaxHealth = 100f;
    public float maxHealth = 100f;
    public float stamina = 100f;

    public float velocity;

    [Header("Player Controls")]
    public bool playerControl = true;
    
    //mouse input multiplier
    public float mouseSensitivity = 100f;
    public bool rawMouseInput = false;
    public bool rawMovementInput = false;

    public float defaultSpeed = 6f;

    public float damageModifier = 1;
    public float reloadModifier = 1;
    public float defenseModifier = 1;
    public float fireRateModifier = 1;
    public float spawnRateModifier = 1;
    public float rageDamageModifier = 1;

    // perk cooldowns
    float rageModeActiveTime = 0;
    float activePerkCooldown = 0;
    float activePerkActiveTime = 0;



    [Header("Perk Unlock")]
    public bool doubleJumpUnlocked = false;
    public bool slideUnlocked = true;
    public bool dodgeUnlocked = true;
    public bool wallJumpUnlocked = true;
    public bool wallRunUnlocked = true;
    [Space(10)]
    public bool rageModeUnlocked = false;
    public bool damageAuraUnlocked = false;
    public bool groundSlamUnlocked = false;
    [Space(10)]
    public bool resurectionUnlocked = false;
    public bool divineShieldUnlocked = false;
    public bool theHolyLightUnlocked = false;

    [Header("Active Perk Modifiers")]
    public float damageAuraCooldown = 10;
    public float damageAuraTime = 10;
    public float theHolyLightCooldown = 10;
    public float divineShieldCooldown = 10;
    public float divineShieldTime = 10;

    [Header("Perk Modifiers")]
    public float speedMod = .05f;
    public float damageMod = .05f;
    public float fireRateMod = -.05f;
    public float reloadMod = -.05f;
    public float healthMod = .05f;
    public float spawnRateMod = -.05f;
    public float defenseMod = -.05f;
    [Space(10)]
    public float damageAuraRange = 5;
    public float rageMod = 1;
    public float rageModRegKills = 3;
    public float rageModTimer = 10;
    [Space(10)]

    List<float> rageKills = new List<float>();
    bool rageMode = false;

    float invulnerability = 0f;
    bool invulnerable = false;

    public float testForce = 10;
    public Vector3 testForceVector = new Vector3(1, 0, 1);

    public GameObject damageAuraModel;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        CheckPerks();
    }

    // Update is called once per frame
    void Update()
    {
        
        //playerController.baseSpeed = defaultSpeed * speedMod;

        velocity = playerController.PlayerVelocity();

        Stamina();  
        Invulnerability();

        if (rageModeUnlocked) RageMode();
        if (damageAuraUnlocked) DamageAura();
        if (theHolyLightUnlocked) TheHolyLight();

        if (Input.GetKeyDown(KeyCode.N))
        {
            PerkTreeReader.Instance.AddPerkPoint(10);
        }

        // active skill "Y"
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (activePerkActiveTime <= 0 && activePerkCooldown <= 0)
            {
                if (divineShieldUnlocked)
                {
                    DivineShield();
                }
                else if (damageAuraUnlocked)
                {
                    activePerkActiveTime = damageAuraTime;
                    activePerkCooldown = damageAuraCooldown;
                }
                else if (theHolyLightUnlocked)
                {
                    TheHolyLight();
                }
            }
        }

        // add force test "T"
        if (Input.GetKeyDown(KeyCode.T))
        {
            playerController.PlayerAddForce(testForceVector, testForce);            
        }

        // kill player test "j"
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isAlive)
            {
                playerController.KillPlayer();
            }
            else
            {
                playerController.RevivePlayer();
            }

        }

        // player control test "k"
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (playerControl)
            {
                PlayerControl(false);
            }
            else
            {
                PlayerControl(true);
            }
        }

        // time scale test "l"
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }

        // rage mode test "i"
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddRageKill();
        }

        // add invulnerability test "o"
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddInvulnerability(5);
        }

        // take damage test "P"
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(-50f, false);
        }

        if (activePerkActiveTime <= 0)
        {
            if (activePerkCooldown > 0)
            {
                activePerkCooldown -= 1 * Time.deltaTime;
            }
            
        }
        else
        {
            activePerkActiveTime -= 1 * Time.deltaTime;
        }
    }

    public void CheckPerks()
    {
        //Agility
        doubleJumpUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(1);
        slideUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(2);
        wallJumpUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(3);
        wallRunUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(4);
        dodgeUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(5);
        playerController.baseSpeed = defaultSpeed * 1 + speedMod * PerkTreeReader.Instance.IsPerkLevel(6);

        //Offence
        damageModifier = 1 + damageMod * PerkTreeReader.Instance.IsPerkLevel(7);
        rageModeUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(8);
        fireRateModifier = 1 + fireRateMod * PerkTreeReader.Instance.IsPerkLevel(9);
        reloadModifier = 1 + reloadMod * PerkTreeReader.Instance.IsPerkLevel(10);
        groundSlamUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(11);
        damageAuraUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(12);

        //Utility
        maxHealth = defaultMaxHealth * 1 + healthMod * PerkTreeReader.Instance.IsPerkLevel(13);
        divineShieldUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(14);
        resurectionUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(15);
        defenseModifier = 1 + defenseMod * PerkTreeReader.Instance.IsPerkLevel(16);
        theHolyLightUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(17);
        spawnRateModifier = 1 + spawnRateMod * PerkTreeReader.Instance.IsPerkLevel(18);
        /*
        if (true) doubleJumpUnlocked = true;
        if (true) slideUnlocked = true;
        if (true) dodgeUnlocked = true;
        if (true) wallJumpUnlocked = true;
        if (true) wallRunUnlocked = true;
        if (true) playerController.baseSpeed = defaultSpeed * speedModifier;

        if (true) damageModifier = 1 + damageMod * 1;
        if (true) rageModeUnlocked = true;
        if (true) fireRateModifier = 1 + fireRateMod * 1;
        if (true) reloadModifier = 1 + reloadMod * 1;
        if (true) damageAuraUnlocked = true;
        if (true) groundSlamUnlocked = true;

        if (true) maxHealth = defaultMaxHealth * 1 + healthMod * 1;
        if (true) divineShieldUnlocked = true;
        if (true) resurectionUnlocked = true;
        if (true) defenseModifier = 1 + defenseMod * 1;
        if (true) theHolyLightUnlocked = true;
        if (true) spawnRateModifier = 1 + spawnRateMod * 1;
        */
    }

    // player functions
    void Stamina()
    {
        if (playerController.isSliding || playerController.isAirborne)
        {

        }
        else if (playerController.PlayerVelocity() > 0 && playerController.isRunning)
        {
            stamina += -10 * Time.deltaTime;
        }
        else if (stamina < 100)
        //if (stamina < 100 && isRunning && playerVelocity > 0)
        {
            stamina += 10 * Time.deltaTime;
        }

        if (stamina > 100)
        {
            stamina = 100;
        }
    }

    public void PlayerControl(bool var)
    {
        if (var)
        {
            playerControl = true;
        }
        else
        {
            playerControl = false;
        }
    }

    public void AddInvulnerability(float time)
    {
        invulnerability += time;
    }

    private void Invulnerability()
    {
        if (invulnerability > 0)
        {
            invulnerable = true;
            invulnerability -= Time.deltaTime;
        }
        else
        {
            invulnerable = false;
        }
    }

    public void TakeDamage(float damage, bool isRanged)
    {
        if (!invulnerable)
        {
            if (isRanged)
            {
                health += damage * defenseMod;
            }
            else
            {
                health += damage;
            }

            if (health <= 0)
            {
                if (isAlive)
                {
                    playerController.KillPlayer();
                }
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }


    // Perk functions
    public void AddRageKill()
    {
        if (rageModeUnlocked)
        {
            if (!rageMode)
            {
                rageKills.Add(Time.time);
            }
        }
    }

    private void RageMode()
    {
        if (rageModeActiveTime <= 0)
        {
            if (rageKills.Count >= rageModRegKills)
            {
                rageModeActiveTime = 10;
                rageMode = true;
                rageDamageModifier = 1 + rageMod;
                rageKills.Clear();
            }
            else
            {
                rageMode = false;
                rageDamageModifier = 1;
            }
            if (rageKills.Count > 0)
            {
                if (Time.time - rageKills[0] >= rageModTimer)
                {
                    rageKills.RemoveAt(0);
                }
            }
        }
        else
        {
            rageModeActiveTime -= Time.deltaTime;
        }
    }

    public void GroundSlam()
    {
        int layerMask = LayerMask.GetMask("EnemyHitbox");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageAuraRange, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.CompareTag("Enemy"))
            {
                hitCollider.transform.parent.transform.GetComponent<Grunt>().StartCoroutine("Die");
            }
        }
    }


    // Active Perks functions
    private void DamageAura()
    {
        if (activePerkActiveTime > 0)
        {
            damageAuraModel.SetActive(true);

            if (playerController.isCrouching)
            {
                damageAuraModel.transform.localScale = new Vector3(damageAuraRange * 2, damageAuraRange * 4, damageAuraRange * 2);
            }
            else
            {
                damageAuraModel.transform.localScale = new Vector3(damageAuraRange * 2, damageAuraRange * 2, damageAuraRange * 2);
            }
            
            int layerMask = LayerMask.GetMask("EnemyHitbox");

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageAuraRange, layerMask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform.CompareTag("Enemy"))
                {
                    hitCollider.transform.parent.transform.GetComponent<Grunt>().StartCoroutine("Die");

                }
            }
        }
        else
        {
            damageAuraModel.SetActive(false);
        }
    }

    private void DivineShield()
    {
        activePerkActiveTime = divineShieldTime;
        activePerkCooldown = divineShieldCooldown;
        AddInvulnerability(divineShieldTime);
    }

    private void TheHolyLight()
    {
        activePerkCooldown = theHolyLightCooldown;

        int layerMask = LayerMask.GetMask("EnemyHitbox");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageAuraRange, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.CompareTag("Enemy"))
            {
                hitCollider.transform.parent.transform.GetComponent<Grunt>().StartCoroutine("Die");

            }
        }
    }
}
