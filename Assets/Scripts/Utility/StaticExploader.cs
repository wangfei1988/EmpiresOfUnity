using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticExploader : MonoBehaviour 
{
    // An Aray of Instances Of Exploasions...
   public GameObject[] Explosions = new GameObject[2];


   public AudioClip[] audioClips = new AudioClip[2];

  //list of Ordered Explosions for next frame...
   private static Dictionary<Vector3, ExplosionAudioClipPair> ExploadingExplosions = new Dictionary<Vector3, ExplosionAudioClipPair>();

   public GameObject ExplosionAudioObject;

   void Start()
   {
       UpdateManager.WEAPONUPDATES+=UpdateManager_WEAPONUPDATES;
       ExplosionAudioObject = this.transform.FindChild("ExplosionAudioSource").gameObject;
   }

   public static void Exploade(int explosionID, Vector3 location)
   {
       ExploadingExplosions.Add(location,new ExplosionAudioClipPair(explosionID,-2));
   }
   public static void Exploade(int explosionID, Vector3 location, int audioID)
   {
       ExploadingExplosions.Add(location,new ExplosionAudioClipPair( explosionID,audioID));
   }
   public static void Exploade(Vector3 location, int audioID)
   {
       ExploadingExplosions.Add(location, new ExplosionAudioClipPair(-1, audioID));
   }
   
    private void UpdateManager_WEAPONUPDATES()
   {
       int explosionID,audioID;
       foreach (Vector3 explosionLocation in ExploadingExplosions.Keys)
       {
           if ((explosionID = ExploadingExplosions[explosionLocation].Explosion) >= 0)
           {

               Explosions[explosionID].transform.position = explosionLocation;
               Explosions[explosionID].particleSystem.Play();
           }
           if ((audioID = ExploadingExplosions[explosionLocation].Audio) >= 0)
           {
               ExplosionAudioObject.transform.position = explosionLocation;
               ExplosionAudioObject.audio.PlayOneShot(audioClips[audioID]);
           }
       }
       ExploadingExplosions.Clear();
   }

   public struct ExplosionAudioClipPair
   {
       public int Explosion, Audio;
       public ExplosionAudioClipPair(int explosionID, int audioID)
       {
           Explosion = explosionID;
           Audio = audioID;
       }
   }
}
