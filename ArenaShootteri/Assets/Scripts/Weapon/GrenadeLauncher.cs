using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    public GameObject grenadePrefab;
    public float speeeed;
    public float damage;
    public Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            GameObject _grenade = Instantiate(grenadePrefab, transform.position, quaternion.identity);
            _grenade.GetComponent<Rigidbody>().AddRelativeForce(direction, ForceMode.Impulse);
        }
    }
}
