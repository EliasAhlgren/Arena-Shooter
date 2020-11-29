using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterControllerRigidBody : MonoBehaviour
{
    public Text playerHP;
    public Movement movement;
    [Header("Test Mode")]
    public bool testKeys = true;
    public bool shakeRotation = false;
    [Space(10)]
    public float magnitudeTest = 1;
    public float roughnesTest = 1;
    public float fadeInTest = 1;
    public float fadeOutTest = 1;
    [Space(10)]
    public float testForce = 10;
    public Vector3 testForceVector = new Vector3(1, 0, 1);

    [Header("Player Stats")]
    public bool isAlive = true;
    public float health = 100f;
    public float defaultMaxHealth = 100f;
    public float maxHealth = 100f;
    public float stamina = 100f;
    public float defaultSpeed = 6f;
    [Space(10)]
    public float damageModifier = 1;
    public float reloadModifier = 1;
    public float defenseModifier = 1;
    public float fireRateModifier = 1;
    public float spawnRateModifier = 1;
    public float rageDamageModifier = 1;


    [Header("Player Controls")]
    public bool playerControl = true;

    //mouse input multiplier
    public float mouseSensitivity = 100f;
    public bool rawMouseInput = false;
    public bool rawMovementInput = false;


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
    public float resurectionHP = 10;
    public float damageAuraRange = 5;
    public float rageMod = 1;
    public float rageModRegKills = 3;
    public float rageModTimer = 10;
    [Space(10)]

    List<float> rageKills = new List<float>();
    bool rageMode = false;

    float invulnerability = 0f;
    bool invulnerable = false;

    [Header("Utility")]
    //public float velocity;

    public Image deathImage;
    public GameObject deathCanvas;
    public float colorAlpha = 0f;

    public GameObject damageAuraModel;
    bool resurection = true;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        //deathCanvas = GetComponentInChildren<Canvas>(true).gameObject;
        //deathImage = deathCanvas.GetComponentInChildren<Image>(true);
        
        CheckPerks();
        health = maxHealth;
        CheckHealth();
    }

    // Update is called once per frame
    void Update()
    {

        //playerController.baseSpeed = defaultSpeed * speedMod;

        //velocity = movement.PlayerVelocity();

        Stamina();
        Invulnerability();

        if (rageModeUnlocked) RageMode();
        if (damageAuraUnlocked) DamageAura();
        if (theHolyLightUnlocked) TheHolyLight();


        if (playerControl && Time.timeScale > 0)
        {
            /*
            //shoot
            if (Input.GetMouseButtonDown(0))
            {
                //trigger pull sound?
                Shoot();
            }
            */

            // activate active perk "Y"
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (activePerkActiveTime <= 0 && activePerkCooldown <= 0)
                {
                    if (divineShieldUnlocked)
                    {
                        //[SOUND] divine shield perk activation sound (One Shot)

                        DivineShield();
                    }
                    else if (damageAuraUnlocked)
                    {
                        //[SOUND] damage aura perk activation sound (One Shot)

                        activePerkActiveTime = damageAuraTime;
                        activePerkCooldown = damageAuraCooldown;
                    }
                    else if (theHolyLightUnlocked)
                    {
                        //[SOUND] the holy light perk activation sound (One Shot)

                        TheHolyLight();
                    }
                    
                }
                else
                {
                    //[SOUND] perk activation failed sound (One Shot)

                }
            }

        }


        // key codes for testing purpose
        if (testKeys)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                StartCoroutine(movement.playerCamera.GetComponent<CameraShake>().Shake(magnitudeTest, roughnesTest, fadeInTest, fadeOutTest, shakeRotation));
                //StartCoroutine(movement.playerCamera.GetComponent<CameraShake>().Shake(1, 1,1,1, shakeRotation));
            }

            // add perk points test "N"
            if (Input.GetKeyDown(KeyCode.N))
            {
                PerkTreeReader.Instance.AddPerkPoint(10);
            }

            // add force test "T"
            if (Input.GetKeyDown(KeyCode.T))
            {
                movement.PlayerAddForce(testForceVector, testForce);
            }

            // kill player test "j"
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (isAlive)
                {
                    killPlayer();
                }
                else
                {
                    RevivePlayer();
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
                TakeDamage(50f, false);
            }

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
        movement.baseSpeed = defaultSpeed * (1 + speedMod * PerkTreeReader.Instance.IsPerkLevel(6));

        //Offence
        damageModifier = 1 + damageMod * PerkTreeReader.Instance.IsPerkLevel(7);
        rageModeUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(8);
        fireRateModifier = 1 + fireRateMod * PerkTreeReader.Instance.IsPerkLevel(9);
        reloadModifier = 1 + reloadMod * PerkTreeReader.Instance.IsPerkLevel(10);
        groundSlamUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(11);
        damageAuraUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(12);

        //Utility
        maxHealth = defaultMaxHealth * (1 + healthMod * PerkTreeReader.Instance.IsPerkLevel(13));
        divineShieldUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(14);
        resurectionUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(15);
        defenseModifier = 1 + defenseMod * PerkTreeReader.Instance.IsPerkLevel(16);
        theHolyLightUnlocked = PerkTreeReader.Instance.IsPerkUnlocked(17);
        spawnRateModifier = 1 + spawnRateMod * PerkTreeReader.Instance.IsPerkLevel(18);

        CheckHealth();
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
        if (movement.isSliding || movement.isAirborne)
        {

        }
        else if (movement.playerVelocity > 0 && movement.isRunning)
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

    // obsolete
    void Shoot()
    {
        //shooting sound
        int layerMask = LayerMask.GetMask("EnemyHitbox");
        RaycastHit hit;
        //raycast from center of screen if tagged enemy destroy target
        if (Physics.Raycast(movement.playerCamera.transform.position, movement.playerCamera.transform.forward, out hit, layerMask))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.parent.transform.GetComponent<Grunt>().StartCoroutine("Die");

            }
            Debug.Log(hit.transform.name);
        }
    }

    public void CameraShaker(float mag, float smooth, float start, float end)
    {
        StartCoroutine(movement.playerCamera.GetComponent<CameraShake>().Shake(mag, smooth, start, end, false));
    }

    public void PlayerAddForce(Vector3 dir, float force)
    {
        movement.PlayerAddForce(dir, force);
    }


    public void AddInvulnerability(float time)
    {
        invulnerability += time;
    }

    private void Invulnerability()
    {
        if (invulnerability > 0)
        {
            //[SOUND] divine shield sound (Continuous)

            invulnerable = true;
            invulnerability -= Time.deltaTime;
        }
        else
        {
            //[SOUND] divine shield deactivation sound (One Shot)

            invulnerable = false;
        }
    }

    public void TakeDamage(float damage, bool isRanged)
    {

        Debug.Log("player is taking " + damage + " damage. from Ranged enemy: " + isRanged);
        if (!invulnerable && isAlive)
        {
            if (isRanged)
            {
                //[SOUND] take damage sound (ranged) (One Shot)
                SoundManager.PlaySound("Oof");
                health -= damage * defenseMod;
            }
            else
            {
                //[SOUND] take damage sound (melee) (One Shot)
                SoundManager.PlaySound("Oof");
                health -= damage;
            }

            if (health <= 0)
            {
                if (isAlive)
                {
                    health = 0;
                    killPlayer();
                }
            }
            CheckHealth();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Grunt"))
        {
            if (collision.transform.root.GetComponent<Grunt>().canAttack)
            {
                var grunt = collision.transform.root.GetComponent<Grunt>();
                Debug.Log("Player hit by " + collision.transform.name);
                TakeDamage(grunt.damage, false);
                grunt.canAttack = false;
            }
        }

        if (collision.transform.CompareTag("Vipeltaja"))
        {
            if(collision.transform.root.GetComponent<Vipeltaja>().canAttack)
            {
                var vipel = collision.transform.root.GetComponent<Vipeltaja>();
                Debug.Log("Player hit by " + collision.transform.name);
                TakeDamage(vipel.damage, false);
                vipel.canAttack = false;
            }
        }

        if (collision.transform.CompareTag("Imp"))
        {
            if (collision.transform.root.GetComponent<Imp>().canAttack)
            {
                var imp = collision.transform.root.GetComponent<Imp>();
                Debug.Log("Player hit by " + collision.transform.name);
                TakeDamage(imp.damage, false);
                imp.canAttack = false;
            }
        }

        if (collision.transform.name.Equals("ChargeCollider"))
        {
            Debug.Log("Charge hit" + collision.transform.name);
            TakeDamage(collision.transform.root.GetComponent<Grunt>().chargeDamage, false);

        }
    }

    public void Heal(float healAmount)
    {
        if (isAlive)
        {
            health += healAmount;

            CheckHealth();
        }
    }

    public void CheckHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        playerHP.text = health.ToString();

        playerHP.text = health.ToString() + " / " + maxHealth.ToString();
    }

    public void killPlayer()
    {
        if (isAlive)
        {
            if (resurectionUnlocked && resurection)
            {
                //[SOUND] resurection sound (OneShot)

                health = resurectionHP;
                resurection = false;
            }
            else
            {
                //[SOUND] player death sound (One Shot)

                health = 0;
                CheckHealth();

                movement.ChangePlayerSize(false);
                //alphaLerp = 0f;
                deathCanvas.SetActive(true);
                colorAlpha = 0.3f;
                /*
                rayDistance = crouchRayDistance;
                groundCheck = new Vector3(0, -.25f, 0);
                //transform.localScale = new Vector3(1, .75f, 1);
                transform.localScale = new Vector3(transform.localScale.x, characterScale * 0.5f, transform.localScale.z);
                //transform.position = transform.position + new Vector3(0, -.75f, 0);

                currentTilt = 90f;
                */
                movement.currentTilt = 90;
                
                movement.isCrouching = false;
                isAlive = false;
                playerControl = false;
            }

        }
    }

    //test function
    public void RevivePlayer()
    {
        health = maxHealth;
        CheckHealth();

        movement.ChangePlayerSize(true);

        colorAlpha = 0f;

        deathCanvas.SetActive(false);
        /*
        rayDistance = standRayDistance;
        groundCheck = new Vector3(0, -.5f, 0);
        //transform.localScale = new Vector3(1, 1.5f, 1);
        transform.localScale = new Vector3(transform.localScale.x, characterScale, transform.localScale.z);
        //transform.position = transform.position + new Vector3(0, .75f, 0);
        transform.position = transform.position + new Vector3(0, height * .5f, 0);

        currentTilt = 0f;
        */
        movement.currentTilt = 0;
        isAlive = true;
        playerControl = true;
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
                //[SOUND] rage mode activation sound (One Shot)

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
            //[SOUND] rage mode sound (Continuous)

            rageModeActiveTime -= Time.deltaTime;
        }
    }

    public void GroundSlam()
    {
        //[SOUND] ground slam sound (One Shot)

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
            //[SOUND] damage aura sound (Continuous)

            //damageAuraModel.SetActive(true);

            if (movement.isCrouching)
            {
                //damageAuraModel.transform.localScale = new Vector3(damageAuraRange * 2, damageAuraRange * 4, damageAuraRange * 2);
            }
            else
            {
                //damageAuraModel.transform.localScale = new Vector3(damageAuraRange * 2, damageAuraRange * 2, damageAuraRange * 2);
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
            //[SOUND] damage aura deactivation sound (One Shot)

            //damageAuraModel.SetActive(false);
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
