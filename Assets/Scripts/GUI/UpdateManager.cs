using UnityEngine;
using System.Collections;

public class UpdateManager : MonoBehaviour 
{

    public delegate void UnitUpdates();
    public static event UnitUpdates UNITUPDATE;
    public delegate void WeaponUpdates();
    public static event WeaponUpdates WEAPONUPDATES;
    public delegate void GUIUpdates();
    public static event GUIUpdates GUIUPDATE;
    public delegate void MouseUpdate();


    private void UpdateUnits()
    {
        if (UNITUPDATE != null) UNITUPDATE();
    }

    private void UpdateWeapons()
    {
        if (WEAPONUPDATES != null) WEAPONUPDATES();
    }

    private void UpdateGUI()
    {
        if (GUIUPDATE != null) GUIUPDATE();  
    }

    private void UpdateMouse()
    {
        MouseEvents.DoUpdate();
    }



    void Update()
    {
        UpdateMouse();
        UpdateGUI();
        UpdateUnits();
        UpdateWeapons();
    }
}
