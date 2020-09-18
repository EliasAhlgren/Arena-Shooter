using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerBody;
    public Transform playerCamera;
    public GameObject gun;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    public float runSpeed = 12f;
    public float walkSpeed = 6f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    float speed = 6f;
    float forwardSpeed;
    float rayDistance;
    float crouchRayDistance;
    float slideLimit;
    float slideSpeed = 17f;
    float slideMovementSpeed = 6f;

    Vector3 velocity;
    Vector3 contactPoint;
    public bool isGrounded;
    public bool isAirborne;
    public bool isSliding;
    public bool isCrouching;
    public bool isRunning;
    public bool playerControl = true;
    float x;
    float z;
    Vector3 move;
    Vector3 airMove;
    Vector3 jump;
    Vector3 slide;
    float DMLimiter;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rayDistance = controller.height * .5f + controller.radius;
        crouchRayDistance = controller.height * .5f + 2.26f;
        slideLimit = controller.slopeLimit + .1f;
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = 1.5f;
        }
        else
        {
            if (!Physics.Raycast(transform.position, Vector3.up, crouchRayDistance))
            {
                isCrouching = false;
                controller.height = 3f;
            }            
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (Mathf.Sqrt(x*x + z*z) > 1)
        {
            DMLimiter = 0.7f;
        }
        else
        {
            DMLimiter = 1f;
        }


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
        Vector3 rotation = (Vector3.up * mouseX);


        if (isGrounded)
        {
            isSliding = false;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance))
            {
                //Debug.Log(Vector3.Angle(hit.normal, Vector3.up));
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                {
                    isSliding = true;
                }
            }
            else
            {
                if (Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit))
                {
                    //Debug.Log(Vector3.Angle(hit.normal, Vector3.up));
                    if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    {
                        isSliding = true;
                    }
                }
            }

            if (isSliding)
            {
                Vector3 hitNormal = hit.normal;
                slide = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref slide);
                slide *= slideSpeed;
                playerControl = false;
            }
            else
            {
                playerControl = true;
            }


            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            {
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
                forwardSpeed = walkSpeed;
                speed = walkSpeed;
            }       

            if (velocity.y < 0)
            {
                velocity.y = -forwardSpeed;
            }

            if (Input.GetButtonDown("Jump") && playerControl)
            {
                isAirborne = true;
                isGrounded = false;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jump = transform.right * x * speed * DMLimiter + transform.forward * z * forwardSpeed * DMLimiter + transform.up;
            }

            
        }

        if (isSliding)
        {
            velocity.y = -slideSpeed;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }


        if (playerControl)
        {
            move = transform.right * x * speed * DMLimiter + transform.forward * z * forwardSpeed * DMLimiter + transform.up * velocity.y;
        } 
        
        

        if (isGrounded)
        {
            isAirborne = false;
            if (isSliding)
            {
                slide.y = velocity.y;
                move = slide - rotation;
                Vector3 slidedir = new Vector3(slide.x, 0, slide.z);
                Vector3 movedir;
                movedir = transform.right * x + transform.forward * z + transform.up;
                slidedir = Quaternion.Euler(0, 90, 0) * slidedir;
                slidedir = Vector3.Normalize(slidedir);
                slidedir = Vector3.Scale(slidedir, -movedir);                
                move += slidedir*slideMovementSpeed;
            }
        }
        else
        {
            isSliding = false;

            

            if (!isAirborne)
            {
                jump = transform.right * x * speed * DMLimiter + transform.forward * z * forwardSpeed * DMLimiter + transform.up;
                velocity.y = -2f;
                isAirborne = true;
            }

            jump.y = velocity.y;
            move = jump - rotation;
            airMove = transform.right * x * speed * DMLimiter * .2f + transform.forward * z * speed * DMLimiter * .2f + transform.up * 0f;
            move += airMove;
        }

        if (move.y > 0 && Physics.Raycast(transform.position, Vector3.up, rayDistance))
        {
            move.y = 0;
        }

        isGrounded = (controller.Move(move * Time.deltaTime) & CollisionFlags.Below) != 0;

        //debugAS();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contactPoint = hit.point;
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Destroy(hit.transform.gameObject);
            }
            Debug.Log(hit.transform.name);
        }
    }


    void debugAS()
    {
        Vector3 forward = gun.transform.TransformDirection(Vector3.forward) * 10;
        //Debug.Log(forward);
        Vector3 forwardplus = new Vector3(forward.x, 0f, forward.z);
        forwardplus = Quaternion.Euler(0, 90, 0) * forwardplus;
        forwardplus = Vector3.Normalize(forwardplus);
        Debug.DrawRay(gun.transform.position, forward, Color.green);
        Debug.DrawRay(gun.transform.position, forwardplus, Color.blue);
    }

}
