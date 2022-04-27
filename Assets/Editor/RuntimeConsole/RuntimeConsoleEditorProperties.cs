using UnityEngine;
using UnityEditor;

public class RuntimeConsoleEditorProperties
{
    [MenuItem("GameObject/UI/TOMICZ/Create Runtine Console")]
    public static void CreateConsolePrefab()
    {
        string path = "Assets/RuntimeConsole/Prefabs/Console.prefab";

        GameObject console = AssetDatabase.LoadAssetAtPath(path, (typeof(GameObject))) as GameObject;
        var consolePrefab = PrefabUtility.InstantiatePrefab(console) as GameObject;
        consolePrefab.transform.SetParent(Selection.activeTransform);
    }
}