using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class BuildingSetting : ScriptableObject
{
    // Main
    public int Level;
    public int UsedEnergy;
    
    // Build
    public uint MatterCost;
    public uint NaniteCost;
    
    // Upgrade
    public List<int> UpgradeCostNanite = new List<int>();
    public List<int> UpgradeCostMatter = new List<int>(); 

    // Production
    public List<int> LevelResource = new List<int>();
}
