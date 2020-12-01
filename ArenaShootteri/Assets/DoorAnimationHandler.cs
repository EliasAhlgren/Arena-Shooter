using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationHandler : MonoBehaviour
{
    public Animator anim;

    private Collider oviCollider;
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        var collider = GetComponents<Collider>();
        oviCollider = collider[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            anim.Play("OvenAvaus");
            SoundManager.PlaySound("GateOpen");
            oviCollider.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        { 
            anim.Play("OvenSulku");
            SoundManager.PlaySound("GateClose");
            oviCollider.enabled = true;
            GameManager.StartWave();
        }
    }
}
