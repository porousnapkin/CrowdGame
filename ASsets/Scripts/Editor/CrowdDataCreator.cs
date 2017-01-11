using UnityEditor;
using UnityEngine;

public class CrowdDataCreator : Editor {
    [MenuItem("Custom/Create Crowd Data")]
    public static void CreateCrowdData() {
        var moveData = ScriptableObject.CreateInstance<CrowdMoveData>();
        AssetDatabase.CreateAsset(moveData, "Assets/Data/CrowdMoveData.asset");
        EditorUtility.SetDirty(moveData);
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = moveData;
    }
}
