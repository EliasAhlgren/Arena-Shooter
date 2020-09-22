using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterControllerRigidBody : MonoBehaviour
{
    public Rigidbody rb;
    public Transform playerCamera;
    public GameObject groundCheck;
    public Vector3 groundCheckSize;

    public bool isGrounded;


    public float mouseSensitivity = 100f;
    
    float xRotation = 0f;

    public float jumpForce = 10f;
    float speed = 6f;

    float x;
    float z;

    float DMLimiter;

    Vector3 move;
    Vector3 rotation;
    Vector3 velocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (isGrounded)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            if (Mathf.Sqrt(x * x + z * z) > 1)
            {
                DMLimiter = 0.7f;
            }
            else
            {
                DMLimiter = 1f;
            }
        }
        

        

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        rotation = (Vector3.up * mouseX);
        
        rb.rotation *= Quaternion.Euler(0, mouseX, 0);

        

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        move = transform.right * x * speed * DMLimiter + transform.forward * z * speed * DMLimiter + transform.up * rb.velocity.y;

        if (!isGrounded)
        {
            velocity.y += -20f * Time.deltaTime;
            move.y = velocity.y;
            move -= rotation;
        }
        else
        {
            velocity.y = 0f;
        }
        

        
        rb.velocity = move;
        isGrounded = Physics.OverlapBox(groundCheck.transform.position, groundCheckSize).Length > 1;

        debugAS();
    }

    void Jump()
    {
        rb.AddForce(new Vector3(0, jumpForce));
    }

    void debugAS()
    {
        Debug.DrawRay(transform.position, move, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }
}
