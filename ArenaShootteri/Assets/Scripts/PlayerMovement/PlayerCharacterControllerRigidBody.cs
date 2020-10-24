using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterControllerRigidBody : MonoBehaviour
{
    //references to GameObjects
    public Rigidbody rb;
    public Transform playerCamera;
    public GameObject groundCheck;
    CapsuleCollider characterCollider;

    public float health = 100f;
    public float maxHealth = 100f;

    Image deathImage;
    public GameObject deathCanvas;

    float colorAlpha = 0f;
    public float minAlpha = 0f;
    public float maxAlpha = 1f;
    public float alphaLerp = 0f;

    //character height
    float height;

    Vector3 groundCheckSize = new Vector3(.3f, .7f, .3f);
    Vector3 wallCheckSize = new Vector3(.6f, .5f, .6f);

    public LayerMask groundLayerMask;

    //mouse input multiplier
    public float mouseSensitivity = 100f;

    //keyboard & mouse input variables;
    float x;
    float z;
    float mouseX;
    float mouseY;

    //mouse input variable holder
    float xRotation = 0f;

    //wallrun camera tilt
    float tiltAngle = 5;
    float cameraTilt = 0f;
    float currentTilt = 0f;
    float previousTilt = 0f;
    float tiltLerp = 0f;

    //diagonal movement limiter variable;
    float DMLimiter;

    //movement modifier variables
    float gravity;
    public float jumpHeight = 3f;
    public float airMoveModifier = .2f;

    //actual movement values
    float speed;
    public float runSpeed = 12f;
    public float walkSpeed = 6f;
    public float slideSpeedControl = 14f;
    public float slideDuration = 1f;
    float slideTime = 1f;
    float slideSpeed = 17f;
    float slideMovementSpeed = 6f;


    //character controller slidelimit variable holder
    public float slideLimit = 45.1f;

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
    public bool isSlidingControl;
    public bool isCrouching;
    public bool isRunning;
    public bool isWallRunning;
    public bool isDodging;
    public bool isTouchingWall;

    public bool playerControl = true;

    public bool isAlive = true;

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

    //normal wallrunning or walljumping
    public bool wallRunninType;

    //character movement vector
    Vector3 move;
    //gravity vector
    Vector3 velocity = new Vector3(0,0,0);

    //character airbone & slide vector holders
    Vector3 airBorne;
    Vector3 slide;
    Vector3 slideControl;
    Vector3 dodge;

    //jump groundcheck delay
    int jumpTimer = 0;

    //dodge duration in frames
    int dodgeFrameTime;

    //character movement vector when airbone
    Vector3 airMove;

    //character controller collider hit point
    Vector3 contactPoint;

    //variable for decouplin horizontal rotation from uncontrollable movement
    Vector3 rotation;

    void Start()
    {
        deathImage = deathCanvas.GetComponentInChildren<Image>();
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        //initialize variables
        characterCollider = GetComponent<CapsuleCollider>();
        height = characterCollider.height * transform.localScale.y;
        standRayDistance = height * .5f;
        crouchRayDistance = standRayDistance * .5f;
        crouchToStandRayDistance = standRayDistance + height * .25f;
        rayDistanceMargin = characterCollider.radius;
        gravity = Physics.gravity.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            health -= 50f;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (playerControl)
            {
                playerControl = false;
            }
            else
            {
                playerControl = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
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

        if (Input.GetKeyDown(KeyCode.K))
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

        if (playerControl)
        {

            //shoot
            if (Input.GetMouseButtonDown(0))
            {
                //trigger pull sound?
                Shoot();
            }

            //jump
            if (Input.GetButtonDown("Jump"))
            {
                //jump sound
                if (isGrounded)
                {
                    Jump();
                }
                else if (isWallRunning)
                {
                    WallJump();
                }
            }

            //crouch
            if (Input.GetKey(KeyCode.C))
            {
                Crouch();

            }
            else if (!isSlidingControl)
            {
                UnCrouch();
            }

            //dodge
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Dodge();
            }

            //running + prevent crouch running
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            {
                //set running speed for forward movement and multiply other direction movement by 25%
                isRunning = true;
                speed = runSpeed;
            }
            else
            {
                //set walking speed
                isRunning = false;
                speed = walkSpeed;
            }

            //mouse input
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            //keyboard input
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }
        else
        {
            mouseX = 0f;
            mouseY = 0f;

            x = 0f;
            z = 0f;
        }


        //set directional movement limiter
        if (Mathf.Sqrt(x * x + z * z) > 1)
        {
            DMLimiter = 0.7f;
        }
        else
        {
            DMLimiter = 1f;
        }

        //limit camera vertical movement
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (isAlive)
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

            alphaLerp += .3f * Time.deltaTime;

            colorAlpha = Mathf.Lerp(minAlpha, maxAlpha, alphaLerp);

            deathImage.color = new Color(deathImage.color.r, deathImage.color.r, deathImage.color.r, colorAlpha);
        }

        //camera vertical rotation
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, currentTilt);

        //playerCamera.transform.rotation = Quaternion.Euler(0f, 0f, -10f);

        //store horizontal rotation in 0 length vector
        rotation = (Vector3.up * mouseX);
        rotation *= 0f;

        //player horizontal rotation
        rb.rotation *= Quaternion.Euler(0, mouseX, 0);
        //playerBody.Rotate(Vector3.up * mouseX);

        //if character is touching ground
        if (isGrounded)
        {
            if (isWallRunning)
            {
                isWallRunning = false;
            }

            if (isAirborne || isSliding)
            {
                //landing sound

                //set airborne false
                isAirborne = false;
                //set sliding false
                isSliding = false;
            }


            //Debug.DrawRay(transform.position, -Vector3.up * (rayDistance + rayDistanceMargin + 2f), Color.red);
            //check if ground normal is over slide limit and set sliding true if it is
            RaycastHit hit;
            //raycast from center of character
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, (rayDistance + rayDistanceMargin + 2f), groundLayerMask))
            {
                //get angles of triangle from raycast to ground normal
                slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                slopeAngle2 = 90 - slopeAngle;

                //if standing on even ground
                if (slopeAngle == 0)
                {
                    evenGround = true;
                }
                //if slope angle too high slide
                else if (slopeAngle > slideLimit)
                {
                    evenGround = false;
                    isSliding = true;
                }
                //calculate slope angle to control vertical movement on slopes
                else
                {
                    Vector3 movedir = new Vector3(move.x, 0, move.z);
                    movedir = Vector3.Normalize(movedir);
                    slopeDir = new Vector3(hit.normal.x, 0, hit.normal.z);
                    slopeDir = Vector3.Normalize(slopeDir);

                    slopeDot = Vector3.Dot(movedir, slopeDir);
                    float vectorlen = Vector3.Magnitude(new Vector3(x, 0, z));
                    slopeSpeed = speed * DMLimiter * vectorlen * (Mathf.Sin((slopeAngle * Mathf.PI) / 180)) / (Mathf.Sin((slopeAngle2 * Mathf.PI) / 180));

                    evenGround = false;
                }

            }
            //if raycast failed due to high incline try again from contact point
            else
            {
                if (Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit, groundLayerMask))
                {
                    slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                    if (slopeAngle > slideLimit)
                    {
                        evenGround = false;
                        isSliding = true;
                    }
                }
            }


            //if ground normal is over slide limit calclulate slide vector from ground normal
            if (isSliding)
            {
                //sliding sound

                Vector3 hitNormal = hit.normal;
                slide = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref slide);
                //set slidespeed
                slide *= slideSpeed;

                //set antibump
                velocity.y = -slideSpeed;
                slide.y = velocity.y;

                //decouple character rotation from slide vector
                move = slide - rotation;

                //create normalized vector for moving sideways during slide and multiply it by player input
                Vector3 slidedir = new Vector3(slide.x, 0, slide.z);
                Vector3 movedir;
                movedir = transform.right * x * DMLimiter + transform.forward * z * DMLimiter + transform.up;
                slidedir = Quaternion.Euler(0, 90, 0) * slidedir;
                slidedir = Vector3.Normalize(slidedir);
                slidedir = Vector3.Scale(slidedir, -movedir);
                move += slidedir * slideMovementSpeed;

            }
            else if (isSlidingControl)
            {
                slideTime -= Time.deltaTime;

                move = slideControl - rotation;

                if (slideTime <= 0f)
                {
                    isSlidingControl = false;
                }
            }
            else if (isDodging)
            {
                dodgeFrameTime -= 1;
                move = dodge;
                if (dodgeFrameTime < 1)
                {
                    isDodging = false;
                }
            }
            else
            {
                if (isCrouching)
                {
                    //crouching sound
                }
                else if (isRunning)
                {
                    //running sound
                }
                else
                {
                    //walking sound
                }

                //if grounded and not sliding allow player free movement
                move = transform.right * x * speed * DMLimiter + transform.forward * z * speed * DMLimiter + transform.up * velocity.y;

                //set vertical movement depending on ground angle to prevent bumping
                if (evenGround)
                {
                    velocity.y = -2f;
                }
                else if (!isMoving)
                {
                    velocity.y = 0f;
                }
                else if (slopeDot < 0)
                {
                    velocity.y = slopeSpeed * slopeDot;
                    //velocity.y = 0f;
                }
                else if (slopeDot > 0)
                {
                    velocity.y = -slopeSpeed * slopeDot;
                }
            }
        }
        //!if is grounded
        else
        {
            if (isDodging)
            {
                isDodging = false;
                dodgeFrameTime = 0;
                //move *= .125f;
                Vector3 norMove = Vector3.Normalize(move);
                dodge = norMove * runSpeed;
                //move *= .5f;
            }

            isSliding = false;

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

                    if (wallRunninType)
                    {
                        airBorne = new Vector3(move.x, 0, move.z);
                        if (Physics.Raycast(transform.position + new Vector3(0, .2f, 0), transform.right, maxWallDistance, groundLayerMask))
                        {
                            velocity.y = 5f;
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

                    if (wallRunninType)
                    {
                        airBorne = new Vector3(move.x, 0, move.z);
                        if (Physics.Raycast(transform.position + new Vector3(0, .2f, 0), -transform.right, maxWallDistance, groundLayerMask))
                        {
                            velocity.y = 5f;
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
        }

        if (isAirborne)
        {

            if (isWallRunning)
            {
                if (!isTouchingWall)
                {
                    Debug.Log("start wall running");
                    isWallRunning = false;
                }

                //if wall is either left or right start wallrunning
                if (wallDirection >= 3 && z > 0 && isRunning && wallRunninType)
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
                            isWallRunning = false;
                        }

                    }
                    else if (wallDirection == 4)
                    {
                        Vector3 wallCheckDir = Quaternion.AngleAxis(-90, Vector3.up) * wallNormal;
                        if (Physics.Raycast(transform.position, wallCheckDir, out wallHit, maxWallDistance, groundLayerMask))
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
                            isWallRunning = false;
                        }

                    }
                    //wallNormal = Vector3.Cross(hit.normal, Vector3.up) * -wallDir;
                    airBorne = wallNormal * speed;

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

                //air sound

                //add gravity
                //velocity.y += gravity * Time.deltaTime;

                //airBorne.y = velocity.y;
                //decouple character rotation from airborne vector
                //Debug.DrawRay(transform.position, airBorne * 10f, Color.red);
                move = airBorne - rotation;

            }
            else
            {
                //air sound

                //add gravity
                velocity.y += gravity * Time.deltaTime;

                airBorne.y = velocity.y;
                //decouple character rotation from airborne vector
                move = airBorne - rotation;

                //airborne player movement
                airMove = transform.right * x * walkSpeed * DMLimiter * airMoveModifier + transform.forward * z * walkSpeed * DMLimiter * airMoveModifier + transform.up * 0;
                move += airMove;
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
            velocity.y = 0;
        }

        //apply vertical vector to movement vector
        move.y = velocity.y;

        //check if grounded and move character
        //Debug.DrawRay(transform.position, transform.forward * 10f, Color.green);
        //Debug.DrawRay(transform.position, move, Color.red);


        //ground check delay when jumping
        if (jumpTimer >= 1)
        {
            jumpTimer -= 1;
            //Debug.Log(jumpTimer);
        }
        else
        {
            jumpTimer = 0;
            isGrounded = Physics.OverlapBox(groundCheck.transform.position, groundCheckSize).Length > 1;
        }

        rb.velocity = move;

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
    }

    private void LateUpdate()
    {
        if (health <= 0)
        {
            if (isAlive)
            {
                killPlayer();
            }
        }
    }

    void Shoot()
    {
        //shooting sound
        int layerMask = LayerMask.GetMask("EnemyHitbox");
        RaycastHit hit;
        //raycast from center of screen if tagged enemy destroy target
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, layerMask))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.parent.transform.GetComponent<Grunt>().StartCoroutine("Die");

            }
            Debug.Log(hit.transform.name);
        }
    }

    private void Jump()
    {
        jumpTimer = 10;
        isSlidingControl = false;
        isAirborne = true;
        isGrounded = false;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //set airborne vector
        airBorne = move;
    }

    private void WallJump()
    {
        //float wallDot;
        //jump sound
        Vector3 lookDir = Vector3.Normalize(transform.forward);
        wallDot = Vector3.Dot(wallHit.normal, lookDir);



        //Debug.Log(wallDot);

        //wallrunning type 1 (wallrunning)
        if (wallRunninType)
        {
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
                airBorne = transform.forward * speed;
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
                    airBorne = jumpLimit * speed;
                }
                else if (wallDirection == 4)
                {
                    jumpLimit = Quaternion.AngleAxis(-81f, Vector3.up) * wallHit.normal;
                    airBorne = jumpLimit * speed;
                }
                else
                {
                    Debug.Log("jump failed");
                }

                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isWallRunning = false;
            }

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

        //airBorne = wallNormal * speed;
        //airBorne = new Vector3(wallHit.normal.x, 0, wallHit.normal.z);
        //Debug.DrawRay(wallHit.point, new Vector3(wallHit.normal.x, 0, wallHit.normal.z) * 20f);


    }

    private void Dodge()
    {
        isDodging = true;
        dodgeFrameTime = 20;
        velocity.y = 0f;
        //dodge = move * 4f *2f;
        Vector3 norMove = Vector3.Normalize(move);
        dodge = norMove * runSpeed * 2f;
    }

    private void Crouch()
    {
        if (isRunning && z > 0 && !isSlidingControl)
        {
            slideTime = slideDuration;
            isSlidingControl = true;
            slideControl = transform.forward * slideSpeedControl;
        }
        if (!isCrouching)
        {
            rayDistance = crouchRayDistance;
            transform.localScale = new Vector3(1, .75f, 1);
            transform.position = transform.position + new Vector3(0, -.75f, 0);
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
                rayDistance = standRayDistance;
                transform.localScale = new Vector3(1, 1.5f, 1);
                transform.position = transform.position + new Vector3(0, .75f, 0);
                isCrouching = false;
            }
        }
    }

    public void killPlayer()
    {
        alphaLerp = 0f;
        deathCanvas.SetActive(true);

        rayDistance = crouchRayDistance;
        transform.localScale = new Vector3(1, .75f, 1);
        //transform.position = transform.position + new Vector3(0, -.75f, 0);

        currentTilt = 90f;

        isCrouching = false;
        isAlive = false;
        playerControl = false;
    }

    private void RevivePlayer()
    {
        health = maxHealth;

        colorAlpha = minAlpha;

        deathCanvas.SetActive(false);

        rayDistance = standRayDistance;
        transform.localScale = new Vector3(1, 1.5f, 1);
        transform.position = transform.position + new Vector3(0, .75f, 0);

        currentTilt = 0f;

        isAlive = true;
        playerControl = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        contactPoint = collision.contacts[0].point;
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, wallCheckSize*2);
        //Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize *2);
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

}
