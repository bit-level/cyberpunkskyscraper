using System.Collections.Generic;
using UnityEngine;

namespace CyberpunkSkyscraper.Concept_0
{
    public enum Color { PINK, BLUE }

    [ExecuteInEditMode]
    public class House : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] Material pink, blue;
        [SerializeField] MeshRenderer windows, neon;
#pragma warning restore 0649

        private Dictionary<Color, Material> _materials;

        private void Awake()
        {
            _materials = new Dictionary<Color, Material>()
            {
                { Color.PINK, pink },
                { Color.BLUE, blue }
            };
        }

        #region Functions

        public void SetWindowsColor(Color color)
        {
            windows.material = _materials[color];
        }

        public void SetNeonColor(Color color)
        {
            neon.material = _materials[color];
        }
        #endregion
    }
}
