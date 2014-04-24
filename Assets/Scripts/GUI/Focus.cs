using UnityEngine;
using System.Collections;


public class Focus : MonoBehaviour
{
    public enum MARKERS : byte
    {
        MoveToPoint = 0,
        WayPoint = 1,
        AttackPoint = 2
    }
    public bool IsLocked = false;
    public static GameObject[] Marker = new GameObject[3];
    public static GameObject masterGameObject;
    private static GameObject Key;
    public static GameObject Focusrectangle;
    private static bool firststart = true;
    private UnitScript UNIT
    {
        get { return gameObject.GetComponent<UnitScript>(); }
    }
    private bool UnitMenuIsOn
    {
        get { return RightClickMenu.showGUI; }
        set { RightClickMenu.showGUI = value; }
    }
    

    private Vector3 groundHit;

    void Start()
    {
        if (firststart)
        {
            Focusrectangle = gameObject;
            Marker[0] = transform.FindChild("MoveToPoint").gameObject;
            Marker[1] = transform.FindChild("WayPoint").gameObject;
            Marker[2] = transform.FindChild("AttackPoint").gameObject;
            transform.DetachChildren();
            Component.Destroy(gameObject.GetComponent<Focus>());
        }   
        else
        {
            masterGameObject = gameObject;
            MouseEvents.RIGHTCLICK += MouseEvents_RIGHTCLICK;
            MouseEvents.LEFTCLICK += MouseEvents_LEFTMouseEvents;
        }
    }

    void MouseEvents_LEFTMouseEvents(Ray qamRay, bool hold)
    {
        if (!IsLocked)
        {
            if (!UnitMenuIsOn)
            {
                if (!hold)
                {
                    
                    GameObject ClickedUnit = RayHittenUnit(qamRay);
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
                        Marker[(int)MARKERS.MoveToPoint].transform.position = groundHit;
                        Marker[(int)MARKERS.MoveToPoint].renderer.enabled = true;
                        UNIT.Options.FocussedLeftOnGround(groundHit);
                    }
                }
            }
        }
    }

    void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {
        if (!IsLocked)
        {
            if (!hold)
            {
                GameObject ClickedUnit = RayHittenUnit(qamRay);
                if (ClickedUnit)
                {
                    if (IsOtherUnit(ClickedUnit)) ClickedUnit.AddComponent<Focus>();
                    else RightClickMenu.PopUpGUI(UNIT);
                }
                else GameObject.Destroy(gameObject.GetComponent<Focus>());
            }
        }
    }

    private GameObject RayHittenUnit(Ray clickRay)
    {
        RaycastHit clicked;
        if (Physics.Raycast(clickRay, out clicked)) return clicked.collider.gameObject;
        else
        {
            GameObject.FindGameObjectWithTag("Ground").collider.Raycast(clickRay, out clicked, 3000f);
            groundHit = clicked.point;
        }
        return null;
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
        Key = gameObject;
        IsLocked = true;
    }
    public bool Unlock(GameObject key)
    {
        if (key == Key) { Key = null; IsLocked = false; }
        return IsLocked;
    }
    private void TryRelease()
    {
        if (masterGameObject.GetInstanceID() != gameObject.GetInstanceID())
            Component.Destroy(gameObject.GetComponent<Focus>());
    }

    void Update()
    {
        if (IsLocked) masterGameObject = gameObject;
        else TryRelease();
    }

    void OnDestroy()
    {
        if (!firststart)
        {
            MouseEvents.RIGHTCLICK -= MouseEvents_RIGHTCLICK;
            MouseEvents.LEFTCLICK -= MouseEvents_LEFTMouseEvents;
        }
        else firststart = false;
    }

}
