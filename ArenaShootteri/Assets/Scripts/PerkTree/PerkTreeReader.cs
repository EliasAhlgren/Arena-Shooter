using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

public class PerkTreeReader : MonoBehaviour
{
    private static PerkTreeReader _instance;

    public static PerkTreeReader Instance
    {
        get
        {
            return _instance;
        }
        set
        {
        }
    }

    // Array with all the perks in our perktree
    private Perk[] _perkTree;

    // Dictionary with the perks in our perktree
    private Dictionary<int, Perk> _perks;

    // Variable for caching the currently being inspected perk
    private Perk _perkInspected;

    public PlayerCharacterControllerRigidBody player;
    //public PerkHub perkHub;

    public bool savePerks = true;

    public int availablePoints = 100;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
            
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (!savePerks)
        {
            PlayerPrefs.SetInt("PerkPoints", 0);
            PlayerPrefs.DeleteKey("PerkTree");
        }

        availablePoints = PlayerPrefs.GetInt("PerkPoints", 0);
        SetUpPerkTree();
        
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacterControllerRigidBody>();
    }

    // Use this for initialization of the perk tree
    void SetUpPerkTree()
    {
        _perks = new Dictionary<int, Perk>();

        if (PlayerPrefs.GetString("PerkTree", "").CompareTo("") == 0)
        {
            LoadPerkTree();
        }
        else
        {
            LoadPlayerPerkTree();
        }
        //LoadPerkTree();
    }

    public void LoadPerkTree()
    {
        string path = "Assets/Data/perktree.json";
        string dataAsJson;
        if (File.Exists(path))
        {
            // Read the json from the file into a string
            dataAsJson = File.ReadAllText(path);

            // Pass the json to JsonUtility, and tell it to create a PerkTree object from it
            PerkTree loadedData = JsonUtility.FromJson<PerkTree>(dataAsJson);

            // Store the PerkTree as an array of Perk
            _perkTree = new Perk[loadedData.perktree.Length];
            _perkTree = loadedData.perktree;

            // Populate a dictionary with the perk id and the perk data itself
            for (int i = 0; i < _perkTree.Length; ++i)
            {
                _perks.Add(_perkTree[i].id_Perk, _perkTree[i]);
            }
        }
        else
        {
            //Debug.LogError("Cannot load game data!");
        }
    }

    public void LoadPlayerPerkTree()
    {
        string dataAsJson = PlayerPrefs.GetString("PerkTree");

        // Pass the json to JsonUtility, and tell it to create a PerkTree object from it
        PerkTree loadedData = JsonUtility.FromJson<PerkTree>(dataAsJson);

        // Store the PerkTree as an array of Perk
        _perkTree = new Perk[loadedData.perktree.Length];
        _perkTree = loadedData.perktree;

        // Populate a dictionary with the perk id and the perk data itself
        for (int i = 0; i < _perkTree.Length; ++i)
        {
            _perks.Add(_perkTree[i].id_Perk, _perkTree[i]);
        }
    }

    public void SavePerkTree()
    {
        // We fill with as many perks as nodes we have
        PerkTree perkTree = new PerkTree();
        perkTree.perktree = new Perk[_perkTree.Length];
        for (int i = 0; i < _perkTree.Length; ++i)
        {
            _perks.TryGetValue(_perkTree[i].id_Perk, out _perkInspected);
            if (_perkInspected != null)
            {
                perkTree.perktree[i] = _perkInspected;
            }
        }

        string json = JsonUtility.ToJson(perkTree);

        PlayerPrefs.SetString("PerkTree", json);
    }

    public PerkType IsPerkType(int id_perk)
    {
        if (_perks.TryGetValue(id_perk, out _perkInspected))
        {
            return _perkInspected.perktype;
        }

        return PerkType.passive;
    }

    public int IsPerkLevel(int id_perk)
    {
        if (_perks.TryGetValue(id_perk, out _perkInspected))
        {
            return _perkInspected.perkLevel;
        }

        return 0;
    }

    public bool IsActivePerkUnlocked()
    {
        for (int i = 0; i < _perkTree.Length; ++i)
        {
            _perks.TryGetValue(_perkTree[i].id_Perk, out _perkInspected);
            if (_perkInspected != null)
            {
                if (_perkInspected.perktype == PerkType.active)
                {
                    if (_perkInspected.unlocked)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool IsPerkUnlocked(int id_perk)
    {
        if (_perks.TryGetValue(id_perk, out _perkInspected))
        {
            return _perkInspected.unlocked;
        }
        else
        {
            return false;
        }
    }

    public int IsPerkCost(int id_perk)
    {
        if (_perks.TryGetValue(id_perk, out _perkInspected))
        {
            return _perkInspected.cost;
        }

        return 0;
    }

    public bool CanActivePerkBeUnlocked(int id_perk)
    {
        bool canUnlock = true;

        if (_perks.TryGetValue(id_perk, out _perkInspected)) // The perk exists
        {
                for (int i = 0; i < _perkTree.Length; ++i)
                {

                    if (_perks.TryGetValue(_perkTree[i].id_Perk, out Perk _perkInspecedActive))
                    {
                        if (_perkInspecedActive.perktype == PerkType.active)
                        {
                            if (_perkInspecedActive.unlocked)
                            {
                                //Debug.Log("active perk unlocked");
                                canUnlock = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
        }
        else // If theperk id doesn't exist, the perk can't be unlocked
        {
            return false;
        }

        return canUnlock;
    }

    public bool CanPerkBeUnlockedCost(int id_perk)
    {
        bool canUnlock = true;

        if (_perks.TryGetValue(id_perk, out _perkInspected)) // The perk exists
        {

            if (_perkInspected.cost <= availablePoints) // Enough points available
            {

            }
            else // If the player doesn't have enough perk points, can't unlock the new perk
            {
                return false;
            }


        }
        else // If theperk id doesn't exist, the perk can't be unlocked
        {
            return false;
        }
        return canUnlock;
    }

    public bool CanPerkBeUnlockedDependencies(int id_perk)
    {

        bool canUnlock = true;

        if (_perks.TryGetValue(id_perk, out _perkInspected)) // The perk exists
        {
            int[] dependencies = _perkInspected.perk_Dependencies;
            for (int i = 0; i < dependencies.Length; ++i)
            {
                if (_perks.TryGetValue(dependencies[i], out _perkInspected))
                {
                    if (!_perkInspected.unlocked)
                    {
                        canUnlock = false;
                        break;
                    }
                }
                else // If one of the dependencies doesn't exist, the perk can't be unlocked.
                {
                    return false;
                }
            }
        }
        else // If theperk id doesn't exist, the perk can't be unlocked
        {
            return false;
        }
        return canUnlock;
    }

    public bool CanPerkBeUnlocked(int id_perk)
    {
        bool canUnlock = true;

        if (_perks.TryGetValue(id_perk, out _perkInspected)) // The perk exists
        {
            
            if (_perkInspected.cost <= availablePoints) // Enough points available
            {
                
                if (_perkInspected.perktype == PerkType.active) // is perk type active
                {

                    for (int i = 0; i < _perkTree.Length; ++i)
                    {

                        if (_perks.TryGetValue(_perkTree[i].id_Perk, out Perk _perkInspecedActive))
                        {
                            if (_perkInspecedActive.perktype == PerkType.active)
                            {
                                if (_perkInspecedActive.unlocked)
                                {
                                    canUnlock = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                

                int[] dependencies = _perkInspected.perk_Dependencies;
                for (int i = 0; i < dependencies.Length; ++i)
                {
                    if (_perks.TryGetValue(dependencies[i], out _perkInspected))
                    {
                        if (!_perkInspected.unlocked)
                        {
                            canUnlock = false;
                            break;
                        }
                    }
                    else // If one of the dependencies doesn't exist, the perk can't be unlocked.
                    {
                        return false;
                    }
                }

            }
            else // If the player doesn't have enough perk points, can't unlock the new perk
            {
                return false;
            }


        }
        else // If theperk id doesn't exist, the perk can't be unlocked
        {
            return false;
        }
        return canUnlock;
    }

    public bool UnlockPerk(int id_Perk)
    {
        if (_perks.TryGetValue(id_Perk, out _perkInspected))
        {
            if (_perkInspected.perktype == PerkType.leveleable)
            {
                if (_perkInspected.perkLevel < 5)
                {
                    if(_perkInspected.cost <= availablePoints)
                    {
                        availablePoints -= _perkInspected.cost;
                        _perkInspected.unlocked = true;
                        _perkInspected.perkLevel += 1;

                        // We replace the entry on the dictionary with the new one (already unlocked)
                        _perks.Remove(id_Perk);
                        _perks.Add(id_Perk, _perkInspected);

                        player.CheckPerks();
                        return true;
                    }

                    return false;
                }
            }

            if (!_perkInspected.unlocked)
            {
                if (_perkInspected.cost <= availablePoints)
                {
                    availablePoints -= _perkInspected.cost;
                    _perkInspected.unlocked = true;

                    // We replace the entry on the dictionary with the new one (already unlocked)
                    _perks.Remove(id_Perk);
                    _perks.Add(id_Perk, _perkInspected);

                    player.CheckPerks();
                    return true;
                }
                else
                {
                    return false;   // The perk can't be unlocked. Not enough points
                }
            }
            else
            {
                return false; // The perk has already been unlocked
            }


        }
        else
        {
            return false;   // The perk doesn't exist
        }
    }

    public bool ResetPerk(int id_Perk)
    {
        if (_perks.TryGetValue(id_Perk, out _perkInspected))
        {
            if (_perkInspected.perktype == PerkType.leveleable)
            {
                availablePoints += _perkInspected.cost * _perkInspected.perkLevel;
                _perkInspected.perkLevel = 0;

                _perkInspected.unlocked = false;

                _perks.Remove(id_Perk);
                _perks.Add(id_Perk, _perkInspected);

                player.CheckPerks();
                return true;
            }

            if (_perkInspected.unlocked)
            {
                availablePoints += _perkInspected.cost;
                _perkInspected.unlocked = false;

                // We replace the entry on the dictionary with the new one (already locked)
                _perks.Remove(id_Perk);
                _perks.Add(id_Perk, _perkInspected);

                player.CheckPerks();
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;   // The perk doesn't exist
        }
    }

    public void AddPerkPoint(int points)
    {
        availablePoints += points;
        PlayerPrefs.SetInt("PerkPoints", availablePoints);
        //perkHub.RefreshButtons();
    }
}
