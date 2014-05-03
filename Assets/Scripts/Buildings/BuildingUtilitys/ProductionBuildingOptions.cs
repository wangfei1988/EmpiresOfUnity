using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[AddComponentMenu("Character/Unit Options (Production Building)")]
public class ProductionBuildingOptions : UnitOptions
{
    new public enum OPTIONS : int
    {
        Produce = EnumProvider.ORDERSLIST.Produce,
        StopProduction = EnumProvider.ORDERSLIST.StopProduction,
        MoveUnitsTo = EnumProvider.ORDERSLIST.MoveUnitsTo,
    }

    internal override void DoStart()
    {
        fabrikatNames = new string[Fabrikat.Count + 1];
        foreach (int option in System.Enum.GetValues(typeof(OPTIONS)))
            if (!OPTIONSlist.ContainsKey(option)) OPTIONSlist.Add(option, ((OPTIONS)option).ToString());
        for (int i = 0; i < Fabrikat.Count; i++) fabrikatNames[i] = Fabrikat[i].name;
        fabrikatNames[Fabrikat.Count] = "StopProduction";
        UnitState = unitState = OPTIONS.StopProduction;
        MoveToPoint = new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z - 10f);
        CurrentFabrikat = 0;
    }

     public OPTIONS unitState;
     string[] fabrikatNames;



     internal override string[] GetUnitsSIDEMenuOptions()
     {
         return fabrikatNames;
     }

     public override void GiveOrder(int orderNumber)
     {
         int i = -1;
         foreach (var entry in OPTIONSlist)
         {
             if (++i == orderNumber)
             {
                 UnitState = (OPTIONS)entry.Key;
                 return;
             }
         }
     }
     public override void SetSIDEOption(int SIDEoptionNumber)
     {
         CurrentFabrikat = SIDEoptionNumber;
         UnitState = (OPTIONS)0;
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
					{
						
						GameObject.Instantiate(Fabrikat[CurrentFabrikat], MoveToPoint, Fabrikat[CurrentFabrikat].transform.rotation);
						break;
					}
				case OPTIONS.StopProduction:
					{
						break;
					}
				case OPTIONS.MoveUnitsTo:
					{
						LockOnFocus();
						MouseEvents.LEFTCLICK+=MouseEvents_LEFTCLICK;
						break;
					}
			}
        }
    }

    protected override void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        MoveToPoint = MouseEvents.State.Position.AsWorldPointOnMap;
        MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
        UnlockFocus();
    }
   
    internal override void FocussedLeftOnGround(Vector3 worldPoint)
    {
        UnlockFocus();
        Component.Destroy(gameObject.GetComponent<Focus>());       
    }

    internal override void MoveAsGroup(GameObject leader)
    {
        
    }

    private int CurrentFabrikat;

    public string typename;
    public List<GameObject> Fabrikat;

    
    internal override void DoUpdate()
    {
        //todo  produce by timer...

    }
}

