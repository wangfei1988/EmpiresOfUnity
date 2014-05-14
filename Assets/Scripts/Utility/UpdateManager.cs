using UnityEngine;
using System.Collections;

/*
 * Event Handler for Update per frame.
 * Use this instead of Unity "void Update()"-Method
 * Usage:
 *  - UpdateManager.OnUpdate += DoUpdate;
 *  - void DoUpdate() {}
 */
public class UpdateManager : MonoBehaviour 
{

    public delegate void UnitUpdates();
    public static event UnitUpdates UNITUPDATE;

    public delegate void WeaponUpdates();
    public static event WeaponUpdates WEAPONUPDATES;

    public delegate void GUIUpdates();
    public static event GUIUpdates GUIUPDATE;

    public delegate void UpdateEvent();
    public static event UpdateEvent OnUpdate;
    public static event UpdateEvent OnMouseUpdate;

    void Update()
    {
        UpdateMouse();
        UpdateGUI();
        UpdateUnits();
        UpdateWeapons();

        UpdateDefatult();
    }


    private void UpdateMouse()
    {
        MouseEvents.DoUpdate();
        if (OnMouseUpdate != null)
            OnMouseUpdate();
    }

    private void UpdateGUI()
    {
        if (GUIUPDATE != null)
            GUIUPDATE();
    }

    private void UpdateUnits()
    {
        if (UNITUPDATE != null)
            UNITUPDATE();
    }

    private void UpdateWeapons()
    {
        if (WEAPONUPDATES != null)
            WEAPONUPDATES();
    }

    private void UpdateDefatult()
    {
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

}
