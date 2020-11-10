using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{

    private ModSelection SightSelection;
    
    public Transform playerCamera;

    public Transform target;
    
    public bool isAiming;

    public float positionDifferenceY;

    public float positionDifferenceZ;
    
    private Vector3 defaultPosition;

    private Recoil _recoil;
    
    // Start is called before the first frame update
    void Start()
    {
        SightSelection = GameObject.Find("SightSelection").GetComponent<ModSelection>();
        _recoil = gameObject.GetComponent<Recoil>();
        defaultPosition = target.localPosition;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isAiming = !isAiming;
        }
        if (isAiming)
        {
            AimDownSights();
        }
        else
        {
           target.localPosition = defaultPosition;
        }
    }

    private void AimDownSights()
    {
        if (SightSelection.currenModStats)
        {
            positionDifferenceY = SightSelection.currenModStats.AimHeight;
        }
        else
        {
            positionDifferenceY = -0.58f;
        }
        
        
        
        target.localPosition = new Vector3(0,positionDifferenceY,defaultPosition.z + positionDifferenceZ);
        //target.position = new Vector3(playerCamera.position.x,playerCamera.position.y - positionDifference, playerCamera.position.z + target.localPosition.z);
        
    }
    
}
