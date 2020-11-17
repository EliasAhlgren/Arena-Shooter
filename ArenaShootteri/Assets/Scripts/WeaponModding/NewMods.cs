using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMods : MonoBehaviour
{
    [Serializable]
    public struct AvailableModsStruct
    {
        public Mod Mod;
        public String railName;
        public int Level;
    }
    public AvailableModsStruct[] AvailableMods;

    private ModSelection[] _modSelections;
    
    public void CheckWave(int currentWave)
    {
        
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
