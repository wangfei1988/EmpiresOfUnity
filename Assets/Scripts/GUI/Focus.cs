using UnityEngine;
using System.Collections;
using System;

public class Focus : MonoBehaviour
{
    [Flags]
    public enum HANDLING : int
    {
        UnlockFocus = -2,
        LockFocus = 2,
        DestroyFocus = -1,
        None = 0,
        HasFocus = 1,
        IsLocked = 3,
    }
    public enum MARKERS : byte
    {
        MoveToPoint = 0,
        WayPoint = 1,
        AttackPoint = 2
    }
    public static bool IsLocked
    {
        get { return (Key!=null); }
    }
    public static MarkerScript[] Marker = new MarkerScript[3];
    public static GameObject masterGameObject = null;
    private static GameObject Key = null;
    public static GameObject Focusrectangle = null;
    private static bool firststart = true;

    private UnitScript UNIT;   //----------------------this Focussed Unit's the UnitScript...

    private bool UnitMenuIsOn
    {
        get { return RightClickMenu.showGUI; }
        set { RightClickMenu.showGUI = value; }
    }
    public bool IsLockedToThis
    {
        get
        {
            if (IsLocked)
                return this.gameObject == Key;
            else return false;
        }
    }

    private Vector3 groundHit;

    void Start()
    {
        if (firststart)
        {
            Focusrectangle = this.gameObject;
            Marker[0] = transform.FindChild("MoveToPoint").gameObject.GetComponent<MarkerScript>();
            Marker[1] = transform.FindChild("WayPoint").GetComponent<MarkerScript>();
            Marker[2] = transform.FindChild("AttackPoint").GetComponent<MarkerScript>();
            transform.DetachChildren();
            foreach (MarkerScript marker in Marker) marker.Visible = false;
            Component.Destroy(gameObject.GetComponent<Focus>());
            // todo Destroy other unit's focus
        }   
        else
        {
            UNIT = this.gameObject.GetComponent<UnitScript>();
            UNIT.Options.FocusFlag = HANDLING.HasFocus;
            masterGameObject = this.gameObject;
            MouseEvents.RIGHTCLICK += MouseEvents_RIGHTCLICK;
            MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
            UpdateManager.OnUpdate += DoUpdate;
        }
    }

    void DoUpdate()
    {
        if (IsLocked)
            masterGameObject = gameObject;
        else
            TryRelease();
    }

    void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        if (!IsLocked)
        {
            if (!UnitMenuIsOn)
            {
                if (!hold)
                {

                    GameObject ClickedUnit = UnitUnderCursor.gameObject;
                    if (ClickedUnit)
                    {
                        if (IsEnemy(ClickedUnit)) 
                        {
                            Marker[(int)MARKERS.AttackPoint].GetComponent<Follower>().targetTransform = ClickedUnit.transform;
                            Marker[(int)MARKERS.AttackPoint].GetComponent<FaceDirection>().TransformToFace = gameObject.transform;
                            Marker[(int)MARKERS.AttackPoint].renderer.enabled = true;
                            UNIT.Options.FocussedLeftOnEnemy(ClickedUnit); //------------triggers the Units StandardOrder for Clicking an EnemyUnit
                            //-----------------------------------------------------------(StandardOrders are Processed via this one call and do not need the unit to Lock the Focus for more specification....)
                            //------------------------------------------------------------...all other detailed nonStandard Orders are handled by the RightClickMenu and need to Lock the focus 
                        }
                        else if (IsOtherUnit(ClickedUnit))
                        {
                            Marker[(int)MARKERS.WayPoint].GetComponent<Follower>().targetTransform = ClickedUnit.transform;
                            Marker[(int)MARKERS.WayPoint].GetComponent<FaceDirection>().TransformToFace = gameObject.transform;
                            Marker[(int)MARKERS.WayPoint].renderer.enabled = true;
                            UNIT.Options.FocussedLeftOnAllied(ClickedUnit);//-------------triggers the Units StandardOrder for Clicking a friendly Unit
                        }
                    }
                    else
                    {
                        Marker[(int)MARKERS.MoveToPoint].transform.position = MouseEvents.State.Position.AsWorldPointOnMap;
                        Marker[(int)MARKERS.MoveToPoint].renderer.enabled = true;
                        UNIT.Options.FocussedLeftOnGround(MouseEvents.State.Position.AsWorldPointOnMap); // StandardOrder for Clicking on Ground (its MoveTo in most cases...)
                    }
                }
            }
        }
    }

    void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {
        if(!hold) 
        {
            if (!IsLocked)
            {
                if (UnitUnderCursor.gameObject)
                {
                    if (IsOtherUnit(UnitUnderCursor.gameObject))
                    {
                        UnitUnderCursor.gameObject.AddComponent<Focus>();
                        RightClickMenu.PopUpGUI(UnitUnderCursor.UNIT);
                    }
                    else
                    {//---------------the Focussed Unit (This unit) was Rightclicked
                        RightClickMenu.PopUpGUI(UNIT);
                    }
                }
                else
                {//------------ Rightclick on Ground...
                    GameObject.Destroy(gameObject.GetComponent<Focus>());
                }
            }
        }
    }

    private bool IsEnemy(GameObject otherUnit)
    {
        return otherUnit.GetComponent<UnitScript>().IsEnemy(UNIT.GoodOrEvil);
    }
    private bool IsOtherUnit(GameObject unit)
    {
        return unit.GetInstanceID() != gameObject.GetInstanceID(); 
    }

    public void Lock()
    {//---------------------------Locks the Focus to This Actual Focussed Unit, till it is has finished recieving orders,so no other Units Will recive MouseData untill
        //                        the complete ordering process is recognized...   should be called by the Unit right after an option on the RightclickPopUp has been Clicked
        if (UNIT.Options.FocusFlag == HANDLING.HasFocus)
        {
            Key = this.gameObject;
            UNIT.Options.FocusFlag |= HANDLING.LockFocus;
        }
    }
    public bool Unlock(GameObject unlockKey)
    {//----------------------------------Releases the Lock to the locked Unit after its ordering process is finished, so other Units can get Focus again....
        //-------------------------------must be called by the Unit whitch has Locked the Focus,givin its own gameObject as parameter for UnlockKey, or the Focus wont be Unlocked...
        if (unlockKey.GetInstanceID() == Key.GetInstanceID())
        {
            unlockKey.GetComponent<UnitScript>().Options.FocusFlag |= HANDLING.UnlockFocus;
            Key = null;
        }
        return IsLockedToThis;
    }
    private void TryRelease()
    {
        if (masterGameObject.GetInstanceID() != gameObject.GetInstanceID())
        {
            Component.Destroy(gameObject.GetComponent<Focus>());
            gameObject.gameObject.GetComponent<UnitScript>().HideLifebar();
        }
    }

    void Update()
    {
        if (IsLockedToThis)
            masterGameObject = gameObject; //----gets back the Focus to the LockedUnit if it was mistakenly taken by another Unit 
        else TryRelease();//----handels the Focus to another Unit, if another Unit will be clicked
    }

    void OnDestroy()
    {
        if (!firststart)
        {
            MouseEvents.RIGHTCLICK -= MouseEvents_RIGHTCLICK;
            MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
            UpdateManager.OnUpdate -= DoUpdate;
            UNIT.Options.FocusFlag = HANDLING.None;
        }
        else
        {
            firststart = false;
        }

        
    }

}
