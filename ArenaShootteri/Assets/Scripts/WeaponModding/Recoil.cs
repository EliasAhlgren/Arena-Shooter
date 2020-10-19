using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    public float baseRecoil;

    public float baseErgo;
    
    public float endRotX;

    public bool isRecoiling;
    
    [Range(0f,1f)]
    public float lerpPct;

    public float lerpSpeed;

    public float returnSpeed;

    public GunAttributes _GunAttributes;

    private Vector3 posDiff;
    
    public Transform parent;

    public float followLazyness;
    
    // Start is called before the first frame update
    void Start()
    {
        _GunAttributes = gameObject.GetComponent<GunAttributes>();

        posDiff = transform.position - parent.transform.position;

    }

    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        LazyGun();
        Recoiling();
        
    }

    public void LazyGun()
    {
        Vector3 newPos = parent.position + posDiff;
        Vector3 newRot = parent.localRotation.eulerAngles;

        transform.position = newPos;
        transform.eulerAngles = newRot;

        //transform.eulerAngles += (parent.transform.eulerAngles - transform.eulerAngles) * 0.2f;


    }
    
    private void Recoiling()
    {
        endRotX = baseRecoil + _GunAttributes.totalRecoil;

        returnSpeed = baseErgo + _GunAttributes.totalErgonomy;

        if (isRecoiling && lerpPct < 0.9f)
        {
            lerpPct += Time.deltaTime * lerpSpeed;
        }

        if (lerpPct >= 0.9f)
        {
            isRecoiling = false;
        }

        if (!isRecoiling && lerpPct > 0f)
        {
            lerpPct -= Time.deltaTime * returnSpeed;
        }

        Vector3 newRot = transform.localEulerAngles;

        newRot.x = -Mathf.Lerp(0, endRotX, lerpPct);

        transform.localEulerAngles = newRot;
    }
}
