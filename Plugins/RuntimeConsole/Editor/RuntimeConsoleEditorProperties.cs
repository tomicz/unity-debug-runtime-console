using UnityEngine;
using UnityEditor;

namespace TOMICZ
{
    public class RuntimeConsoleEditorProperties
    {
        [MenuItem("GameObject/Tomicz/New Console Window")]
        public static void CreateConsolePrefab()
        {
            string path = "Assets/Plugins/RuntimeConsole/Prefabs/ConsoleWindow.prefab";

            GameObject console = AssetDatabase.LoadAssetAtPath(path, (typeof(GameObject))) as GameObject;
            var consolePrefab = PrefabUtility.InstantiatePrefab(console) as GameObject;

            SetObjectProperties(consolePrefab.transform);
        }

        private static void SetObjectProperties(Transform newObject)
        {
            newObject.SetParent(Selection.activeTransform);
            newObject.localScale = Vector3.one;
            newObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 400);
        }
    }
}