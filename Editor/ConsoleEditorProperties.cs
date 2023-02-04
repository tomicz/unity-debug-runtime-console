using UnityEngine;
using UnityEditor;

namespace TOMICZ.Debugger
{
    public class ConsoleEditorProperties
    {
        [MenuItem("GameObject/Tomicz/New Console Window")]
        public static void CreateConsolePrefab()
        {
            GameObject console = Resources.Load<GameObject>("ConsoleWindow");
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