using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    //references to GameObjects
    public CharacterController controller;
    public Transform playerBody;
    public Transform playerCamera;
    public GameObject gun;

    //mouse input multiplier
    public float mouseSensitivity = 100f;

    //keyboard & mouse input variables;
    float x;
    float z;
    float mouseX;
    float mouseY;

    //mouse input variable holder      
    float xRotation = 0f;

    //diagonal movement limiter variable;
    float DMLimiter;

    //movement modifier variables
    public float runSpeed = 12f;
    public float walkSpeed = 6f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float airMoveModifier = .2f;

    //actual movement values
    float speed = 6f;
    float forwardSpeed;
    float slideSpeed = 17f;
    float slideMovementSpeed = 6f;

    //character controller slidelimit variable holder
    float slideLimit;

    //character size raycast length holders
    float rayDistance;
    float crouchRayDistance;

    //character state variables
    public bool isMoving;
    public bool isGrounded;
    public bool isAirborne;
    public bool isSliding;
    public bool isCrouching;
    public bool isRunning;
    public bool playerControl = true;

    //character movement vector
    Vector3 move;
    //gravity vector
    Vector3 velocity;

    //character airbone & slide vector holders
    Vector3 airBorne;
    Vector3 slide;

    //character movement vector when airbone
    Vector3 airMove;

    //character controller collider hit point
    Vector3 contactPoint;

    //variable for decouplin horizontal rotation from uncontrollable movement
    Vector3 rotation;


    void Start()
    {
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        //initialize variables
        rayDistance = controller.height * .5f + controller.radius;
        crouchRayDistance = controller.height * .5f + 2.26f;
        slideLimit = controller.slopeLimit + .1f;
    }

    void FixedUpdate()
    {
        //shoot
        if (Input.GetMouseButtonDown(0))
        {
            //trigger pull sound?
            Shoot();
        }

        //crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = 1.5f;
        }
        else
        {
            //prevent from standing up if obstacle detected
            if (!Physics.Raycast(transform.position, Vector3.up, crouchRayDistance))
            {
                isCrouching = false;
                controller.height = 3f;
            }
        }

        //running + prevent crouch running
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            //set running speed for forward movement and multiply other direction movement by 25%
            isRunning = true;
            speed = runSpeed * 0.75f;

            if (z > 0)
            {
                forwardSpeed = runSpeed;
            }
            else
            {
                forwardSpeed = runSpeed * 0.75f;
            }
        }
        else
        {
            isRunning = false;
            forwardSpeed = walkSpeed;
            speed = walkSpeed;
        }

        //mouse input
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //keyboard input
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

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

        //camera vertical rotation
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //player horizontal rotation
        playerBody.Rotate(Vector3.up * mouseX);
        
        //store horizontal rotation in 0 length vector
        rotation = (Vector3.up * mouseX);
        rotation = Vector3.Normalize(rotation);
        rotation *= 0f;

        //if character is touching ground
        if (isGrounded)
        {
            if (isAirborne || isSliding)
            {
                //landing sound

                //set airborne false
                isAirborne = false;
                //set sliding false
                isSliding = false;
            }
                
            

            //check if ground normal is over slide limit and set sliding true if it is 
            RaycastHit hit;
            //raycast from center of character
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance))
            {

                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                {
                    isSliding = true;
                }
            }
            //if raycast failed due to high incline try again from contact point
            else
            {
                if (Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit))
                {
                    if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    {
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
                velocity.y = -slideSpeed * 10f;
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

                //prevent jumping from slide
                playerControl = false;
            }
            else
            {
                if (isCrouching)
                {
                    //crouching sound
                }
                if (isRunning)
                {
                    //running sound
                }
                else
                {
                    //walking sound
                }
                //if grounded and not sliding allow player free movement
                move = transform.right * x * speed * DMLimiter + transform.forward * z * forwardSpeed * DMLimiter + transform.up * velocity.y;

                //allow jumping
                playerControl = true;
            }

            //anti bump
            if (velocity.y < 0)
            {
                velocity.y = -forwardSpeed;
                //velocity.y = -2f;
            }

            //jump
            if (Input.GetButtonDown("Jump") && playerControl)
            {
                //jump sound

                isAirborne = true;
                isGrounded = false;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                //set airborne vector
                airBorne = move;
            }
        }
        //!if is grounded 
        else
        {
            isSliding = false;

            //if falling from ledge
            if (!isAirborne)
            {
                isAirborne = true;
                //make initial falling less floaty
                velocity.y = -2f;
                //set ariborne vector
                airBorne = move;
            }            
        }

        if (isAirborne)
        {
            //air sound

            airBorne.y = velocity.y;
            //decouple character rotation from airborne vector
            move = airBorne - rotation;

            //airborne player movement
            airMove = transform.right * x * speed * DMLimiter * airMoveModifier + transform.forward * z * speed * DMLimiter * airMoveModifier + transform.up * 0;
            move += airMove;
        }

        //check if character hit roof and reset upward movement
        if (move.y > 0 && Physics.Raycast(transform.position, Vector3.up, rayDistance))
        {
            move.y = 0;
        }

        //add gravity
        velocity.y += gravity * Time.deltaTime;

        if (move.x > 0f || move.z > 0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        //check if grounded and move character
        isGrounded = (controller.Move(move * Time.deltaTime) & CollisionFlags.Below) != 0;

        //movement vector debug
        //debugAS();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contactPoint = hit.point;
    }

    //shoot function
    void Shoot()
    {
        //shooting sound

        RaycastHit hit;
        //raycast from center of screen if tagged enemy destroy target
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Destroy(hit.transform.gameObject);
            }
            Debug.Log(hit.transform.name);
        }
    }

    //debug function
    void debugAS()
    {
        Debug.DrawRay(transform.position, move, Color.green);
        Debug.DrawRay(transform.position, slide, Color.red);
        //Vector3 forward = gun.transform.TransformDirection(Vector3.forward) * 10;
        //Debug.Log(forward);
        //Vector3 forwardplus = new Vector3(forward.x, 0f, forward.z);
        //forwardplus = Quaternion.Euler(0, 90, 0) * forwardplus;
        //forwardplus = Vector3.Normalize(forwardplus);
        //Debug.DrawRay(gun.transform.position, forward, Color.green);
        //Debug.DrawRay(gun.transform.position, forwardplus, Color.blue);
    }

}
