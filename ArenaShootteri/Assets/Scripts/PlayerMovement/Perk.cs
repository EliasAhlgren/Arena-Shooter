using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perk")]
public class Perk : ScriptableObject
{
    public string perkName = "perk name";
    public string perkDescription = "perk description";

    public float pHealth = 0f;

    public float pRunSpeed = 0f;
    public float pWalkSpeed = 0f;

    public float pSlideSpeedControl = 0f;
    public float pSlideDuration = 0f;

    public float pJumpHeight = 0f;
    public float pAirMoveModifier = 0f;

    private PlayerStats PCS;
    public void SetStat(GameObject obj)
    {
        PCS = obj.GetComponent<PlayerStats>();

        PCS.perkHealth += pHealth;

        PCS.runSpeed += pRunSpeed;
        PCS.walkSpeed += pWalkSpeed;

        PCS.slideSpeedControl += pSlideSpeedControl;
        PCS.slideDuration += pSlideDuration;

        PCS.jumpHeight += pJumpHeight;
        PCS.airMoveModifier += pAirMoveModifier;
}
}
