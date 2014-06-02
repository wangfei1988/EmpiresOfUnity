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
        if (this.GetComponent<Animator>())
        {
            this.GetComponent<Animator>().SetBool("Release", false);
            GetComponent<Animator>().enabled=true;
        }
    }

    internal override void DoUpdate()
    {
        //todo produce by timer...
    }
    
     public OPTIONS unitState;

     string[] fabrikatNames;

     void OnFabrikatReleased()
     {
         this.GetComponent<Animator>().SetBool("Release", false);

         GameObject TheNewOne = (GameObject.Instantiate(CurrentFabrikat, this.transform.GetChild(1).gameObject.GetComponent<releasePoin>().TakenOff(), Quaternion.LookRotation(Vector3.forward)) as GameObject);
         TheNewOne.GetComponent<Movability>().Throttle=0.85f;
         TheNewOne.GetComponent<Movability>().MoveToPoint=this.MoveToPoint;
         TheNewOne.GetComponent<Movability>().MovingDirection = this.MoveToPoint;
         TheNewOne.GetComponent<Movability>().IsMoving=true;
         TheNewOne=null;
     }
     private int[] fabrikatReleaseHashes = new int[0];
     private void ReleaseFabrikat(Object release)
     {
         if (this.GetComponent<Animator>())
         {
             
             this.GetComponent<Animator>().SetBool("Release", true);
         }

         else
             OnFabrikatReleased();
     }

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
            if (System.Enum.IsDefined(typeof(OPTIONS), (int)baseUnitState))
                return unitState;
            else
                return base.UnitState;
        }
        set
        {
            unitState = (OPTIONS)value;
            if (System.Enum.IsDefined(typeof(OPTIONS), unitState))
            {
                
                switch (unitState)
                {
                    case OPTIONS.Produce:
                        {
                            ReleaseFabrikat(CurrentFabrikat);
                            
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
                            MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
                            break;
                        }
                }
                baseUnitState = (EnumProvider.ORDERSLIST)value;
            }
            base.UnitState = value;
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
        MoveToPoint = MouseEvents.State.Position.AsWorldPointOnMap;
     //   UnlockAndDestroyFocus();
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

