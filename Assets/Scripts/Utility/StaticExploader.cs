using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticExploader : MonoBehaviour 
{
    // An Aray of Instances Of Exploasions...
   public GameObject[] Explosions = new GameObject[2];


   public AudioClip[] audioClips = new AudioClip[2];

  //list of Ordered Explosions for next frame...
   private static List<ExplosionAudioClipPair> ExploadingExplosions = new List<ExplosionAudioClipPair>();

   public GameObject ExplosionAudioObject;

   void Start()
   {
       UpdateManager.OnUpdate+=UpdateManager_On_LASTUPDATES;
       ExplosionAudioObject = this.transform.FindChild("ExplosionAudioSource").gameObject;
   }

   public static void Exploade(int explosionID, Vector3 location)
   {
       ExploadingExplosions.Add(new ExplosionAudioClipPair(location,explosionID, -2));
   }
   public static void Exploade(int explosionID, Vector3 location, int audioID)
   {
       ExploadingExplosions.Add(new ExplosionAudioClipPair(location, explosionID,audioID));
   }
   public static void Exploade(Vector3 location, int audioID)
   {
       ExploadingExplosions.Add(new ExplosionAudioClipPair(location, -1, audioID));
   }
   
    private void UpdateManager_On_LASTUPDATES()
   {
       foreach (ExplosionAudioClipPair explosion in ExploadingExplosions)
       {
           if (explosion.Explosion >= 0)
           {
               Explosions[explosion.Explosion].transform.position = explosion.Position;
               Explosions[explosion.Explosion].particleSystem.Play();
           }
           if (explosion.Audio >= 0)
           {
               ExplosionAudioObject.transform.position = explosion.Position;
               ExplosionAudioObject.audio.PlayOneShot(audioClips[explosion.Audio]);
           }
       }
       ExploadingExplosions.Clear();
   }

   public struct ExplosionAudioClipPair
   {
       public Vector3 Position;
       public int Explosion, Audio;
       public ExplosionAudioClipPair(Vector3 position , int explosionID, int audioID)
       {
           Position = position;
           Explosion = explosionID;
           Audio = audioID;
       }
   }
}
