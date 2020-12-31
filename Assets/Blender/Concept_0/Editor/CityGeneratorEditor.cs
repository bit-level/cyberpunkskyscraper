using UnityEngine;
using UnityEditor;

namespace CyberpunkSkyscraper.Concept_0
{
    [CustomEditor(typeof(CityGenerator))]
    public class CityGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CityGenerator manager = (CityGenerator)target;

            if (GUILayout.Button("Generate"))
            {
                manager.GenerateCity();
            }

            if (manager.transform.childCount > 0)
            {
                if (GUILayout.Button("Clear Data")) manager.ClearData();
                if (GUILayout.Button("Activate check mode")) manager.SetCheckMode(true);
                if (GUILayout.Button("Deactivate check mode")) manager.SetCheckMode(false);
            }
        }
    }
}

