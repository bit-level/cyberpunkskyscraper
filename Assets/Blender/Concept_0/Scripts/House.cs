using UnityEngine;

namespace CyberpunkSkyscraper.Concept_0
{
    public class House : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] Material[] materials;
        [SerializeField] MeshRenderer[] elements;
        [SerializeField] Material checkedMaterial;
        [SerializeField] float checkShift = 0f;
#pragma warning restore 0649

        private bool _checked = false;

        public bool checkVisibility = false;

        private void Update()
        {
            if (checkVisibility && !_checked && IsObjectVisible())
            {
                foreach (MeshRenderer el in elements)
                    el.material = checkedMaterial;
                _checked = true;
                print(name);
            }
        }

        private bool IsObjectVisible()
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            screenPosition.x -= Screen.width / 2f;
            screenPosition.y -= Screen.height / 2f;

            bool xCorrect = Mathf.Abs(screenPosition.x) - checkShift < Screen.width / 2f;
            bool yCorrect = Mathf.Abs(screenPosition.y) - checkShift < Screen.height / 2f;

            return xCorrect && yCorrect;
        }

        public void SetRandomColors()
        {
            foreach (MeshRenderer el in elements)
                el.material = materials[Random.Range(0, materials.Length)];
        }
    }
}
