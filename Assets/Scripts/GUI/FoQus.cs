using UnityEngine;
using System.Collections;


public class FoQus : MonoBehaviour
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
    public static GameObject Foqusrectangle;
    private static bool firststart = true;
    private UnitSqript UNIT
    {
        get { return gameObject.GetComponent<UnitSqript>(); }
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
            Foqusrectangle = gameObject;
            Marker[0] = transform.FindChild("MoveToPoint").gameObject;
            Marker[1] = transform.FindChild("WayPoint").gameObject;
            Marker[2] = transform.FindChild("AttackPoint").gameObject;
            transform.DetachChildren();
            Component.Destroy(gameObject.GetComponent<FoQus>());
        }
        else
        {
            masterGameObject = gameObject;
            Qlick.RIGHTQLICK += Qlick_RIGHTQLICK;
            Qlick.LEFTQLICK += Qlick_LEFTQLICK;
        }
    }

    void Qlick_LEFTQLICK(Ray qamRay, bool hold)
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
                            UNIT.Options.FoqussedLeftOnEnemy(ClickedUnit); 
                        }
                        else if (IsOtherUnit(ClickedUnit))
                        {
                            Marker[(int)MARKERS.WayPoint].GetComponent<Follower>().targetTransform = ClickedUnit.transform;
                            Marker[(int)MARKERS.WayPoint].GetComponent<FaceDirection>().TransformToFace = gameObject.transform;
                            Marker[(int)MARKERS.WayPoint].renderer.enabled = true;
                            UNIT.Options.FoqussedLeftOnAllied(ClickedUnit);
                        }
                    }
                    else
                    {
                        Marker[(int)MARKERS.MoveToPoint].transform.position = groundHit;
                        Marker[(int)MARKERS.MoveToPoint].renderer.enabled = true;
                        UNIT.Options.FoqussedLeftOnGround(groundHit);
                    }
                }
            }
        }
    }

    void Qlick_RIGHTQLICK(Ray qamRay, bool hold)
    {
        if (!IsLocked)
        {
            if (!hold)
            {
                GameObject ClickedUnit = RayHittenUnit(qamRay);
                if (ClickedUnit)
                {
                    if (IsOtherUnit(ClickedUnit)) ClickedUnit.AddComponent<FoQus>();
                    else RightClickMenu.PopUpGUI(UNIT);
                }
                else GameObject.Destroy(gameObject.GetComponent<FoQus>());
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
        return otherUnit.GetComponent<UnitSqript>().GoodOrEvil != this.UNIT.GoodOrEvil;
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
            Component.Destroy(gameObject.GetComponent<FoQus>());
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
            Qlick.RIGHTQLICK -= Qlick_RIGHTQLICK;
            Qlick.LEFTQLICK -= Qlick_LEFTQLICK;
        }
        else firststart = false;
    }

}
