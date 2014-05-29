using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[AddComponentMenu("Program-X/UNIT/UnitOptions (Production Building)")]
public class ProductionBuildingOptions : UnitOptions
{
    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get { return EnumProvider.UNITCLASS.PRODUCTION_BUILDING; }
    }

    new public enum OPTIONS : int
    {
        Produce = EnumProvider.ORDERSLIST.Produce,
        StopProduction = EnumProvider.ORDERSLIST.StopProduction,
        MoveUnitsTo = EnumProvider.ORDERSLIST.MoveUnitsTo,
    }

    internal override void DoStart()
    {
        //UNIT.Settings = ScriptableObject.CreateInstance(typename) as BuildingSetting;
        fabrikatNames = new string[Fabrikat.Count + 1];
        foreach (int option in System.Enum.GetValues(typeof(OPTIONS)))
            if (!OptionalStatesOrder.ContainsKey(option)) OptionalStatesOrder.Add(option, ((OPTIONS)option).ToString());
        for (int i = 0; i < Fabrikat.Count; i++) fabrikatNames[i] = Fabrikat[i].name;
        fabrikatNames[Fabrikat.Count] = "StopProduction";
        UnitState = unitState = OPTIONS.StopProduction;
        MoveToPoint = new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z - 5f);
        CurrentFabrikatNumber = 0;
        CurrentFabrikat = Fabrikat[CurrentFabrikatNumber];
    }

    internal override void DoUpdate()
    {
        //todo produce by timer...
    }
    
     public OPTIONS unitState;

     string[] fabrikatNames;



     internal override Object[] GetUnitsSIDEMenuObjects()
     {
         if (UNIT.weapon.HasArsenal)
         {

             Object[] objBuffer = new Object[Fabrikat.Count + UNIT.weapon.arsenal.Count];
             int index = 0;
             
             // Add buildable Buildings
             foreach (Object fabrikat in Fabrikat)
                 objBuffer[index++] = fabrikat;

             // Add Weapon Arsenal
             for (int i = 0; i < UNIT.weapon.arsenal.Count; i++)
                 objBuffer[index + i] = UNIT.weapon.arsenal[i];

             return objBuffer;
         }
         return Fabrikat.ToArray();
     }

     public override void GiveOrder(int orderNumber)
     {
         int i = -1;
         foreach (var entry in OptionalStatesOrder)
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
         CurrentFabrikatNumber = SIDEoptionNumber;
         UnitState = (OPTIONS)0;

     }

     internal override void SetSIDEObject(Object returned)
     {
         if (returned is WeaponObject)
             UNIT.weapon.prefabSlot = returned as WeaponObject;
         else
         {
             CurrentFabrikat = returned;
             UnitState = EnumProvider.ORDERSLIST.Produce;
         }
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
						GameObject.Instantiate(CurrentFabrikat, MoveToPoint, (CurrentFabrikat as GameObject).transform.rotation);
                        // TODO Let they Spawn within the Building and then let they so to "MoveToPoint"
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

    internal override void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        MoveToPoint = MouseEvents.State.Position.AsWorldPointOnMap;
        MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
        UnlockFocus();
    }
   
    internal override void FocussedLeftOnGround(Vector3 worldPoint)
    {
        UnlockAndDestroyFocus();
        //DestroyFocus();
    }

    internal override void MoveAsGroup(GameObject leader)
    {
        
    }

    internal override void FocussedLeftOnEnemy(GameObject enemy)
    {
        if (GetComponent<Attackability>())
        {
            UNIT.Options.Target = enemy;
            GetComponent<Attackability>().AttackPoint = enemy.transform.position;
            standardOrder = true;
            UnitState = EnumProvider.ORDERSLIST.Attack;

            standardOrder = false;
        }
        else
            base.FocussedLeftOnEnemy(enemy);
    }

    public int CurrentFabrikatNumber;
    public Object CurrentFabrikat;
    public string typename;
    public List<Object> Fabrikat;

}

