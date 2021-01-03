using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CombineMeshes))]
public class CombineMeshesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CombineMeshes manager = (CombineMeshes)target;

        if (GUILayout.Button("Combine")) manager.Combine();
        if (GUILayout.Button("Activate All")) manager.ActivateAllObjects();
    }
}
