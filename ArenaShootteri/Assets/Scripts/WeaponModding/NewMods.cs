using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NewMods : MonoBehaviour
{
    [Serializable]
    public struct AvailableModsStruct
    {
        public Mod Mod;
        public String railName;
    }
    [Serializable]
    public struct Level
    {
        public AvailableModsStruct[] ModsOnThisLevel;
    }

    public Level[] levels;
    
    private ModSelection[] _modSelections;

    
    public void CheckWave(int currentWave)
    {
        for (int i = 0; i < _modSelections.Length; i++)
        {
            if (levels.Length >= currentWave)
            {
                foreach (var VARIABLE in levels[currentWave].ModsOnThisLevel)
                {
                    if (VARIABLE.railName == _modSelections[i].RailName)
                    {
                        _modSelections[i].selectedMods[0] = VARIABLE.Mod;
                    }
                    else
                    {
                        //_modSelections[i].OnSelectEmpty();
                    }
                }
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ModSelection");
        _modSelections = new ModSelection[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            _modSelections[i] = objects[i].GetComponent<ModSelection>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
