public enum PerkType { passive = 0, active = 1, leveleable = 2 };

[System.Serializable]
public class Perk
{

    public int id_Perk;
    public string perkName;
    public int[] perk_Dependencies;
    public bool unlocked;
    //public int type;
    public PerkType perktype;
    public int perkLevel;
    public int cost;
}