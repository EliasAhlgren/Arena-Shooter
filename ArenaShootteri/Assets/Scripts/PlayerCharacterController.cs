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

    public float speed = 12f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = gun.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(gun.transform.position, forward, Color.green);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 move = transform.right * x * speed
            + transform.forward * z * speed
            + transform.up * velocity.y;


        controller.Move(move * Time.deltaTime);
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

}
