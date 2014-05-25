using UnityEngine;
using System.Collections;

public class Thruster : MonoBehaviour {

    public UnitScript UNIT
    {
        get {return this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<UnitScript>(); }
    }
    public float Throttle
    {
        get { return this.GetComponent<Animator>().GetFloat("Throttle"); }
        set { this.GetComponent<Animator>().SetFloat("Throttle", value); }
    }

	void Start () 
    {
        UpdateManager.UNITUPDATE+=UpdateManager_UNITUPDATE;
	}
    public float landingHeight=10;
    void UpdateManager_UNITUPDATE()
    {
        Throttle = UNIT.GetComponent<Movability>().Throttle;
        Vector3 position = this.transform.position;
        position.y = (15 * Throttle)+landingHeight;
        Debug.Log(position.ToString());
        this.transform.position = position;
        this.transform.position += UNIT.GetComponent<Movability>().Speed * this.transform.forward;
    }


}
