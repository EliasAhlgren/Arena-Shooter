using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationHandler : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        { 
            anim.Play("OvenSulku");
            var collider = GetComponents<Collider>();
            collider[1].enabled = true;
            GameManager.StartWave();
        }
    }
}
