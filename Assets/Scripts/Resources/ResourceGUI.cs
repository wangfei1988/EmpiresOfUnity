using UnityEngine;
using System.Collections;

public class ResourceGUI : MonoBehaviour
{

    public Texture2D icon_wood;
    public Texture2D icon_stone;
    public Texture2D icon_gold;
    public Texture2D icon_people;

	/* Reflection */
	void Start () {
        UpdateManager.OnUpdate += DoUpdate;
	}

    void DoUpdate()
    {
    }

    void OnGUI()
    {
        GUI.Box(new Rect(12, 12, 70, 30), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.NANITEN), icon_wood));
        GUI.Box(new Rect(88, 12, 70, 30), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.MATTER), icon_stone));
        GUI.Box(new Rect(164, 12, 70, 30), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.ENERGY), icon_gold));
        GUI.Box(new Rect(240, 12, 70, 30), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.ENERGY), icon_people));
    }

    /* Methods */
}
