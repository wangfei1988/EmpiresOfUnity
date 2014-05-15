using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetFromObject : Editor
{
    [MenuItem("Project X/Create Prefab From Object", false, 10000)]
    public static void CreateManager()
    {

        AssetDatabase.CreateAsset(PrefabUtility.CreatePrefab("Assets/Resources/" + Selection.activeObject.name + ".prefab", Selection.activeGameObject), "Assets/Resources/" + Selection.activeObject.name + ".asset");

        EditorUtility.FocusProjectWindow();
        
    //    Selection.activeObject = newObject;
    }
}
