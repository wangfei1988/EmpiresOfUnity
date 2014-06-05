using UnityEngine;
using System.Collections;

public class GroundLayer : MonoBehaviour {
    
    public Vector2 groundSize
    {
        get;
        private set;
    }
    new public Collider collider;
 //   new public Rigidbody rigidbody;
    public bool lightSwitch = false;
    public bool IsActiveGround
    {
        get
        {
            return this.light.enabled = lightSwitch;
        }
        set
        {
            SetGroundVisible(value);
            this.light.enabled = lightSwitch = value;
        }
    }
    new public Light light;
    public Ground Control;
    public bool IsATerrain
    {
        get { return this.gameObject.transform.GetChild(0).gameObject.GetComponent<TerrainCollider>(); }
    }

    void Awake()
    {
        if (this.IsATerrain)
            groundSize = new Vector2(this.gameObject.transform.GetChild(0).GetComponent<Terrain>().terrainData.size.x, this.gameObject.transform.GetChild(0).GetComponent<Terrain>().terrainData.size.z);
        else
            groundSize = new Vector2(this.gameObject.transform.GetChild(0).lossyScale.x, this.gameObject.transform.GetChild(0).lossyScale.z);
    }

	void Start () 
    {
        Control = GameObject.FindGameObjectWithTag("GroundControl").GetComponent<Ground>();
        collider = this.gameObject.transform.GetChild(0).gameObject.collider;
      //  rigidbody = this.gameObject.transform.GetChild(0).gameObject.rigidbody;
        light = this.gameObject.transform.GetChild(0).transform.FindChild("GroundLight").light;
        //IsActiveGround = false;
	}

    private void SetGroundVisible(bool value)
    { 
            if (this.IsATerrain)
                this.gameObject.transform.GetChild(0).GetComponent<Terrain>().enabled = value;
            else
                this.gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = value;
    }

}
