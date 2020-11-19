using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float verticalFrequency = 1f;
    public float vertical = 1f;
    public float rotation = 360f;

    public GameObject platform;
    protected PickupPlatform platformSript;

    protected GameObject player;
    protected Player playerScript;
    public Rigidbody rb;

    public float respawnTime = 5;

    Collider col;
    Vector3 position;


    void Start()
    {

        platformSript = platform.GetComponent<PickupPlatform>();

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.isKinematic = true;
        col.isTrigger = true;

        transform.position = platform.transform.position + Vector3.up;
        position = transform.position;
    }

    private void Update()
    {
        float verticalMovement = ((Mathf.Sin(Time.time * verticalFrequency) * 0.5f) + 0.5f) * vertical;
        transform.position = position + Vector3.up * verticalMovement;

        transform.Rotate(Vector3.up, rotation * Time.deltaTime, Space.Self);
    }
}
