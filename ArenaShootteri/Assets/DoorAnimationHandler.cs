using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationHandler : MonoBehaviour
{
    public Animator anim;
    public Light doorLight;
    public Color red1, green1;

    private Collider oviCollider;
    // Start is called before the first frame update
    void Start()
    {
        green1 = new Color(0.170218f, 0.6705883f, 0.1215686f);
        red1 = new Color(0.6698113f, 0.12322f, 0.12322f);
        anim = transform.parent.GetComponent<Animator>();
        var collider = GetComponents<Collider>();
        oviCollider = collider[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.waveEnd == true)
        {
            doorLight.color = green1;
        }
        else
        {
            doorLight.color = red1;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            GameObject.FindWithTag("GameManagement").GetComponent<NewMods>().CheckMods();
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
