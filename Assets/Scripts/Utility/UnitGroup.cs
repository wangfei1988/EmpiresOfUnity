using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGroup : ScriptableObject
{
    public enum GROUPSTATE : byte
    {
        None,
        Ready,
        UnderConstruction,
        Moving,
        Attacking,
        Guarding,
        Building,
        Waiting
    }
    private GROUPSTATE groupState;
    public GROUPSTATE GroupState
    {
        get
        {
            return groupState;
        }
        set
        {
            if (groupState != value)
            {
                if (value == GROUPSTATE.Waiting)
                    foreach (GameObject groupMember in MemberUnit) groupMember.GetComponent<UnitOptions>().GiveOrder(groupMember.GetComponent<UnitOptions>().GetUnitsMenuOptions().Length - 2);
            }
            groupState = value;
        }
    }
    public GameObject gameObject;
    private List<GameObject> MemberUnit = new List<GameObject>(0);
    private Vector3 position = Vector3.zero;
 //   private Vector2 size = Vector2.zero;
    public Vector3 Scale
    {
        get;
        private set;
    }
    public Rect groupRect;
    public int Count;
    public FoE.GOODorEVIL GoodOrEvil;
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
    public GameObject this[int index]
    {
        get { return MemberUnit[index]; }
    }


    public void CalculateSize()
    {
        if (MemberUnit.Count > 0)
        {
            Vector2 XY_max;
            Vector2 XY_min;
            XY_max = XY_min = new Vector2(MemberUnit[0].transform.position.x, MemberUnit[0].transform.position.z);
            foreach (GameObject member in MemberUnit)
            {
                if (XY_max.x > member.transform.position.x) XY_max.x = member.transform.position.x;
                if (XY_max.y > member.transform.position.y) XY_max.y = member.transform.position.y;
                if (XY_min.x < member.transform.position.x) XY_min.x = member.transform.position.x;
                if (XY_min.y < member.transform.position.y) XY_min.y = member.transform.position.y;
            }
            groupRect = new Rect(XY_min.x, XY_min.y, (XY_max.x - XY_min.x), (XY_min.y - XY_max.y));
            position = new Vector3(groupRect.center.x,0.5f,groupRect.center.y);
            Scale = new Vector3(groupRect.width,groupRect.height,1f);
        }
    }
    public void AddUnit(GameObject newGroupMember)
    {
            if (MemberUnit.Count == 0)
            {
                this.GoodOrEvil = newGroupMember.GetComponent<UnitScript>().GoodOrEvil;
                position = newGroupMember.transform.position;
            }
        if (newGroupMember.GetComponent<UnitScript>().GoodOrEvil == this.GoodOrEvil)
        {
            MemberUnit.Add(newGroupMember);
            if (GroupState != GROUPSTATE.UnderConstruction) CalculateSize();
        }
        Count = MemberUnit.Count;
    }
    public void AddUnits(List<GameObject> units)
    {
        BeginGroupFill(units[0].GetComponent<UnitScript>().GoodOrEvil);
        fillGroup(units);
        EndGroupFill();
    }
    public void NewGroup(List<GameObject> units)
    {
        ResetGroup();
        AddUnits(units);
    }
    public void BeginGroupFill(FoE.GOODorEVIL side)
    {
        if (this.GoodOrEvil != side) ResetGroup();
        GroupState = GROUPSTATE.UnderConstruction;
        GoodOrEvil = side; 
    }
    public void BeginGroupFill(GameObject firstUnit)
    {
        GroupState = GROUPSTATE.UnderConstruction;
        AddUnit(firstUnit);
    }
    public void EndGroupFill()
    {

        CalculateSize();
        Count = MemberUnit.Count;
        GroupState = GROUPSTATE.Ready;
    }
    internal void fillGroup(GameObject unit)
    {
        if (GroupState == GROUPSTATE.UnderConstruction)
            if (unit.GetComponent<UnitScript>().GoodOrEvil == this.GoodOrEvil)
                MemberUnit.Add(unit);
    }
    private void fillGroup(List<GameObject> units)
    {
        for (int i = units.Count-1; i >= 0; i++)
        {
            if (units[i].GetComponent<UnitScript>().GoodOrEvil != this.GoodOrEvil) units.RemoveAt(i);
        }
        MemberUnit.AddRange(units);
    }
    public void ResetGroup()
    {
        GroupState = GROUPSTATE.UnderConstruction;
            MemberUnit.Clear();
            Count = 0;
    }

    public void startGroup()
    {
        foreach (GameObject rectangle in GameObject.FindGameObjectsWithTag("Rectangles"))
        {
            if (rectangle.GetComponent<GroupRectangleScript>())
            {
                gameObject = rectangle;
                gameObject.GetComponent<GroupRectangleScript>().SetToGUI(GUIScript.main.GetComponent<GUIScript>());
            }
        }


    }

    public void GroupedLeftOnGround()
    {
        for (int i = 1; i < MemberUnit.Count; i++)
            MemberUnit[i].GetComponent<UnitOptions>().MoveAsGroup(MemberUnit[0]);
        MemberUnit[0].GetComponent<UnitScript>().Options.FocussedLeftOnGround(MouseEvents.State.Position.AsWorldPointOnMap);
        GroupState = GROUPSTATE.Moving;
    }
    public void GoupedLeftOnEnemy(GameObject enemy)
    {
        foreach (GameObject unit in MemberUnit)
            unit.GetComponent<UnitScript>().Options.FocussedLeftOnEnemy(enemy);
    }
}
