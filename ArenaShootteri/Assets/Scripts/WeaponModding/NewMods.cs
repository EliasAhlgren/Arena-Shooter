using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NewMods : MonoBehaviour
{
    public List<Mod> modPool = new List<Mod>();
    
    public List<Mod> level1 = new List<Mod>();

    public List<Mod> level2 = new List<Mod>();

    public List<Mod> level3 = new List<Mod>();

    public ModSelection[] modSelections;

    public void CheckMods()
    {
        modSelections = FindObjectsOfType<ModSelection>();

        if (gameObject.GetComponent<GameManager>().wave == 5)
        {
            modPool.AddRange(level1);
        }
        if (gameObject.GetComponent<GameManager>().wave == 10)
        {
            modPool.AddRange(level2);
        }
        if (gameObject.GetComponent<GameManager>().wave == 15)
        {
            modPool.AddRange(level3);
        }
        
        
        foreach (var instance in modSelections)
        {
            List<Mod> mods = new List<Mod>();
            
            foreach (var mod in modPool)
            {
                if (mod.rail == instance.RailName)
                {
                    Debug.Log("Mod found");
                    mods.Add(mod);
                }    
            }

            Mod[] selectedMods = mods.ToArray();
            foreach (var VARIABLE in selectedMods)
            {
                Debug.Log(VARIABLE + " " + instance.gameObject);
            }
            Debug.Log("Lengths: " + selectedMods.Length);
            instance.selectedMods = new Mod[1];
            if (selectedMods.Length <= 0) continue;
            instance.selectedMods[0] = selectedMods[Random.Range(0,selectedMods.Length)];


        }
    }

}
