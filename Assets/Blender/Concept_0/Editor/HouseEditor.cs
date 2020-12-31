using UnityEngine;
using UnityEditor;

namespace CyberpunkSkyscraper.Concept_0
{
    [CustomEditor(typeof(House))]
    public class HouseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            House manager = (House)target;

            if (GUILayout.Button("Randomize Colors"))
            {
                manager.SetRandomColors();
            }
        }
    }
}
