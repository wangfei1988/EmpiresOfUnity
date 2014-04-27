using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[AddComponentMenu("Character/Unit Options (Building)")]
public class BuildingOptions : UnitOptions
{
    new public enum OPTIONS : int
    {
        Produce,
        StopProduction
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

    internal override void Hit(int power)
    {
        
    }
    private int CurrentFabrikat;

    public string typename;
    public List<GameObject> Fabrikat;


    public override void SetMoveToPoint(Vector3 point)
    {

    }

    internal override void MoveAsGroup(GameObject leader)
    {
        
    }

    void Start()
    {
        
 
        fabrikatNames = new string[Fabrikat.Count+1];

        Life = 1000;

        for (int i = 0; i < Fabrikat.Count; i++) fabrikatNames[i]=Fabrikat[i].name;
        fabrikatNames[Fabrikat.Count]="StopProduction";
        UnitState = unitState = OPTIONS.StopProduction;

    }
    
    internal override void DoUpdate()
    {

    }
}
