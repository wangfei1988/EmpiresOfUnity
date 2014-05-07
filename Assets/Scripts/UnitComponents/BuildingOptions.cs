using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[AddComponentMenu("Character/Unit Options (Building)")]
public class BuildingOptions : UnitOptions
{
    new public enum OPTIONS : int
    {
        Produce=EnumProvider.ORDERSLIST.Produce,
        StopProduction=EnumProvider.ORDERSLIST.StopProduction,
    }
    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get { return EnumProvider.UNITCLASS.BUILDING; }
    }
   
    public OPTIONS unitState;
     string[] fabrikatNames;

     internal override string[] GetUnitsMenuOptions()
     {
         return fabrikatNames;
     }

     public override void GiveOrder(int orderNumber)
     {
         if (orderNumber < fabrikatNames.Length - 1)
         {
             CurrentFabrikat = orderNumber;
             UnitState = OPTIONS.Produce;
         }
         else UnitState = OPTIONS.StopProduction;
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
            return  unitState;
        }
        set
        {
            unitState = (OPTIONS)value;
            switch (unitState)
                {
                    case OPTIONS.Produce:
                        {
                            GameObject.Instantiate(Fabrikat[CurrentFabrikat], Fabrikat[CurrentFabrikat].transform.position, Fabrikat[CurrentFabrikat].transform.rotation);
                            break;
                        }
                    case OPTIONS.StopProduction:
                        {
                            break;
                        }
                }
        }
    }


    private int CurrentFabrikat;

    public string typename;
    public List<GameObject> Fabrikat;




    internal override void MoveAsGroup(GameObject leader)
    {
        
    }

    internal override void DoStart()
    {

        fabrikatNames = new string[Fabrikat.Count + 1];


        for (int i = 0; i < Fabrikat.Count; i++) fabrikatNames[i] = Fabrikat[i].name;
        fabrikatNames[Fabrikat.Count] = "StopProduction";
        UnitState = unitState = OPTIONS.StopProduction;
    }

    
    
    internal override void DoUpdate()
    {

    }
}
