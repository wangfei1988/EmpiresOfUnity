using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class BuildingSetting : ScriptableObject
{
    // Main
    public int Level;
    public int UsedEnergy;
    public bool EnergyUse;
    
    // Build
    public int MatterCost;
    public int NaniteCost;
    public bool IsBuildable;
    
    // Upgrade
    public List<int> UpgradeCostNanite = new List<int>();
    public List<int> UpgradeCostMatter = new List<int>(); 

    // Production
    public List<int> LevelResource = new List<int>();
}
