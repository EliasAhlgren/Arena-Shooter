using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    public Text ammoText;
    public string ammoString;

    public GunAttributes gunAttributes;
    // Start is called before the first frame update
    void Start()
    {
        ammoText = gameObject.GetComponentInChildren<Text>();
        gunAttributes = FindObjectOfType<GunAttributes>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoString = (gunAttributes.ammoInMag + "/" + gunAttributes.totalAmmo);
        ammoText.text = ammoString;
    }
}
