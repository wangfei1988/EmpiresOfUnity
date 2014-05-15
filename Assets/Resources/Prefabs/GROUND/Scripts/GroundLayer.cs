using UnityEngine;
using System.Collections;

public class GroundLayer : MonoBehaviour {

    public Vector2 groundSize
    {
        get;
        private set;
    }
    new public Collider collider;
    public bool lightSwitch = false;
    public bool IsActiveGround
    {
        get
        {
            return this.light.enabled = lightSwitch;
        }
        set
        {
            this.light.enabled = lightSwitch = value;
        }
    }
    new public Light light;
    public Ground Controll;

    void Awake()
    {
        if (this.gameObject.transform.GetChild(0).gameObject.GetComponent<TerrainCollider>())
            groundSize = new Vector2(this.gameObject.transform.GetChild(0).GetComponent<Terrain>().terrainData.size.x, this.gameObject.transform.GetChild(0).GetComponent<Terrain>().terrainData.size.z);
        else
            groundSize = new Vector2(this.gameObject.transform.GetChild(0).lossyScale.x, this.gameObject.transform.GetChild(0).lossyScale.z);
    }

	void Start () 
    {
        Controll = this.gameObject.transform.parent.gameObject.GetComponent<Ground>();
        collider = this.gameObject.transform.GetChild(0).gameObject.collider;
        light = this.gameObject.transform.GetChild(0).transform.FindChild("GroundLight").light;
        IsActiveGround = false;
	}
	


}
