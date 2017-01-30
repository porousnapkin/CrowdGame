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

    [MenuItem("Custom/Create Enemy Spawning Data")]
    public static void CreateEnemySpawningData()
    {
        var moveData = ScriptableObject.CreateInstance<EnemySpawningSet>();
        AssetDatabase.CreateAsset(moveData, "Assets/Data/EnemySpawningSet.asset");
        EditorUtility.SetDirty(moveData);
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = moveData;
    }
}
