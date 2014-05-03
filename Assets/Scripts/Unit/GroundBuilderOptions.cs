using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Character/Unit Options (Standard GroundBuilder)")]
public class GroundBuilderOptions : MovingUnitOptions 
{


   new public enum OPTIONS : int
    {
        Build = EnumProvider.ORDERSLIST.Build,
        Repaire = EnumProvider.ORDERSLIST.Repaire,
    }

   public List<GameObject> BuildableBuildings = new List<GameObject>();

   internal override string[] GetUnitsSIDEMenuOptions()
   {
       int index= -1;
       string[] buffer = new string[BuildableBuildings.Count];
       foreach (var buildableDing in BuildableBuildings)
           buffer[++index] = buildableDing.GetComponent<UnitScript>().unitType.ToString();
       return buffer;
   }
   
   private int currentBuildableDing = 0;
   public GameObject BuildingUnderConstruction;
    private bool UnderConstruction=false;
    internal override void DoStart()
    {
        base.DoStart();
        foreach (int option in System.Enum.GetValues(typeof(OPTIONS)))
            if (!OPTIONSlist.ContainsKey(option)) OPTIONSlist.Add(option, ((OPTIONS)option).ToString());
        UnitState = unitState;
        IsMoving = true;
    }
    protected override bool GotToDoPrimaryOrders
    {
        get
        {
            return !standardOrder;
        }
        set
        {

        }
    }
    private OPTIONS unitState;
    public override System.Enum UnitState
    {
        get
        {
            if (System.Enum.IsDefined(typeof(OPTIONS), (OPTIONS)unitstateint))
                return unitState;
            else return base.UnitState;
        }
        set
        {
            OPTIONS order;
            if (System.Enum.IsDefined(typeof(OPTIONS), (OPTIONS)value))
            {
                order = (OPTIONS)value;
                if (unitstateint != (int)order)
                {

                    if (!standardOrder)
                    {
                        switch (order)
                        {
                            case OPTIONS.Build:
                                {
                                     MouseEvents.LEFTCLICK+=MouseEvents_LEFTCLICK;
                                     LockOnFocus();
                                    break;
                                }
                            case OPTIONS.Repaire:
                                {
                                    MouseEvents.LEFTCLICK+=MouseEvents_LEFTCLICK;
                                    LockOnFocus();
                                    break;
                                }
                        }
                    }
                    unitstateint = (int)order;
                    unitState = order;
                }
            }
            else base.UnitState = value;
        }
    }

    public override void SetSIDEOption(int SIDEoptionNumber)
    {
        currentBuildableDing = SIDEoptionNumber;
        UnitState = OPTIONS.Build;
    }

    protected override void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        if (!hold)
        {
            if (!gameObject.GetComponent<Focus>())
            {
                if (standardOrder)
                    gameObject.AddComponent<Focus>();
            }
            else
            {
                if (!standardOrder) base.MouseEvents_LEFTCLICK(qamRay, hold);

                if ((OPTIONS)UnitState == OPTIONS.Build)
                {
                    ConstructionAreaCenter = standardOrder ? ActionPoint ?? gameObject.transform.position : MouseEvents.State.Position.AsWorldPointOnMap;
                    if (!standardOrder)
                    {
                        MoveToPoint = ConstructionAreaCenter + Random.insideUnitSphere * 10f;
                        Orderble[] MoveToBuildADing = new Orderble[2];
                        MoveToBuildADing[0] = new Orderble(EnumProvider.ORDERSLIST.MoveTo, MoveToPoint, 0);
                        MoveToBuildADing[1] = new Orderble(EnumProvider.ORDERSLIST.Build, ConstructionAreaCenter, currentBuildableDing);
                        ChainedOrders.AddRange(MoveToBuildADing);
                        MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
                    }
                    else
                    {
                        gameObject.transform.up = (ConstructionAreaCenter - gameObject.transform.position).normalized;
                        BuildingUnderConstruction = GameObject.Instantiate(BuildableBuildings[CurrentSIDEMENUoption], ActionPoint ?? gameObject.transform.position, BuildableBuildings[CurrentSIDEMENUoption].transform.rotation) as GameObject;
                        UnderConstruction = true;
                    }
                    UnlockFocus();
                    if (standardOrder) standardOrder = false;
                }
                else if (UnitState == (System.Enum)OPTIONS.Repaire)
                {

                }
            }

        }
    }
    protected override void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {
        base.MouseEvents_RIGHTCLICK(qamRay, hold);
    }
    protected bool someLittleTasksThenDone;
    protected override bool ProcessAllOrders()
    {
        someLittleTasksThenDone = base.ProcessAllOrders();
        if (stillWorkDoDo)
        {
            if (ActionPoint != null) MouseEvents_LEFTCLICK(MouseEvents.State.Position.AsRay,false);
        }
        return someLittleTasksThenDone;
    }

    internal override void MoveAsGroup(GameObject leader)
    {

    }

    internal override void FocussedRightOnGround(Vector3 worldPoint)
    {
        
    }
    internal override void FocussedLeftOnAllied(GameObject friend)
    {
        //todo---------------------------repair
    }
    internal override void FocussedLeftOnEnemy(GameObject enemy)
    {
        
    }
    public Vector3 ConstructionAreaCenter = Vector3.zero;

    private bool waitngToArive = false;
    public bool IsBuildingABuildDing
    {
        get { return (bool)BuildingUnderConstruction; }
        set { if (!value) { BuildingUnderConstruction = null; UnderConstruction = value; } }
    }
    private int BuildingPowerFactor=0;
    private bool BuildDing()
    {
        if (!IsMoving)
        {

            

            if (UnderConstruction)
            {

                System.Random rand = new System.Random(System.DateTime.Now.ToUniversalTime().Millisecond);
                int d = 0;

                if (--BuildingPowerFactor == 0)
                {
                    BuildingPowerFactor = rand.Next(10);
                    d = rand.Next(4);
                }
                int bonus = d * BuildingPowerFactor;
                switch (d)
                {
                    case 0:
                        gameObject.transform.position += gameObject.transform.right * bonus / 10;
                        break;
                    case 1:
                        gameObject.transform.position += -gameObject.transform.right * bonus / 10;
                        break;
                    case 2:
                        gameObject.transform.position += gameObject.transform.forward * bonus / 10;
                        break;
                    case 3:
                        gameObject.transform.position += -gameObject.transform.forward * bonus / 10;
                        break;
                }

                BuildingUnderConstruction.GetComponent<UnitScript>().RandomBuildingBonus(bonus);
                UnderConstruction = ((bool)gameObject.GetComponent<BuildingsGrower>());
            }


            return ((bool)gameObject.GetComponent<BuildingsGrower>());
        }
        return true;
    }
  
 

    internal override void DoUpdate()
    {
        base.DoUpdate();
        if (IsBuildingABuildDing) IsBuildingABuildDing = BuildDing();
    }
}
