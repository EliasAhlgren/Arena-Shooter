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

    [Tooltip("Korkeammalla numerolla aseella kestää pidempään tulla perus asentoon")]
    public float followLazyness;

    public float rotationLazyness;

    public Transform lookatPoint;

    public GameObject debugCube;

    private Vector3 newpoint;
    // Start is called before the first frame update
    void Start()
    {
        newpoint = lookatPoint.position;
        
        _GunAttributes = gameObject.GetComponent<GunAttributes>();

        posDiff = transform.position - parent.transform.position;


        transform.position = parent.position;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        LazyGun();
        //Recoiling();
        
    }

    public void LazyGun()
    {
        if (!_GunAttributes.isModding && !_GunAttributes.isAiming)
        {
            Debug.Log("Vibin");
            
            //transform.position = (followLazyness * transform.position) + ((1f - followLazyness) * parent.position);
                transform.position = parent.position;
            //transform.position += (parent.position - transform.position) * followLazyness;

            var position = lookatPoint.position;
            newpoint += (position - newpoint) * rotationLazyness;

            newpoint.x = Mathf.Clamp(newpoint.x, position.x - 10f, position.x + 10f);
            
            debugCube.transform.position = newpoint;
            
            transform.LookAt(newpoint);
        }

        


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
