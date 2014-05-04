using UnityEngine;
using System.Collections;

public class TimedObjectDestructorCS : MonoBehaviour {

	public float timeOut = 1f;
	public bool detachChildren = false;
    private Vector3 randomDirection;
    public bool Exploadet = false;

    void Start () 
    {
        if (!detachChildren) 
            UpdateManager.WEAPONUPDATES += UpdateManager_WEAPONUPDATES;
        Invoke("DestroyNow", timeOut);
        randomDirection = UnityEngine.Random.insideUnitSphere;
	}

    void UpdateManager_WEAPONUPDATES()
    {
        if (Exploadet) this.gameObject.transform.position += randomDirection * (timeOut - 8f);
    }

    void DestroyNow()
    {
        if (detachChildren)
        {

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<TimedObjectDestructorCS>().enabled = true;
                transform.GetChild(i).GetComponent<TimedObjectDestructorCS>().Exploadet = true;
            }
            
            GameObject.Destroy(transform.FindChild("Emission").gameObject);
            transform.DetachChildren();
        }
        else
            UpdateManager.WEAPONUPDATES -= UpdateManager_WEAPONUPDATES;

        GameObject.DestroyObject(gameObject);
    }
    public void DestroyImmideate()
    {
        DestroyNow();
    }
    void OnDestroy()
    {

    }


}


