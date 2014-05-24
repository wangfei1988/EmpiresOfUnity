using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticExploader : MonoBehaviour 
{
    // An Aray of Instances Of Exploasions...
   public GameObject[] Explosions = new GameObject[1];


    //A list of matching audioclips comes next....

  //list of Ordered Explosions for next frame...
   private static Dictionary<Vector3, int> ExploadingExplosions = new Dictionary<Vector3,int>();

   void Start()
   {
       UpdateManager.WEAPONUPDATES+=UpdateManager_WEAPONUPDATES;
   }

   public static void Exploade(int explosionID, Vector3 location)
   {
       ExploadingExplosions.Add(location, explosionID);
   }

   private void UpdateManager_WEAPONUPDATES()
   {
       foreach (Vector3 explosionLocation in ExploadingExplosions.Keys)
       {
           Explosions[ExploadingExplosions[explosionLocation]].transform.position = explosionLocation;
           Explosions[ExploadingExplosions[explosionLocation]].particleSystem.Play();
       }
       ExploadingExplosions.Clear();
   }

}
