using UnityEngine;
using System.Collections;

public class GroupRectangleScript : MonoBehaviour {

    public GUIScript mainGUI;
    private bool IsSignedIn;
    //public UnitGroup Group
    //{
    //    get { return qui}
    //}

	void Start () 
    {
        IsSignedIn = false;
        gameObject.renderer.enabled = false;

        UpdateManager.OnUpdate += DoUpdate;
	}

    void DoUpdate()
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
            if (hit.collider.gameObject.GetComponent<UnitScript>().GoodOrEvil != mainGUI.SelectedGroup.GoodOrEvil)
            {
                mainGUI.SelectedGroup.GoupedLeftOnEnemy(hit.collider.gameObject);
            }
        }
        else
        {
            mainGUI.SelectedGroup.GroupedLeftOnGround();
        }
        
    }
    void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {

    }


    private void Equalize()
    {
        if (mainGUI.SelectedGroup.Count > 0)
        {
            SignIn();
            mainGUI.SelectedGroup.CalculateSize();
            gameObject.transform.position = mainGUI.SelectedGroup.Position;
            gameObject.transform.localScale = mainGUI.SelectedGroup.Scale;
        }
        else
        {
            SignOut();
            
        }
    }

    public void SetToGUI(GUIScript gui)
    {
          mainGUI = gui;
    }


}
