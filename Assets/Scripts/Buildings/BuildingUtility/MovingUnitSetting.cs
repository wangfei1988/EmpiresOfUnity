using UnityEngine;
using System.Collections;

public class MovingUnitSetting : ScriptableObject
{
    public EnumProvider.ORDERSLIST currentTask;

    public bool[] UnitsCurrentMovingStateSet;
    public string[] UnitsMovingStateNames;
    public int[] currentInteractingUnitsList;
    public int TargetUnitsInstaceID;
    public Component[] currentAttachedMovingrelevantComponents;

    public int Life;
    public int SettingsFile.Level;
    public int ViewDistance;
    public int EnergyConsumption;
    public int ProductionTime;
    public float Speed;
    public Vector3 MoveToPoint;
    public int SettingsFile.Level3Resource;
    public int SettingsFile.Level4Resource;
    public int SettingsFile.Level5Resource;

}
