using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttributes : MonoBehaviour
{
    public int totalErgonomy;
    public int totalRecoil;
    public float totalDamage;
    
        
    
    // Start is called before the first frame update
    void Start()
    {


        StartCoroutine(CheckDefaultStats());
    }


    // Check stats from default mods
    IEnumerator CheckDefaultStats()
    {
        // wait 1 second to account for possible lag or delay when instatiating default mods
        yield return new  WaitForSeconds(1f);
        
        foreach (var _stockSelection in GetComponentsInChildren<StockSelection>())
        {
            Debug.Log("initialising with stats");
            _stockSelection.OnModChosen += UpdateStats;
            if (_stockSelection.currenModStats)
            {
                
                totalErgonomy += _stockSelection.currenModStats.Ergonomy;
                totalRecoil += _stockSelection.currenModStats.Recoil;
                totalDamage += _stockSelection.currenModStats.Damage;
            }
            
        }
        
        foreach (var _modSelection in GetComponentsInChildren<ModSelection>())
        {
            _modSelection.OnModChosen += UpdateStats;
            if (_modSelection.currenModStats)
            {
                totalErgonomy += _modSelection.currenModStats.Ergonomy;
                totalRecoil += _modSelection.currenModStats.Recoil;
                totalDamage += _modSelection.currenModStats.Damage;
            }
            
        }
    }
    
    // called everytime a mod is changed in any of the rails
    void UpdateStats()
    {
        
        Vector3 totalStats;
        totalStats.x = totalErgonomy;
        totalStats.y = totalRecoil;
        totalStats.z = totalDamage;

        totalErgonomy -= Mathf.RoundToInt(totalStats.x);
        totalRecoil -= Mathf.RoundToInt(totalStats.y);
        totalDamage -= Mathf.RoundToInt(totalStats.z);
        
        Debug.Log("Updating Stats");
        
        foreach (var _modSelection in GetComponentsInChildren<ModSelection>())
        {
            if (_modSelection.currenModStats)
            {
                totalErgonomy += _modSelection.currenModStats.Ergonomy;
                totalRecoil += _modSelection.currenModStats.Recoil;
                totalDamage += _modSelection.currenModStats.Damage;
            }
        }
        
        foreach (var _modSelection in GetComponentsInChildren<StockSelection>())
        {
            if (_modSelection.currenModStats)
            {
                totalErgonomy += _modSelection.currenModStats.Ergonomy;
                totalRecoil += _modSelection.currenModStats.Recoil;
                totalDamage += _modSelection.currenModStats.Damage;
            }
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
