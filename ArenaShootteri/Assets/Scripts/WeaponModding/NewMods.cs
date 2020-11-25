using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NewMods : MonoBehaviour
{
    public List<Mod> modPool = new List<Mod>();
    
    public List<Mod> level1 = new List<Mod>();

    public List<Mod> level2 = new List<Mod>();

    public List<Mod> level3 = new List<Mod>();

    public ModSelection[] modSelections;

    public void Start()
    {
        modSelections = Component.FindObjectsOfType<ModSelection>();
        
        
        foreach (var instance in modSelections)
        {
            List<Mod> mods = new List<Mod>();
            
            foreach (var mod in modPool)
            {
                if (mod.rail == instance.RailName)
                {
                    mods.Add(mod);
                }
            }

            instance.selectedMods[0] = mods[Random.Range(0, mods.Count)];

        }
    }

}
