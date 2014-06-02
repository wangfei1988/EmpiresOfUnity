using UnityEngine;
using System.Collections;

public class ReleasePoint : MonoBehaviour 
{
    public Transform lTransform;
    public UnitScript UNIT;

	void Start () 
    {
        UNIT = this.transform.parent.gameObject.GetComponent<UnitScript>();
	}

    public Vector3 Release()
    {
         UNIT.GetComponent<Animator>().SetBool(this.gameObject.name, false);
         return this.gameObject.transform.TransformPoint(this.transform.localPosition);
    }
}
