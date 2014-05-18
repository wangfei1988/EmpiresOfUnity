using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{

	void Start ()
	{
	    UpdateManager.OnUpdate += this.DoUpdate;
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 5, Screen.width / 3, Screen.height / 5), "Spiel Starten"))
        {
            Debug.Log("Start");
            Application.LoadLevel("InGame");
        }
        if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 5*2, Screen.width / 3, Screen.height / 5), "Ende"))
        {
            Debug.Log("Ende");
            Application.Quit();
        }
    }

    void DoUpdate()
    {

    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= this.DoUpdate;
    }
}
