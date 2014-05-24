using UnityEngine;
using System.Collections;
using System;

public class Focus : MonoBehaviour
{
    //[Flags]
    //public enum HANDLING : int
    //{
    //    UnlockFocus = -2,
    //    LockFocus = 2,
    //    DestroyFocus = -1,
    //    None = 0,
    //    HasFocus = 1,
    //    IsLocked = 3,
    //}
    public enum MARKERS : byte
    {
        MoveToPoint = 0,
        WayPoint = 1,
        AttackPoint = 2
    }

    // Static Member:
    public static bool IsLocked
    {
        get { return (KeyObject!=null); }
    }
    public static MarkerScript[] Marker = new MarkerScript[3];
    public static GameObject masterGameObject = null;
    private static GameObject KeyObject = null;
    public static GameObject Focusrectangle = null;
    private static bool firststart = true;
    private static bool UnitMenuIsOn
    {
        get { return RightClickMenu.showGUI; }
        set { RightClickMenu.showGUI = value; }
    }

    // Instance Member:
    private UnitScript UNIT;   //----------------------the Focussed Unit it's UnitScript...
    //private bool SettingFocusIsComplete=false;

    public bool IsLockedToThis
    {
        get
        {
            if (IsLocked)
                return this.gameObject == KeyObject;
            else return false;
        }
    }

    private Vector3 groundHit;

    void Start()
    {
        //SettingFocusIsComplete = false;
        if (firststart == false)
        {
            if ((!IsLocked) || (IsLockedToThis))
            {
                if (this.gameObject.GetComponent<UnitScript>())
                    UNIT = this.gameObject.GetComponent<UnitScript>();
                masterGameObject = this.gameObject;
            }
            if (!IsLocked)
            {
                UpdateManager.OnUpdate += DoUpdate;
                MouseEvents.RIGHTCLICK += MouseEvents_RIGHTCLICK;
                MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
                //SettingFocusIsComplete = true;
            }

            // Add Healthbar it not there
            if (gameObject.GetComponent<UnitScript>())
                gameObject.GetComponent<UnitScript>().ShowLifebar();
        }   
        else
        {
            //-Focus's first start:  
            //-References needet by The Focus. It is called only once, at Gamestart.
            //-when References have been copied, this childopjects will be detached
            // and Focus will Destroy itself, keeping this references in its Static part of it's class)

            Focusrectangle = this.gameObject;
            Marker[0] = GUIScript.main.transform.FindChild("MoveToPoint").gameObject.GetComponent<MarkerScript>();
            Marker[1] = GUIScript.main.transform.FindChild("WayPoint").GetComponent<MarkerScript>();
            Marker[2] = GUIScript.main.transform.FindChild("AttackPoint").GetComponent<MarkerScript>();
            GUIScript.main.transform.DetachChildren();

            foreach (MarkerScript marker in Marker)
                marker.Visible = false;

            Component.Destroy(this.gameObject.GetComponent<Focus>());
        }
    }

    [SerializeField]
    //private bool _islocked = false;
    // Check if Focus is on another gameobject -> than release old-focus
    void DoUpdate()
    {
        //_islocked = IsLocked;
        if (IsLocked)
        {
            // gets back the Focus to the LockedUnit if it was mistakenly taken by another Unit 
            masterGameObject = gameObject;
        }
        else
        {
            // handels the Focus to another Unit, if another Unit will be clicked
            TryRelease();
        }
    }

