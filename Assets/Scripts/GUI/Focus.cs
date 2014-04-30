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
        get { return Key; }
    }
    public static MarkerScript[] Marker = new MarkerScript[3];
    public static GameObject masterGameObject = null;
    private static GameObject Key = null;
    public static GameObject Focusrectangle = null;
    private static bool firststart = true;

    private UnitScript UNIT;

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
                return this.gameObject.GetInstanceID() == Key.GetInstanceID();
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
        }   
        else
        {
            UNIT = this.gameObject.GetComponent<UnitScript>();
            UNIT.Options.FocusFlag = HANDLING.HasFocus;
            masterGameObject = this.gameObject;
            MouseEvents.RIGHTCLICK += MouseEvents_CancelFocus;
            MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
			
            //MouseEvents.LEFTCLICK += MouseEvents_CancelFocus;
            //MouseEvents.RIGHTCLICK += MouseEvents_MoveUnit;

            RightClickMenu.PopUpGUI(UNIT);
        }

        UpdateManager.OnUpdate += DoUpdate;

        
    }

    void DoUpdate()
    {
        if (IsLocked)
            masterGameObject = gameObject;
        else
            TryRelease();
    }

    void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    //void MouseEvents_MoveUnit(Ray qamRay, bool hold)
    {
        if (!IsLocked)
        {
            if (!UnitMenuIsOn)
            {
                if (!hold)
                {

                    GameObject ClickedUnit = AnimatedCursor.UnitUnderCursor;
                    if (ClickedUnit)
                    {
                        if (IsEnemy(ClickedUnit)) 
                        {
                            Marker[(int)MARKERS.AttackPoint].GetComponent<Follower>().targetTransform = ClickedUnit.transform;
                            Marker[(int)MARKERS.AttackPoint].GetComponent<FaceDirection>().TransformToFace = gameObject.transform;
                            Marker[(int)MARKERS.AttackPoint].renderer.enabled = true;
                            UNIT.Options.FocussedLeftOnEnemy(ClickedUnit); 
                        }
                        else if (IsOtherUnit(ClickedUnit))
                        {
                            Marker[(int)MARKERS.WayPoint].GetComponent<Follower>().targetTransform = ClickedUnit.transform;
                            Marker[(int)MARKERS.WayPoint].GetComponent<FaceDirection>().TransformToFace = gameObject.transform;
                            Marker[(int)MARKERS.WayPoint].renderer.enabled = true;
                            UNIT.Options.FocussedLeftOnAllied(ClickedUnit);
                        }
                    }
                    else
                    {
                        Marker[(int)MARKERS.MoveToPoint].transform.position = MouseEvents.State.Position.AsWorldPointOnMap;
                        Marker[(int)MARKERS.MoveToPoint].renderer.enabled = true;
                        UNIT.Options.FocussedLeftOnGround(MouseEvents.State.Position.AsWorldPointOnMap);
                    }
                }
            }
        }
    }

    void MouseEvents_CancelFocus(Ray qamRay, bool hold)
    {
        if (!IsLocked)
        {
            if (!hold)
            {
                GameObject ClickedUnit = AnimatedCursor.UnitUnderCursor;
                if (ClickedUnit)
                {
                    if (IsOtherUnit(ClickedUnit))
                    {
                        ClickedUnit.AddComponent<Focus>();
                    }
                }
                else
                {
                    GameObject.Destroy(gameObject.GetComponent<Focus>());
                }
            }
        }
    }

    private bool IsEnemy(GameObject otherUnit)
    {
        return otherUnit.GetComponent<UnitScript>().GoodOrEvil != this.UNIT.GoodOrEvil;
    }
    private bool IsOtherUnit(GameObject unit)
    {
        return unit.GetInstanceID() != gameObject.GetInstanceID(); 
    }

    public void Lock()
    {
        if (UNIT.Options.FocusFlag == HANDLING.HasFocus)
        {
            Key = this.gameObject;
            UNIT.Options.FocusFlag |= HANDLING.LockFocus;
        }
    }
    public bool Unlock(GameObject key)
    {
        if (key.GetInstanceID() == Key.GetInstanceID()) { key.GetComponent<UnitScript>().Options.FocusFlag |= HANDLING.UnlockFocus; Key = null; }
        return IsLockedToThis;
    }
    private void TryRelease()
    {
        if (masterGameObject.GetInstanceID() != gameObject.GetInstanceID())
            Component.Destroy(gameObject.GetComponent<Focus>());
    }

    void Update()
    {
        if (IsLockedToThis) masterGameObject = gameObject;
        else TryRelease();
    }

    void OnDestroy()
    {
        if (!firststart)
        {
            MouseEvents.RIGHTCLICK -= MouseEvents_CancelFocus;
            MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
            //MouseEvents.LEFTCLICK -= MouseEvents_CancelFocus;
            //MouseEvents.RIGHTCLICK -= MouseEvents_MoveUnit;
			
            UNIT.Options.FocusFlag = HANDLING.None;
        }
        else firststart = false;
        UpdateManager.OnUpdate -= DoUpdate;
    }

}
