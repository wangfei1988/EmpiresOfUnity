using UnityEngine;
using System.Collections;

public class GroupRectangleSqript : MonoBehaviour {

    public GUISqript mainGUI;
    private bool IsSignedIn;
    //public UnitGroup Group
    //{
    //    get { return qui}
    //}

	void Start () 
    {
        IsSignedIn = false;
        gameObject.renderer.enabled = false;

	}
    
    private void SignIn()
    {
        if (!IsSignedIn)
        {
            gameObject.renderer.enabled = true;
            Qlick.LEFTQLICK += Qlick_LEFTQLICK;
            Qlick.RIGHTQLICK += Qlick_RIGHTQLICK;
            IsSignedIn = true;
        }
    }
    private void SignOut()
    {
        if (IsSignedIn)
        {
            gameObject.renderer.enabled = false;
            Qlick.LEFTQLICK -= Qlick_LEFTQLICK;
            Qlick.RIGHTQLICK -= Qlick_RIGHTQLICK;
            IsSignedIn = false;
        }
    }

    void Qlick_LEFTQLICK(Ray qamRay, bool hold)
    {
        RaycastHit hit;
        if (Physics.Raycast(qamRay, out hit))
        {
            if (hit.collider.gameObject.GetComponent<UnitSqript>().GoodOrEvil != mainGUI.SellectedGroup.GoodOrEvil)
                mainGUI.SellectedGroup.GoupedLeftOnEnemy(hit.collider.gameObject);
        }
        else mainGUI.SellectedGroup.GroupedLeftOnGround();
    }
    void Qlick_RIGHTQLICK(Ray qamRay, bool hold)
    {

    }


    private void Equalize()
    {
        if (mainGUI.SellectedGroup.Count > 0)
        {
            SignIn();
            mainGUI.SellectedGroup.CalculateSize();
            gameObject.transform.position = mainGUI.SellectedGroup.Position;
            gameObject.transform.localScale = mainGUI.SellectedGroup.Scale;
        }
        else
        {
            SignOut();
            
        }
    }

    public void SetToGUI(GUISqript gui)
    {
          mainGUI = gui;
    }
	// Update is called once per frame
	void Update () 
    {
        Equalize();
	}
}
