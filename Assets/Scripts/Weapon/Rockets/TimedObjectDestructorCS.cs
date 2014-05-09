using UnityEngine;
using System.Collections;

/*
public class TimedObjectDestructorCS : MonoBehaviour {

	public float timeOut = 1f;
	public bool detachChildren = false;
    private Vector3 randomDirection;
    public bool Exploadet = false;

    void Start () 
    {
        Invoke("DestroyNow", timeOut);
        randomDirection = UnityEngine.Random.insideUnitSphere;
	}

    void DestroyNow()
    {
        if (detachChildren)
        {
            GameObject.Destroy(transform.FindChild("Emission").gameObject);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<TimedObjectDestructorCS>().Exploadet = true;
            }
            transform.DetachChildren();
        }


        DestroyObject(gameObject);
    }

    void Update()
    {
        if (Exploadet) this.gameObject.transform.position += randomDirection * (timeOut-8f);
    }


}

*/
