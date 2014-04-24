using UnityEngine;
using System.Collections;

public class MarkerSqript : MonoBehaviour {

    public UnitAnimation animationSqrips;
    [SerializeField]
    private bool visible;
    public bool Visible
    {
        get { return gameObject.renderer.enabled = gameObject.light.enabled = visible; }
        set {visible = gameObject.renderer.enabled = gameObject.light.enabled = value; }
    }
	void Start ()
    {
        Visible = false;
	}
	
	// Update is called once per frame
    public void DoUpdate()
    {
        animationSqrips.DoUpdate();
    }

}
