using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SoundFactory : MonoBehaviour {

    /* Audio */
    private AudioSource audioSource;
    private List<GameObject> objList = new List<GameObject>();

    void Start()
    {
        UpdateManager.GUIUPDATE += DoUpdate;
        this.audioSource = this.gameObject.AddComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void DoUpdate () {
	
	}

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound, 1f);
    }
}
