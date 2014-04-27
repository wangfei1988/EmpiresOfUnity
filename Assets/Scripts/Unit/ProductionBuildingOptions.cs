using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[AddComponentMenu("Character/Unit Options (Production Building)")]
public class ProductionBuildingOptions : UnitOptions
{
    new public enum OPTIONS : int
    {
        Produce,
        LaunchRocket,
        StopProduction
    }
    string[] fabrikatNames;

    private int CurrentFabrikat;
    public string typename;
    public List<GameObject> Fabrikat;
    public List<Texture> menuButtons;

    /* Start & Update */
    void Start()
    {
        fabrikatNames = new string[Fabrikat.Count + 1];
        Life = 1000;
        for (int i = 0; i < Fabrikat.Count; i++)
            fabrikatNames[i] = Fabrikat[i].name;
        fabrikatNames[Fabrikat.Count] = "StopProduction";
        UnitState = unitState = OPTIONS.StopProduction;
        MoveToPoint = new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z - 120f);
    }

    internal override void DoUpdate()
    {

    }

    /* Methods */
    internal override string[] GetUnitsMenuOptions()
    {
        return fabrikatNames;
    }

    internal Texture[] GetButtons()
    {
        return menuButtons.ToArray();
    }

    public override void GiveOrder(int orderNumber)
    {
        if (orderNumber < fabrikatNames.Length - 1)
        {
            CurrentFabrikat = orderNumber;
            UnitState = (OPTIONS)orderNumber;
        }
        else
        {
            UnitState = OPTIONS.StopProduction;
        }
    }

    internal override void Hit(int power)
    {
         
    }

    public OPTIONS unitState;
    public override System.Enum UnitState
    {
        get
        {
            return unitState;
        }
        set
        {
            unitState = (OPTIONS)value;
            switch (unitState)
            {
                case OPTIONS.Produce:
                    GameObject.Instantiate(Fabrikat[CurrentFabrikat], MoveToPoint, Fabrikat[CurrentFabrikat].transform.rotation);
                    break;
                case OPTIONS.LaunchRocket:
                    LockOnFocus();
                    MouseEvents.LEFTCLICK+=MouseEvents_LEFTMouseEvents;
                    break;
                case OPTIONS.StopProduction:
                     break;
            }
        }
    }

    internal override void FocussedLeftOnGround(Vector3 worldPoint)
    {
        SetMoveToPoint(worldPoint);
    }

    internal override void MoveAsGroup(GameObject leader)
    {
        
    }

    public override void SetMoveToPoint(Vector3 point)
    {
        MoveToPoint = point;
    }

    protected override void MouseEvents_LEFTMouseEvents(Ray qamRay, bool hold)
    {
        if (!hold)
        {
            if (unitState == OPTIONS.LaunchRocket)
            {
                GetComponent<Weapon>().Engage(MouseEvents.State.Position.AsWorldPointOnMap);
                UnlockFocus();
                MouseEvents.LEFTCLICK -= MouseEvents_LEFTMouseEvents;
            }
        }
    }
}

