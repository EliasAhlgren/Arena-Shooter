using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSight")]
public class Mod : ScriptableObject
{
    public String Name;
    
    public string Type;
    
    public int Level;

    //Determines what ADS animation to use
    public int animIndex;

    public GameObject Prefab;

    public Sprite Icon;

    //Determines if you can move the object on the rail
    public bool moveable = true;
    
    public float Ergonomy;

    public int Recoil;

    public float Damage;


}
