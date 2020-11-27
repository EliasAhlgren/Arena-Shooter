using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    //references to GameObjects
    PlayerCharacterControllerRigidBody player;
    public Rigidbody rb;
    public Transform playerCamera;
    public Transform cameraHolder;
    CapsuleCollider characterCollider;

    public float jumpHeight = 3f;
    public float airMoveModifier = .2f;


    //character height
    float height;
    float characterScale;

    Vector3 groundCheck = new Vector3(0, -1f, 0);
    Vector3 groundCheckSize = new Vector3(.3f, .6f, .3f);
    Vector3 wallCheckSize = new Vector3(.6f, .5f, .6f);

    LayerMask groundLayerMask;
    LayerMask wallLayerMask;

    //keyboard & mouse input variables;
    float x;
    float z;
    float rawX;
    float rawZ;
    float mouseX;
    float mouseY;

    //mouse input variable holder
    float xRotation = 0f;

    //wallrun camera tilt
    float tiltAngle = 5;
    float cameraTilt = 0f;
    public float currentTilt = 0f;
    float previousTilt = 0f;
    float tiltLerp = 0f;

    //diagonal movement limiter variable;
    //float DMLimiter;
    float movementAcc;

    //gravity
    float gravity;

    //actual movement values
    float speed;
    [Space(10)]
    //public float defaultSpeed;
    public float baseSpeed = 6f;
    [Space(10)]
    public float runSpeedMod = 2f;
    public float slideSpeedMod = 1.2f;
    public float dodgeSpeedMod = 3f;
    public float crouchSpeedMod = 0.7f;

    public float playerVelocity;
    //public float runSpeed = 12f;
    //public float walkSpeed = 6f;
    float dodgeSpeed;
    float dodgeCooldown;
    float slideSpeed = 14f;
    float slideDuration = 1f;

    //character size raycast length holders
    float rayDistance;
    float standRayDistance;
    float crouchRayDistance;
    float crouchToStandRayDistance;

    float rayDistanceMargin;

    //character state variables
    public bool isMoving;
    public bool isGrounded;
    public bool isAirborne;
    public bool isSliding;
    public bool isCrouching;
    public bool isRunning;
    public bool isWallRunning;
    public bool isDodging;
    public bool isTouchingWall;

    //variables for movement on slopes
    float slopeDot;
    float slopeAngle;
    float slopeAngle2;
    float slopeSpeed;
    bool evenGround;
    Vector3 slopeDir;

    //variables for movement on walls
    float maxWallDistance = 1.25f;
    RaycastHit wallHit;
    Vector3 wallNormal;
    int wallDirection;
    Collider[] wallCheck;
    float wallDot;

    //character movement vector
    Vector3 move;
    //gravity vector
    Vector3 velocity = new Vector3(0, 0, 0);

    //character airbone & slide vector holders
    Vector3 airBorne;
    Vector3 slide;
    Vector3 dodge;

    float deaccerelation;
    float stopDodgingSpeed;

    //jump groundcheck delay
    int jumpTimer = 0;
    int dodgeTimer = 0;

    //character movement vector when airbone
    Vector3 airMove;

    //character controller collider hit point
    List<Vector3> contactPoints;
    Vector3 contactPoint;

    //variable for decouplin horizontal rotation from uncontrollable movement
    public Vector3 rotation;

    //public float playerVelocity;
    //Vector3 lastPlayerPosition;
    //Vector3 playerPosition;
    //Vector3 movementVector;

    

    bool addingForce = false;
    bool doubleJump = false;

    //footsteps
    bool onCooldown = false;


    void Start()
    {
        //get components
        player = GetComponent<PlayerCharacterControllerRigidBody>();
        playerCamera = GetComponentInChildren<Camera>().transform;
        rb = GetComponent<Rigidbody>();
        characterCollider = GetComponent<CapsuleCollider>();
        groundLayerMask = LayerMask.GetMask("Map");

        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        //initialize variables
        //maxHealth = defaultMaxHealth * healthMod;
        //baseSpeed = defaultSpeed * speedMod;

        //playerPosition = transform.position;
        //lastPlayerPosition = playerPosition;
        height = characterCollider.height * transform.localScale.y;
        characterScale = transform.localScale.y;
        standRayDistance = height * .5f;
        crouchRayDistance = standRayDistance * .5f;
        crouchToStandRayDistance = standRayDistance + height * .25f;
        rayDistanceMargin = characterCollider.radius;
        gravity = Physics.gravity.y;

        PlayerVelocity();
    }

    void Update()
    {

        if (player.playerControl && Time.timeScale > 0)
        {
            //jump
            if (Input.GetButtonDown("Jump") && !isDodging)
            {
                if (isGrounded)
                {
                    Jump();
                }
                else if (isWallRunning && player.wallJumpUnlocked)
                {
                    WallJump();
                }
                else if (player.doubleJumpUnlocked)
                {
                    if (doubleJump)
                    {
                        Jump();
                        doubleJump = false;
                    }
                    else if (isSliding)
                    {
                        UnCrouch();
                    }
                }
                else if (isSliding)
                {
                    UnCrouch();
                }
            }

            //crouch
            if (Input.GetKey(KeyCode.C))
            {
                Crouch();

            }
            else if (!isSliding)
            {
                UnCrouch();
            }

            //dodge
            if (Input.GetKeyDown(KeyCode.Q) && isMoving && dodgeCooldown <= 0 && player.dodgeUnlocked && isGrounded)
            {
                //Debug.Log(playerVelocity);
                Dodge();
            }

            //running + prevent crouch running
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            {
                //set running speed for forward movement and multiply other direction movement by 25%
                isRunning = true;
                //speed = runSpeed;
                speed = baseSpeed * runSpeedMod;
            }
            else if (isCrouching)
            {
                //set crouching speed
                isRunning = false;

                speed = baseSpeed * crouchSpeedMod;
            }
            else
            {
                //set walking speed
                isRunning = false;
                //speed = walkSpeed;
                speed = baseSpeed;
            }

            //mouse input
            if (!player.rawMouseInput)
            {
                rb.angularDrag = 0.05f;
                mouseX = Input.GetAxis("Mouse X") * player.mouseSensitivity * Time.deltaTime;
                mouseY = Input.GetAxis("Mouse Y") * player.mouseSensitivity * Time.deltaTime;
            }
            else
            {
                rb.angularDrag = 0f;
                mouseX = Input.GetAxisRaw("Mouse X") * player.mouseSensitivity * Time.deltaTime;
                mouseY = Input.GetAxisRaw("Mouse Y") * player.mouseSensitivity * Time.deltaTime;
            }

            rawX = Input.GetAxisRaw("Horizontal");
            rawZ = Input.GetAxisRaw("Vertical");

            //keyboard input
            if (!player.rawMovementInput)
            {
                x = Input.GetAxis("Horizontal");
                z = Input.GetAxis("Vertical");
            }
            else
            {
                x = rawX;
                z = rawZ;
                //x = Input.GetAxisRaw("Horizontal");
                //z = Input.GetAxisRaw("Vertical");
            }
        }
        else
        {
            mouseX = 0f;
            mouseY = 0f;

            x = 0f;
            z = 0f;
        }



        //set directional movement limiter

        DirectioanalMovementLimit();

        /*
        if (Mathf.Sqrt(x * x + z * z) > 1)
        {
            DMLimiter = 0.7f;
        }
        else
        {
            DMLimiter = 1f;
        }
        */

        //limit camera vertical movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // camera tilting while wall running
        if (player.isAlive)
        {
            if (!isWallRunning)
            {
                cameraTilt = 0f;
            }

            if (currentTilt == cameraTilt)
            {
                previousTilt = currentTilt;
                tiltLerp = 0f;
            }
            else
            {
                tiltLerp += 5f * Time.deltaTime;
            }

            currentTilt = Mathf.Lerp(previousTilt, cameraTilt, tiltLerp);
        }
        else
        {
            if (player.colorAlpha <= 1)
            {
                player.colorAlpha += 0.5f * Time.deltaTime;

                player.deathImage.color = new Color(player.deathImage.color.r, player.deathImage.color.r, player.deathImage.color.r, player.colorAlpha);
            }
            //alphaLerp += .3f * Time.deltaTime;
            //colorAlpha = Mathf.Lerp(minAlpha, maxAlpha, alphaLerp);

        }

        //camera vertical rotation
        if (player.shakeRotation)
        {
            cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, currentTilt);
        }
        else
        {
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, currentTilt);
            //playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, -20);
            //playerCamera.transform.rotation = Quaternion.Euler(0f, 0f, -10f);
        }





        //store horizontal rotation in 0 length vector
        rotation = (Vector3.up * mouseX);
        rotation *= 0f;

        //player horizontal rotation
        rb.rotation *= Quaternion.Euler(0, mouseX, 0);
        //playerBody.Rotate(Vector3.up * mouseX);
    }

    private void FixedUpdate()
    {

        //if character is touching ground
        if (isGrounded)
        {

            if (!doubleJump)
            {
                doubleJump = true;
            }

            if (isWallRunning)
            {
                isWallRunning = false;
            }

            if (isAirborne || addingForce)
            {
                //[SOUND] landing sound (One Shot)

                if (player.groundSlamUnlocked)
                {
                    if (move.y < -13)
                    {
                        Debug.Log(move.y);
                        player.GroundSlam();
                    }
                }
                //set airborne false
                isAirborne = false;
                addingForce = false;
                //set sliding false
            }


            //Debug.DrawRay(transform.position, -Vector3.up * (rayDistance + rayDistanceMargin + 2f), Color.red);
            //check if ground normal is over slide limit and set sliding true if it is
            RaycastHit hit;

            //raycast from center of character
            if (Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit, groundLayerMask))
            //if (Physics.Raycast(transform.position, -Vector3.up, out hit, (rayDistance + rayDistanceMargin + 2f), groundLayerMask))
            {
                //Debug.Log(hit.normal);
                //get angles of triangle from raycast to ground normal
                slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                slopeAngle2 = 90 - slopeAngle;

                //if standing on even ground
                if (slopeAngle == 0)
                {
                    evenGround = true;
                }
                //calculate slope angle to control vertical movement on slopes
                else
                {
                    //Vector3 movedir = new Vector3(rawX, 0, rawZ);
                    Vector3 movedir = new Vector3(move.x, 0, move.z);

                    movedir = Vector3.ClampMagnitude(movedir, 1f);
                    //movedir = Vector3.Normalize(movedir);

                    slopeDir = new Vector3(hit.normal.x, 0, hit.normal.z);
                    slopeDir = Vector3.Normalize(slopeDir);

                    slopeDot = Vector3.Dot(movedir, slopeDir);
                    float vectorlen = Vector3.Magnitude(new Vector3(x, 0, z));
                    slopeSpeed = speed * movementAcc * vectorlen * (Mathf.Sin((slopeAngle * Mathf.PI) / 180)) / (Mathf.Sin((slopeAngle2 * Mathf.PI) / 180));
                    //slopeSpeed = speed * DMLimiter * vectorlen * (Mathf.Sin((slopeAngle * Mathf.PI) / 180)) / (Mathf.Sin((slopeAngle2 * Mathf.PI) / 180));
                    //Debug.Log(speed * movementAcc);
                    //Debug.Log(slopeSpeed);

                    evenGround = false;
                }

            }
            // if raycast failed from contactpoint try again directly downwards
            else if (Physics.Raycast(transform.position, -Vector3.up, out hit, (rayDistance + rayDistanceMargin + 2f), groundLayerMask))
            {
                //get angles of triangle from raycast to ground normal
                slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                slopeAngle2 = 90 - slopeAngle;

                //if standing on even ground
                if (slopeAngle == 0)
                {
                    evenGround = true;
                }
                //calculate slope angle to control vertical movement on slopes
                else
                {
                    //Vector3 movedir = new Vector3(rawX, 0, rawZ);
                    Vector3 movedir = new Vector3(move.x, 0, move.z);

                    movedir = Vector3.ClampMagnitude(movedir, 1f);
                    //movedir = Vector3.Normalize(movedir);

                    slopeDir = new Vector3(hit.normal.x, 0, hit.normal.z);
                    slopeDir = Vector3.Normalize(slopeDir);

                    slopeDot = Vector3.Dot(movedir, slopeDir);
                    float vectorlen = Vector3.Magnitude(new Vector3(x, 0, z));
                    slopeSpeed = speed * movementAcc * vectorlen * (Mathf.Sin((slopeAngle * Mathf.PI) / 180)) / (Mathf.Sin((slopeAngle2 * Mathf.PI) / 180));
                    //slopeSpeed = speed * DMLimiter * vectorlen * (Mathf.Sin((slopeAngle * Mathf.PI) / 180)) / (Mathf.Sin((slopeAngle2 * Mathf.PI) / 180));
                    //Debug.Log(speed * movementAcc);
                    //Debug.Log(slopeSpeed);

                    evenGround = false;
                }
            }
            //if raycast failed
            else
            {
                Debug.Log("ground raycast failed");
            }


            //if sliding
            if (isSliding)
            {
                //[SOUND] sliding sound (Continuous)

                //float deaccerelation = 14f - walkSpeed;
                float deaccerelation = (baseSpeed * runSpeedMod * slideSpeedMod) - baseSpeed;

                slideDuration -= Time.deltaTime * 4f;

                if (slideDuration <= 0)
                {
                    slideSpeed -= deaccerelation * Time.fixedDeltaTime;
                }

                //slideTime -= Time.deltaTime;

                if (slideSpeed > baseSpeed && playerVelocity > baseSpeed)
                {
                    move = (slide * slideSpeed) - rotation;
                }
                else
                {
                    isSliding = false;
                }

            }
            //if dodging
            else if (isDodging)
            {
                if (dodgeTimer > 0) dodgeTimer -= 1;

                dodgeSpeed -= deaccerelation * Time.deltaTime;

                move = dodge * dodgeSpeed - rotation;

                //move.y = velocity.y;
                velocity.y = 0;
                //rb.AddForce(move, ForceMode.VelocityChange);
                //PlayerVelocity();

                if (dodgeTimer == 0)
                {
                    if (dodgeSpeed <= stopDodgingSpeed || playerVelocity <= baseSpeed)
                    {
                        isDodging = false;
                        dodgeCooldown = .2f;
                        move = Vector3.ClampMagnitude(transform.right * x + transform.forward * z + transform.up * 0, 1f) * speed * movementAcc;
                    }  
                }

            }
            //if normal movement
            else
            {
                if (isMoving)
                {
                    if (isRunning)
                    {
                        if (!onCooldown)
                        {
                            SoundManager.PlaySound("WalkStep");
                            StartCoroutine(Cooldown(0.3f));
                        }
                    }
                    else if (isCrouching)
                    {
                        if (!onCooldown)
                        {
                            SoundManager.PlaySound("WalkStep");
                            StartCoroutine(Cooldown(0.6f));
                        }
                    }
                    else
                    {
                        if (!onCooldown)
                        {
                            SoundManager.PlaySound("WalkStep");
                            StartCoroutine(Cooldown(0.45f));
                        }
                    }
                }
                

                //if grounded and not sliding allow player free movement
                move = Vector3.ClampMagnitude(transform.right * x + transform.forward * z + transform.up * 0, 1f) * speed * movementAcc;

                //Debug.Log(speed * movementAcc);
                //move = transform.right * x * speed * DMLimiter + transform.forward * z * speed * DMLimiter + transform.up * velocity.y;

                
            }

            //set vertical movement depending on ground angle to prevent bumping
            if (isDodging)
            {
                velocity.y = 0f;
            }
            else if (evenGround)
            {
                velocity.y = -2f;
            }
            else if (!isMoving)
            {
                velocity.y = 0f;
            }
            else if (slopeDot < 0)
            {
                //velocity.y = (-slopeSpeed * slopeDot);
                velocity.y = 0f;
            }
            else if (slopeDot > 0)
            {
                velocity.y = (-slopeSpeed * slopeDot);
            }

        }
        //if is airborne
        else
        {

            //if falling from ledge
            if (!isAirborne)
            {
                Debug.Log("this shouldn't show if jumping");
                isAirborne = true;
                //make initial falling less floaty
                velocity.y = -2f;
                //set ariborne vector
                airBorne = move;
            }

            //if touching wall in air
            if (!isWallRunning && isTouchingWall && jumpTimer < 1)
            {
                //raycasts to detect wall direction when posible to start wallrunning
                //forward
                if (Physics.Raycast(transform.position, transform.forward, out wallHit, maxWallDistance, groundLayerMask))
                {
                    wallDirection = 1;
                    isWallRunning = true;
                    wallNormal = wallHit.normal;
                    //Debug.Log("wall forward");
                    //Debug.DrawRay(wallHit.point, wallHit.normal * 20f);
                }
                //backwards
                else if (Physics.Raycast(transform.position, -transform.forward, out wallHit, maxWallDistance, groundLayerMask))
                {
                    wallDirection = 2;
                    isWallRunning = true;
                    wallNormal = wallHit.normal;
                    //Debug.Log("wall behind");
                }
                //right
                else if (Physics.Raycast(transform.position, transform.right, out wallHit, maxWallDistance, groundLayerMask))
                {

                    if (player.wallRunUnlocked && isRunning)
                    {
                        airBorne = new Vector3(move.x, 0, move.z);
                        if (Physics.Raycast(transform.position + new Vector3(0, .2f, 0), transform.right, maxWallDistance, groundLayerMask))
                        {
                            velocity.y = 2f;
                        }
                        else
                        {
                            velocity.y = 0f;
                        }

                    }

                    wallDirection = 3;
                    isWallRunning = true;
                    wallNormal = wallHit.normal;
                    wallNormal = -Vector3.Cross(wallNormal, Vector3.up);
                    //Debug.Log("wall right");
                }
                //left
                else if (Physics.Raycast(transform.position, -transform.right, out wallHit, maxWallDistance, groundLayerMask))
                {

                    if (player.wallRunUnlocked && isRunning)
                    {
                        airBorne = new Vector3(move.x, 0, move.z);
                        if (Physics.Raycast(transform.position + new Vector3(0, .2f, 0), -transform.right, maxWallDistance, groundLayerMask))
                        {
                            velocity.y = 2f;
                        }
                        else
                        {
                            velocity.y = 0f;
                        }

                    }

                    wallDirection = 4;
                    isWallRunning = true;
                    wallNormal = wallHit.normal;
                    wallNormal = Vector3.Cross(wallNormal, Vector3.up);
                    //Debug.Log("wall left");
                }
                //move = new Vector3(0, 0, 0);
                //airBorne = move;
                //isWallRunning = true;
            }

            if (isDodging)
            {
                if (dodgeTimer > 0) dodgeTimer -= 1;

                dodgeSpeed -= deaccerelation * Time.deltaTime;

                airBorne = dodge * dodgeSpeed - rotation;

                //move.y = velocity.y;
                velocity.y = 0;
                //rb.AddForce(move, ForceMode.VelocityChange);
                //PlayerVelocity();

                if (dodgeTimer == 0)
                {
                    if (dodgeSpeed <= stopDodgingSpeed || playerVelocity <= baseSpeed)
                    {
                        isDodging = false;
                        dodgeCooldown = .2f;
                        airBorne = dodge * baseSpeed * runSpeedMod;
                        velocity.y = -2;
                    }                         
                }
                
            }
            //if wall running
            else if (isWallRunning)
            {
                if (!isTouchingWall)
                {
                    isWallRunning = false;
                }

                //if wall is either left or right start wallrunning
                if (wallDirection >= 3 && z > 0 && playerVelocity > 10 && player.wallRunUnlocked)
                {
                    //Debug.Log("wall running");
                    if (velocity.y >= 0.0f)
                    {
                        //airBorne = new Vector3(airBorne.x, airBorne.y, airBorne.z);
                        velocity.y -= 4.5f * Time.deltaTime;
                    }
                    //Debug.DrawRay(transform.position, transform.right * 10f, Color.red);
                    //Debug.DrawRay(transform.position, -transform.right * 10f, Color.red);
                    //Debug.Log(wallDirection);
                    if (wallDirection == 3)
                    {
                        Vector3 wallCheckDir = Quaternion.AngleAxis(90, Vector3.up) * wallNormal;
                        //Debug.DrawRay(transform.position, wallCheckDir * 10f, Color.red);
                        if (Physics.Raycast(transform.position, wallCheckDir, out wallHit, maxWallDistance, groundLayerMask))
                        {
                            //Vector3 wallNormalNormal = Vector3.Normalize(wallNormal);
                            Vector3 wallHitNormal = new Vector3(wallHit.normal.x, 0, wallHit.normal.z);

                            float changeInDir = Vector3.Dot(wallNormal, wallHitNormal);

                            changeInDir = Mathf.Round(changeInDir * 100f) / 100f;

                            if (changeInDir <= 0)
                            {
                                if (!Physics.Raycast(transform.position + new Vector3(0, .2f, 0), transform.right, maxWallDistance, groundLayerMask))
                                {
                                    velocity.y = 0f;
                                }

                                //Debug.DrawRay(wallHit.point, wallHit.normal * 10f, Color.red);
                                wallNormal = wallHit.normal;
                                wallNormal = -Vector3.Cross(wallNormal, Vector3.up);
                                cameraTilt = tiltAngle;
                            }
                            else
                            {
                                //testFloat = bestea;
                                //Physics.Raycast(transform.position, wallCheckDir, out testHit, maxWallDistance, groundLayerMask);
                                jumpTimer = 10;
                                isWallRunning = false;
                            }

                        }
                        else
                        {
                            isWallRunning = false;
                        }

                    }
                    else if (wallDirection == 4)
                    {
                        Vector3 wallCheckDir = Quaternion.AngleAxis(-90, Vector3.up) * wallNormal;
                        if (Physics.Raycast(transform.position, wallCheckDir, out wallHit, maxWallDistance, groundLayerMask))
                        {
                            //Vector3 wallNormalNormal = Vector3.Normalize(wallNormal);
                            Vector3 wallHitNormal = new Vector3(wallHit.normal.x, 0, wallHit.normal.z);

                            float changeInDir = Vector3.Dot(wallNormal, wallHitNormal);

                            changeInDir = Mathf.Round(changeInDir * 100f) / 100f;

                            if (changeInDir <= 0)
                            {
                                if (!Physics.Raycast(transform.position + new Vector3(0, .2f, 0), -transform.right, maxWallDistance, groundLayerMask))
                                {
                                    velocity.y = 0f;
                                }

                                //Debug.DrawRay(wallHit.point, wallHit.normal * 10f, Color.red);
                                wallNormal = wallHit.normal;
                                wallNormal = Vector3.Cross(wallNormal, Vector3.up);
                                cameraTilt = -tiltAngle;
                            }
                            else
                            {
                                //testFloat = bestea;
                                //Physics.Raycast(transform.position, wallCheckDir, out testHit, maxWallDistance, groundLayerMask);
                                jumpTimer = 10;
                                isWallRunning = false;
                            }

                        }
                        else
                        {
                            isWallRunning = false;
                        }

                    }
                    //wallNormal = Vector3.Cross(hit.normal, Vector3.up) * -wallDir;
                    airBorne = wallNormal * speed;

                    //[SOUND] Wallrunning sound (Continuous)

                }
                else
                {
                    velocity.y += gravity * Time.deltaTime;

                    airBorne.y = velocity.y;
                }

                //move = transform.right * x * speed * DMLimiter + transform.forward * z * speed * DMLimiter + transform.up * velocity.y;
                //velocity.y = -2f;
                //playerControl = true;
                //move = new Vector3(0, 0, 0);
                //velocity.y = 0f;

                //add gravity
                //velocity.y += gravity * Time.deltaTime;

                //airBorne.y = velocity.y;
                //decouple character rotation from airborne vector
                //Debug.DrawRay(transform.position, airBorne * 10f, Color.red);
                move = airBorne - rotation;

            }
            //if force is being added
            else if (addingForce)
            {
                //[SOUND] air sound (Continuous)

                velocity.y += gravity * Time.deltaTime;

                airBorne.y = velocity.y;
                //decouple character rotation from airborne vector
                move = airBorne - rotation;
            }
            //if in air
            else
            {
                //[SOUND] air sound (Continuous)

                //add gravity
                velocity.y += gravity * Time.deltaTime;

                airBorne.y = velocity.y;

                
                airMove = Vector3.ClampMagnitude(transform.right * x + transform.forward * z + transform.up * 0, 1f) * baseSpeed * airMoveModifier * movementAcc;
                //airBorne += airMove;
                //decouple character rotation from airborne vector
                move = (airBorne - rotation) + airMove;

                //airborne player movement
                //airMove = transform.right * x * walkSpeed * DMLimiter * airMoveModifier + transform.forward * z * walkSpeed * DMLimiter * airMoveModifier + transform.up * 0;
                //airMove = transform.right * x * baseSpeed * DMLimiter * airMoveModifier + transform.forward * z * baseSpeed * DMLimiter * airMoveModifier + transform.up * 0;
                //airMove = Vector3.Normalize(transform.right * x + transform.forward * z + transform.up * 0) * speed * airMoveModifier * DirectioanlMovementLimit();
                //move += airMove;
            }
        }

        //if moving vertically
        if (move.x != 0f || move.z != 0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }


        //check if character hit roof and reset upward movement
        if (move.y > 0 && Physics.Raycast(transform.position, Vector3.up, rayDistance))
        {
            //[SOUND] hit roof sound (One Shot)

            velocity.y = 0;
        }

        //apply vertical vector to movement vector
        move.y = velocity.y;

        //check if grounded and move character

        //ground check delay when jumping
        if (jumpTimer >= 1)
        {
            jumpTimer -= 1;
            //jumpTimer -= 1;
            //Debug.Log(jumpTimer);
        }
        else
        {
            jumpTimer = 0;
            isGrounded = Physics.OverlapBox(transform.position + groundCheck, groundCheckSize, Quaternion.identity, groundLayerMask).Length > 0;
        }

        //Vector3 forceVector = rb.AddForce(transform.forward * 200);
        //rb.velocity = move;
        rb.AddForce(move, ForceMode.VelocityChange);

        //check if character is trouching wall
        wallCheck = Physics.OverlapBox(transform.position, wallCheckSize, Quaternion.identity, groundLayerMask);


        if (wallCheck.Length > 0)
        {
            isTouchingWall = true;

            //calculate wall direction
            /**
            for (int i = 1; i < wallCheck.Length; i++)
            {
                Vector3 wall;
                Vector3 dirWall;

                wall = wallCheck[i].gameObject.transform.position;

                dirWall = transform.position - wall;
                dirWall = new Vector3(dirWall.x, 0, dirWall.z);

                Physics.Raycast(transform.position, dirWall, out wallHit, maxWallDistance, groundLayerMask);

                Vector3 lookDir = Vector3.Normalize(transform.forward);
                wallDot = Vector3.Dot(wallHit.normal, lookDir);

                Debug.Log(wallDot);
            }
            **/
        }
        else
        {
            isTouchingWall = false;
        }
        //isTouchingWall = Physics.OverlapBox(transform.position, wallCheckSize, Quaternion.identity, groundLayerMask).Length > 0;


        //isGrounded = (controller.Move(move * Time.deltaTime) & CollisionFlags.Below) != 0;

        //movement vector debug
        //debugAS();
        if (dodgeCooldown > 0)
        {
            dodgeCooldown -= Time.fixedDeltaTime;

            if(dodgeCooldown < 0)
            {
                dodgeCooldown = 0;
            }
        }
        

        PlayerVelocity();
    }

    public void PlayerAddForce(Vector3 dir, float force)
    {
        jumpTimer = 10;
        isSliding = false;
        isAirborne = true;
        isGrounded = false;
        addingForce = true;

        Vector3 dar = dir * force;
        if (dar.y < 5) dar.y = 5;
        velocity.y = dar.y;

        airBorne = new Vector3(dar.x, 0, dar.z);
    }

    private void DirectioanalMovementLimit()
    {
        movementAcc = Mathf.Clamp(Mathf.Abs(x) + Mathf.Abs(z), 0, 1);
        //return 
    }

    public void PlayerVelocity()
    {
        /*
        playerPosition = transform.position;

        movementVector = new Vector3(playerPosition.x, 0, playerPosition.z) - new Vector3(lastPlayerPosition.x, 0, lastPlayerPosition.z);

        playerVelocity = Vector3.Magnitude(movementVector);
        playerVelocity *= 50f;
        lastPlayerPosition = playerPosition;

        if (playerVelocity > 16) Debug.Log(playerVelocity);

        float rbVelocity = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
        Debug.Log(rbVelocity);
        */
        playerVelocity = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
    }

    private void Jump()
    {
        //[SOUND] jump sound (One Shot)

        jumpTimer = 10;
        isSliding = false;
        isAirborne = true;
        isGrounded = false;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //set airborne vector
        if (isGrounded)
        {
            airBorne = Vector3.Normalize(transform.right * x + transform.forward * z + transform.up * 0) * playerVelocity * movementAcc;
        }
        else
        {
            if (rawX == 0 && rawZ == 0)
            {
                airBorne = move;
            }
            else
            {
                if (playerVelocity > baseSpeed * runSpeedMod * slideSpeedMod)
                {
                    airBorne = Vector3.Normalize(transform.right * rawX + transform.forward * rawZ + transform.up * 0) * baseSpeed * runSpeedMod * slideSpeedMod;
                }
                else
                {
                    airBorne = Vector3.Normalize(transform.right * rawX + transform.forward * rawZ + transform.up * 0) * playerVelocity;
                }
                
            }
        }

        //airBorne = move;
    }

    private void WallJump()
    {
        //[SOUND] jump sound (walljump) (One Shot)

        //float wallDot;
        Vector3 lookDir = Vector3.Normalize(transform.forward);
        wallDot = Vector3.Dot(wallHit.normal, lookDir);

        if (wallDot > .1f)
        {
            jumpTimer = 10;
            //isAirborne = true;
            //isGrounded = false;

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //set airborne vector
            //airBorne = move;

            //Debug.Log("wall jump" + Vector3.Magnitude(transform.forward));

            //if looking away from wall jump forward
            //airBorne = transform.forward * speed;
            airBorne = transform.forward * playerVelocity;
            isWallRunning = false;

            //old version using the walldirection variable
            //if (wallDirection == 5)
            //{
            //    airBorne = -transform.forward * speed;
            //}
            //else
            //{
            //    airBorne = transform.forward * speed;
            //}
        }
        //if looking torwards wall jump behind
        else if (wallDot < -.1f)
        {
            jumpTimer = 10;
            //Debug.Log("wall jump backwards");

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            airBorne = -transform.forward * speed;
            //airBorne = -transform.forward * PlayerVelocity();
            isWallRunning = false;
        }
        else
        {
            jumpTimer = 10;
            Vector3 jumpLimit;
            //Debug.Log("low angle jump");

            if (wallDirection == 3)
            {
                jumpLimit = Quaternion.AngleAxis(81, Vector3.up) * wallHit.normal;
                //airBorne = jumpLimit * speed;
                airBorne = jumpLimit * playerVelocity;
            }
            else if (wallDirection == 4)
            {
                jumpLimit = Quaternion.AngleAxis(-81f, Vector3.up) * wallHit.normal;
                //airBorne = jumpLimit * speed;
                airBorne = jumpLimit * playerVelocity;
            }
            else
            {
                Debug.Log("jump failed");
            }

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isWallRunning = false;
        }

        /**
        //Debug.Log(wallDot);

        //wallrunning type 1 (wallrunning)
        if (wallRunninType)
        {
            

        }
        //wallrunning type 2 (walljumping)
        else
        {
            //jump backwards if facing at wall
            if (Physics.Raycast(transform.position, transform.forward, out wallHit, maxWallDistance, groundLayerMask))
            {
                airBorne = -transform.forward * speed;
                isWallRunning = false;
            }
            // else jump forward
            else
            {
                airBorne = transform.forward * speed;
                isWallRunning = false;
            }

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        **/
        //airBorne = wallNormal * speed;
        //airBorne = new Vector3(wallHit.normal.x, 0, wallHit.normal.z);
        //Debug.DrawRay(wallHit.point, new Vector3(wallHit.normal.x, 0, wallHit.normal.z) * 20f);
    }

    private void Dodge()
    {
        //[SOUND] dodging sound (jumpsound?) (One Shot)

        isDodging = true;
        dodgeTimer = 3;
        dodgeSpeed = baseSpeed * dodgeSpeedMod;

        velocity.y = 0f;

        transform.position += new Vector3(0, .1f, 0);

        deaccerelation = ((baseSpeed * dodgeSpeedMod) - (baseSpeed * runSpeedMod)) * 8f;

        stopDodgingSpeed = baseSpeed;

        dodge = Vector3.Normalize(new Vector3(move.x, 0, move.z));
        move = dodge * dodgeSpeed - rotation;

        move.y = velocity.y;

        rb.AddForce(move, ForceMode.VelocityChange);
    }

    private void Slide()
    {
        slideDuration = 1f;
        isSliding = true;
        //slideSpeedControl = 14f;
        slideSpeed = baseSpeed * runSpeedMod * slideSpeedMod;

        if (slideSpeed < playerVelocity) slideSpeed = playerVelocity;
        //slide = transform.forward;
        slide = Vector3.Normalize(new Vector3(move.x, 0, move.z));
        //slideControl = transform.forward * slideSpeedControl;
    }

    private void Crouch()
    {
        if (isRunning && z > 0 && !isSliding && player.slideUnlocked)
        {
            Slide();
        }
        if (!isCrouching)
        {
            ChangePlayerSize(false);
            /*
            rayDistance = crouchRayDistance;
            groundCheck = new Vector3(0, -.25f, 0);
            //transform.localScale = new Vector3(1, .75f, 1);
            transform.localScale = new Vector3(transform.localScale.x, characterScale * 0.5f, transform.localScale.z);
            */
            //transform.position = transform.position + new Vector3(0, -.75f, 0);
            if (isGrounded)
            {
                transform.position = transform.position + new Vector3(0, -height * .25f, 0);
            }

            //[SOUND] start crounching sound???? (One Shot)

            isCrouching = true;
        }
    }

    private void UnCrouch()
    {
        //prevent from standing up if obstacle detected
        if (!Physics.Raycast(transform.position, Vector3.up, crouchToStandRayDistance))
        {
            if (isCrouching)
            {
                ChangePlayerSize(true);
                /*
                rayDistance = standRayDistance;
                groundCheck = new Vector3(0, -.5f, 0);
                //transform.localScale = new Vector3(1, 1.5f, 1);
                transform.localScale = new Vector3(transform.localScale.x, characterScale, transform.localScale.z);
                //transform.position = transform.position + new Vector3(0, .75f, 0);
                */
                if (isGrounded)
                {
                    transform.position = transform.position + new Vector3(0, height * .25f, 0);
                }
                else
                {
                    if (Physics.Raycast(transform.position, -Vector3.up, rayDistance))
                    {
                        transform.position = transform.position + new Vector3(0, height * .25f, 0);
                    }

                }

                //[SOUND] stand up sound???? (One Shot)

                isSliding = false;
                isCrouching = false;
            }
        }
    }

    public void ChangePlayerSize(bool standing)
    {
        if (standing)
        {
            rayDistance = standRayDistance;
            groundCheck = new Vector3(0, -1, 0);
            characterCollider.height = 3f;
        }
        else
        {
            rayDistance = crouchRayDistance;
            groundCheck = new Vector3(0, -0.5f, 0);
            characterCollider.height = 1.5f;
        }
    }

    /*
    public void KillPlayer()
    {
        if (player.isAlive)
        {
            if (player.resurectionUnlocked && resurection)
            {
                player.health = 10f;
                resurection = false;
            }
            else
            {
                //alphaLerp = 0f;
                deathCanvas.SetActive(true);
                colorAlpha = 0.3f;

                rayDistance = crouchRayDistance;
                groundCheck = new Vector3(0, -.25f, 0);
                //transform.localScale = new Vector3(1, .75f, 1);
                transform.localScale = new Vector3(transform.localScale.x, characterScale * 0.5f, transform.localScale.z);
                //transform.position = transform.position + new Vector3(0, -.75f, 0);

                currentTilt = 90f;

                isCrouching = false;
                player.isAlive = false;
                player.playerControl = false;
            }

        }
    }

    public void RevivePlayer()
    {
        player.health = player.maxHealth;

        colorAlpha = 0f;

        deathCanvas.SetActive(false);

        rayDistance = standRayDistance;
        groundCheck = new Vector3(0, -.5f, 0);
        //transform.localScale = new Vector3(1, 1.5f, 1);
        transform.localScale = new Vector3(transform.localScale.x, characterScale, transform.localScale.z);
        //transform.position = transform.position + new Vector3(0, .75f, 0);
        transform.position = transform.position + new Vector3(0, height * .5f, 0);

        currentTilt = 0f;

        player.isAlive = true;
        player.playerControl = true;
    }
    */

    private void OnCollisionStay(Collision collision)
    {
        //contactPoint = collision.contacts[0].point;
        //contactPoints.Clear();
        //for (int i = 0; i < collision.contacts.Length; i++)
        //{
        //   contactPoints.Add(collision.contacts[i].point);
        //}

        foreach (ContactPoint p in collision.contacts)
        {
            Vector3 bottom = characterCollider.bounds.center - (Vector3.up * characterCollider.bounds.extents.y);
            Vector3 curve = bottom + (Vector3.up * characterCollider.radius);

            Vector3 testVector = p.point - curve;
            //Debug.DrawLine(curve, p.point, Color.blue, 0.5f);

            Debug.DrawRay(curve, testVector, Color.red);
            Vector3 dir = curve - p.point;

            if (dir.y > 0f)
            {
                contactPoint = p.point;
                return;
            }



        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, wallCheckSize * 2);
        if (isCrouching)
        {
            Gizmos.DrawWireCube(transform.position + groundCheck, new Vector3(groundCheckSize.x, groundCheckSize.y * .5f, groundCheckSize.z) * 2);
        }
        else
        {
            Gizmos.DrawWireCube(transform.position + groundCheck, groundCheckSize * 2);
        }

    }
    //debug function
    void debugAS()
    {
        //Debug.DrawRay(transform.position, move, Color.green);
        //Debug.DrawRay(transform.position, slide, Color.red);
        //Vector3 forward = gun.transform.TransformDirection(Vector3.forward) * 10;
        //Debug.Log(forward);
        //Vector3 forwardplus = new Vector3(forward.x, 0f, forward.z);
        //forwardplus = Quaternion.Euler(0, 90, 0) * forwardplus;
        //forwardplus = Vector3.Normalize(forwardplus);
        //Debug.DrawRay(gun.transform.position, forward, Color.green);
        //Debug.DrawRay(gun.transform.position, forwardplus, Color.blue);
    }

    private IEnumerator Cooldown(float time)
    {
        // Start cooldown
        onCooldown = true;
        // Wait for time you want
        yield return new WaitForSeconds(time);
        // Stop cooldown
        onCooldown = false;
        //Debug.Log("Cooldown Ended");
        //GameManager.waveStart = true;
        //GameManager.waveEnd = false;
    }

}
