using UnityEngine;
using System.Collections;

public class BlibContainer : MonoBehaviour
{
   public static GameObject UnitBlip;
   public static GameObject BuildingBlip;
   public static GameObject EnemyUnitBlip;
   public static GameObject EnemyBuildingBlip;

   [SerializeField]
   GameObject[] blibloader;

   void Awake()
   {
       UnitBlip = blibloader[0];
       BuildingBlip = blibloader[1];
       EnemyUnitBlip = blibloader[2];
       EnemyBuildingBlip = blibloader[3];
   }
}