    void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        
        if ((!hold)&&(!UnitMenuIsOn))
        {
            if (!IsLocked)
            {
                if (MouseEvents.State.Position.AsUnitUnderCursor)
                {
                    UnitScript ClickedUnit = MouseEvents.State.Position.AsUnitUnderCursor;
                    if (ClickedUnit)
                    {
                        if (IsEnemy(ClickedUnit.gameObject))
                        {
                            Marker[(int)MARKERS.AttackPoint].GetComponent<Follower>().targetTransform = ClickedUnit.transform;
                            Marker[(int)MARKERS.AttackPoint].GetComponent<FaceDirection>().TransformToFace = gameObject.transform;
                            Marker[(int)MARKERS.AttackPoint].renderer.enabled = true;
                            UNIT.Options.FocussedLeftOnEnemy(ClickedUnit.gameObject); //------------triggers the Units StandardOrder for Clicking an EnemyUnit
                            //-----------------------------------------------------------(StandardOrders are Processed via this one call and do not need the unit to Lock the Focus for more specification....)
                            //------------------------------------------------------------...all other detailed nonStandard Orders are handled by the RightClickMenu and need to Lock the focus 
                        }
                        else if (IsOtherUnit(ClickedUnit.gameObject))
                        {
                            Marker[(int)MARKERS.WayPoint].GetComponent<Follower>().targetTransform = ClickedUnit.transform;
                            Marker[(int)MARKERS.WayPoint].GetComponent<FaceDirection>().TransformToFace = gameObject.transform;
                            Marker[(int)MARKERS.WayPoint].renderer.enabled = true;
                            UNIT.Options.FocussedLeftOnAllied(ClickedUnit.gameObject);//-------------triggers the Units StandardOrder for Clicking a friendly Unit
                        }
                    }

                }
                else
                {
                    Marker[(int)MARKERS.MoveToPoint].transform.position = MouseEvents.State.Position.AsWorldPointOnMap;
                    Marker[(int)MARKERS.MoveToPoint].renderer.enabled = true;

                    UNIT.Options.FocussedLeftOnGround(MouseEvents.State.Position.AsWorldPointOnMap); // StandardOrder for Clicking on Ground (its MoveTo in most cases...)
                }
            }
            else if (IsLockedToThis)
            {
                UNIT.Options.MouseEvents_LEFTCLICK(qamRay, hold);
            }
        }
    }

    void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {
        if ((!hold) && (!UnitMenuIsOn))
        {
            if (!IsLocked)
            {
                if (MouseEvents.State.Position.AsUnitUnderCursor)
                {
                    UnitScript ClickedUnit = MouseEvents.State.Position.AsUnitUnderCursor;

                    if (IsOtherUnit(ClickedUnit.gameObject))
                    {//--------------- I think of adding anozher Menu like InteractionMenu or what.
                        //-------------it would contain Interaction specific comamds like Guard, GroupMove, BuildAGroup, TakeLeadership, and so on...
                        //-------------butt now this Rightclick will handle the focus to the other Unit and call it's CommandOptionsMenu
                        ClickedUnit.gameObject.AddComponent<Focus>();
                        RightClickMenu.PopUpGUI(MouseEvents.State.Position.AsUnitUnderCursor);
                    }
                    else
                    {//---------------the Focussed Unit (This unit) was Rightclicked self.
                        RightClickMenu.PopUpGUI(UNIT);
                    }
                }
                else
                {//------------ Rightclick on Ground...
                    GameObject.Destroy(gameObject.GetComponent<Focus>());
                }
            }

            if (IsLockedToThis)
            {
                UNIT.Options.MouseEvents_RIGHTCLICK(qamRay,hold);
                Debug.Log("Focussed Units RightClickHandler has been called !");
            }
        }
    }

    private bool IsEnemy(GameObject other)
    {
        if (other == null)
            return false;
        return other.GetComponent<UnitScript>().IsEnemy(UNIT.GoodOrEvil);
    }
    private bool IsOtherUnit(GameObject other)
    {
        return other.GetInstanceID() != this.gameObject.GetInstanceID(); 
    }


    // Locks the Focus to This Actual Focussed Unit, till it is has finished recieving orders,so no other Units Will recive MouseData untill
    // the complete ordering process is recognized...   should be called by the Unit right after an option on the RightclickPopUp has been Clicked
    public bool Lock()
    {
        if ((!IsLocked) || (IsLockedToThis))
        {
            KeyObject = this.gameObject;
            return true;
        }
        else return false;
    }

    // Releases the Lock to the locked Unit after its ordering process is finished, so other Units can get Focus again....
    // must be called by the Unit whitch has Locked the Focus,givin its own gameObject as parameter for UnlockKey, or the Focus wont be Unlocked...
    public bool Unlock(GameObject unlockKey)
    {
        if(IsLocked)
            if (unlockKey.GetInstanceID() == KeyObject.GetInstanceID())
                KeyObject = null;

        return !IsLocked;
    }
    
    private void TryRelease()
    {
        if (masterGameObject == null || masterGameObject.GetInstanceID() != gameObject.GetInstanceID())
        {
            Component.Destroy(gameObject.GetComponent<Focus>());
        }
    }

    void OnDestroy()
    {
        if (firststart)
        {
            firststart = false;
        }
        else
        {
            MouseEvents.RIGHTCLICK -= MouseEvents_RIGHTCLICK;
            MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
            UpdateManager.OnUpdate -= DoUpdate;

            // Destroy Lifebar if not already destroyed
            if (gameObject.GetComponent<UnitScript>())
                gameObject.GetComponent<UnitScript>().HideLifebar();
        }  
    }
}
