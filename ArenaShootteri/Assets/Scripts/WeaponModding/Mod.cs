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

    public float Ergonomy;

    public int Recoil;

    public float Damage;


}
