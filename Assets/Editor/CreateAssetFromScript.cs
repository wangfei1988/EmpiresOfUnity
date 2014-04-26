using UnityEngine;
using UnityEditor;
using System;

public class CreateAssetFromScript : Editor
{
    /*
     * Create an Asset From an ScriptableObject C# Script
     * @github https://gist.github.com/mstevenson/4726563
     * @author Dario D. Müller
     * @date 2014-04-25
     */

    [MenuItem("Project X/Create Asset From Script", false, 10000)]
    public static void CreateManager()
    {
        ScriptableObject asset = ScriptableObject.CreateInstance(Selection.activeObject.name);
        AssetDatabase.CreateAsset(asset, String.Format("Assets/Resources/{0}.asset", Selection.activeObject.name));
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}