using UnityEngine;
using System.Collections;

public abstract class AbstractBuilding : MonoBehaviour
{
    public uint Life;
    public uint Level;
    public uint ViewDistance;
    public uint ProductionTime;
    public uint Level1Resource;
    public uint Level2Resource;
    public uint Level3Resource;
    public uint Level4Resource;
    public uint Level5Resource;

    public BuildingSetting SettingFile;

}
