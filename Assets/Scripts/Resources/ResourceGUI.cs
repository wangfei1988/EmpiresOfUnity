using UnityEngine;
using System.Collections;

public class ResourceGUI : MonoBehaviour
{

    public Texture2D icon_nanite;
    public Texture2D icon_matter;
    public Texture2D icon_energy;
    public Texture2D icon_laborer;

    void OnGUI()
    {
        GUI.Box(new Rect(12, 12, 70, 30), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.NANITEN), icon_nanite));
        GUI.Box(new Rect(88, 12, 70, 30), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.MATTER), icon_matter));
        GUI.Box(new Rect(164, 12, 70, 30), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.ENERGY), icon_energy));
        GUI.Box(new Rect(240, 12, 70, 30), new GUIContent(" " + ResourceManager.GetResourceCount(ResourceManager.Resource.LABORER), icon_laborer));
    }
}
