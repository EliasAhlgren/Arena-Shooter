using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;


public class Recoil : MonoBehaviour
{
    public bool isRecoiling;
    
    public float recoilTime;

    public float RateOfFire = 1f;

    public float RecoilAmount = 1f;

    public float aimingRecoil;

    public float _recoil;
    
    public float horizontalRecoil;
    
    // How much the head bobs when firing
    public float FeltRecoil = 1f;
    
    public GunAttributes _GunAttributes;
    
    // Gun position target
    public Transform target;

    // how fast the gun rotates to target rotation
    public float rotationLazyness;

    //temporary value
    private float _rotationLazyness;
    
    // how fast the recoil decays
    public float ReturnTime;
    
    // Aiming point
    public Transform lookatPoint;

    public GameObject debugCube;

    private Vector3 newpoint;

    public Transform cameraTransform;

    public float timer1;

    public float timer2;

    [NonSerialized] public Vector3 defaultPos;

    public bool showRecoil;

    public bool DisableLazyGun;

    public bool isAiming;

    private bool _canFire = true;

    public Shooting shooting;
    public VisualEffect muzzleFlash;

    private bool onCooldown;

    public bool increasedRof;

    public float RateOfFire2;

    // increasedRof ja rateOfFire2 on niitä perkkejä varten
    
    // Start is called before the first frame update
    void Start()
    {

        defaultPos = lookatPoint.localPosition;
        
        newpoint = lookatPoint.position;
        
        _GunAttributes = gameObject.GetComponent<GunAttributes>();

        transform.position = target.position;
        
    }
    
    public void UpdateStats()
    {
        
        
        // stats are divided by 10 to make inputting them in the Mod assets make more sense
        rotationLazyness = 0.3f;
        RecoilAmount = 2f;
        ReturnTime = 0.05f;    
        
        rotationLazyness += _GunAttributes.totalErgonomy / 10;
        RecoilAmount += _GunAttributes.totalRecoil / 10;
        ReturnTime += _GunAttributes.totalErgonomy / 100f;
        
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if (isAiming)
                {
                    _recoil = aimingRecoil;
                }
                else
                {
                    _recoil = RecoilAmount;
                }
        //UpdateStats();
        
        timer1 += Time.deltaTime * RateOfFire;
        timer2 += Time.deltaTime / 2;
        
        
        // resets the timer
        if (timer1 > 100000)
        {
            timer1 = 0;
        }

        //Disables gun lazy rotation
        if (DisableLazyGun)
        {
            _rotationLazyness = 1;
        }
        else
        {
            _rotationLazyness = rotationLazyness;
        }
        
        LazyGun();

        if (Input.GetMouseButton(0) && !_GunAttributes.isModding && _GunAttributes.ammoInMag > 0)
        {
            Firing();
            Recoiling();
        }
        else if (Input.GetMouseButtonDown(0) && !_GunAttributes.isModding && _GunAttributes.ammoInMag <= 0)
        {
            SoundManager.PlaySound("Empty");
        }

        else
        {
            // return back to normal position
            lookatPoint.localPosition += (defaultPos - lookatPoint.localPosition) * ReturnTime;
        }
    }

    

    public void LazyGun()
    {
        if (!_GunAttributes.isModding)
        {
            
            transform.position = target.position;

            var position = lookatPoint.position;
            
            // newpoint += differnce between aiming point and itself * rotationLazyness ( around 0.5 in normal setup)
            
            newpoint += (position - newpoint) * _rotationLazyness;

            //newpoint.x = Mathf.Clamp(newpoint.x, position.x - 10f, position.x + 10f);

            if (showRecoil)
            {
                debugCube.transform.position = newpoint;
            }
            
            
            transform.LookAt(newpoint);
        }

        
        

    }

    public void Firing()
    {
        if (_canFire)
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        _canFire = false;
        //
        //Aseen ampumis ääni tähän
        SoundManager.PlaySound("Shoot");
        //
        yield return new WaitForSeconds(increasedRof == true ? RateOfFire2 : 0.041f);
        shooting.Shoot(); 
        muzzleFlash.Play();
        _GunAttributes.ammoInMag--;
        _canFire = true;
    }
    
    public void Recoiling()
    {
        
        
        isRecoiling = true;
        
        if (recoilTime < 1)
        {
            recoilTime += Time.deltaTime / 2;    
        }
        else
        {
            recoilTime = 0;
        }
        
        
        
        var localRotation = cameraTransform.localRotation;
        
        // get a positive only sine wave and add perlin noise sample
        float sineValue =  Mathf.Sin(timer2);
        if (sineValue < 0)
        {
            sineValue += -sineValue;
            sineValue += Mathf.PerlinNoise(timer2, Random.Range(0, 50)) * 2;
        }
        
        //headbobbipn when firing
        localRotation.eulerAngles = new Vector3(localRotation.eulerAngles.x + sineValue * FeltRecoil,localRotation.eulerAngles.y + Mathf.Sin(timer2) + (Mathf.PerlinNoise(timer2, Random.Range(0, 50)) * (FeltRecoil / 2.5f)),localRotation.eulerAngles.z);
        cameraTransform.localRotation = localRotation;
        
        // gun recoil
        
        Vector3 randomPos = lookatPoint.localPosition;

        randomPos.y += Mathf.Sin(timer1) * _recoil;
        randomPos.x += Mathf.Sin(timer1) * horizontalRecoil * Random.Range(-0.5f, 0.5f);
        
        lookatPoint.localPosition = randomPos;
        
       // Debug.Log(Mathf.Sin(timer));
        
    }


    private IEnumerator Cooldown()
    {
        // Start cooldown
        onCooldown = true;
        // Wait for time you want
        yield return new WaitForSeconds(1.0f);
        // Stop cooldown
        onCooldown = false;
        //Debug.Log("Cooldown Ended");
    }

}
