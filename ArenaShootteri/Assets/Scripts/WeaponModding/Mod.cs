using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSight")]
public class Mod : ScriptableObject
{
    public String Name;
    
    public string Type;
    
    public int Level;

    // How high the gun should be held
    public float AimHeight;

    public int PoseNumber;
    
    public GameObject Prefab;

    public Sprite Icon;

    //Determines if you can move the object on the rail
    public bool moveable = true;
    
    public float Ergonomy;

    public int Recoil;

    public float Damage;

    public Vector3 posDiff;

}
