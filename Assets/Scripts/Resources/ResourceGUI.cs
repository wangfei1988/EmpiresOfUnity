using UnityEngine;
using System.Collections;

public class ResourceGUI : MonoBehaviour
{
    /* Images */
    public Texture2D icon_nanite;
    public Texture2D icon_matter;
    public Texture2D icon_energy;
    public Texture2D icon_laborer;

    private Vector2 size = new Vector2(1920, 1080);
    private float left, top, leftPlus, width, height;
    //private GUIStyle GuiStyle;

    void Start()
    {
        //this.GuiStyle = new GUIStyle();
        //this.GuiStyle.fontSize = 20;
    }

    void OnGUI()
    {
        // TODO Only Update these Values at ResolutionChange
        left = Screen.width / size.x * 30f;
        top = Screen.height / size.y * 30f;
        leftPlus = Screen.width / size.x * 215f;
        width = Screen.width / size.x * 200f;
        height = Screen.width / size.x * 70f;

        GUI.Box(new Rect(left + leftPlus * 0, top, width, height), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.NANITEN), icon_nanite));
        GUI.Box(new Rect(left + leftPlus * 1, top, width, height), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.MATTER), icon_matter));
        GUI.Box(new Rect(left + leftPlus * 2, top, width, height), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.ENERGY) + " / " + ResourceManager.GetResourceCount(ResourceManager.Resource.MAXENERGY), icon_energy));
        GUI.Box(new Rect(left + leftPlus * 3, top, width, height), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.LABORER), icon_laborer));
    }
}
