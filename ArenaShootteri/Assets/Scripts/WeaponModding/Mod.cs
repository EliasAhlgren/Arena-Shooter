using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSight")]
public class Mod : ScriptableObject
{
    public String Name;
    
    public string Type;
    
    public int Level;

    public GameObject Prefab;

    public Sprite Icon;

    public int Ergonomy;

    public int Recoil;

    public float Damage;


}
