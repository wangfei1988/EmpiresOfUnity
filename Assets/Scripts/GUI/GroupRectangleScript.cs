using UnityEngine;
using System.Collections;

public class GroupRectangleScript : MonoBehaviour {

    public GUIScript mainGUI;
    private bool IsSignedIn;

	void Start ()
    {
        this.mainGUI = GUIScript.main.GetComponent<GUIScript>();
        this.IsSignedIn = false;
        gameObject.renderer.enabled = false;
	}

    public void DoUpdate()
    {
        Equalize();
    }

    private void SignIn()
    {
        if (!IsSignedIn)
        {
            gameObject.renderer.enabled = true;
            MouseEvents.LEFTCLICK += MouseEvents_LEFTMouseEvents;
            MouseEvents.RIGHTCLICK += MouseEvents_RIGHTCLICK;
            IsSignedIn = true;
        }
    }
    private void SignOut()
    {
        if (IsSignedIn)
        {
            gameObject.renderer.enabled = false;
            MouseEvents.LEFTCLICK -= MouseEvents_LEFTMouseEvents;
            MouseEvents.RIGHTCLICK -= MouseEvents_RIGHTCLICK;
            IsSignedIn = false;
        }
    }

    void MouseEvents_LEFTMouseEvents(Ray qamRay, bool hold)
    {
        RaycastHit hit;
        if (Physics.Raycast(qamRay, out hit))
        {
            if (hit.collider.gameObject.GetComponent<UnitScript>().IsEnemy(GUIScript.SelectedGroup.GoodOrEvil))
            {
                GUIScript.SelectedGroup.GoupedLeftOnEnemy(hit.collider.gameObject);
            }
        }
        else
        {
            GUIScript.SelectedGroup.GroupedLeftOnGround();
        }
        
    }
    void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {

    }

    private void Equalize()
    {
        if (GUIScript.SelectedGroup.Count > 0)
        {
            SignIn();
            GUIScript.SelectedGroup.CalculateSize();
            gameObject.transform.position = GUIScript.SelectedGroup.Position;
            gameObject.transform.localScale = GUIScript.SelectedGroup.Scale;
        }
        else
        {
            SignOut();
        }
    }

    //public void SetToGUI(GUIScript gui)
    //{
    //}


}
