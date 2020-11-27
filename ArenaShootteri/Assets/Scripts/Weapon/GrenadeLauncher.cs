using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    public GameObject grenadePrefab;
    public float speeeed;
    public float damage;
    public Vector3 direction;
    public bool canFire = true;
    public int grenadesLeft = 6;
    
    private Transform _gunParent;
    // Start is called before the first frame update
    void Start()
    {
        _gunParent = GameObject.Find("GUN2 1").transform;
    }

    IEnumerator timeOut()
    {
        canFire = false;
        yield return new  WaitForSeconds(1f);
       //
       // Kranuheittimen lataus ääni tähän
       //
        canFire = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3") && canFire && grenadesLeft > 0)
        {
            //
            // Kranaatinheittimen ääni tähän
            //
            grenadesLeft--;
            var _grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            _grenade.GetComponent<Rigidbody>().AddRelativeForce(_gunParent.forward * speeeed, ForceMode.Impulse);
            StartCoroutine(timeOut());
        }
    }
}
