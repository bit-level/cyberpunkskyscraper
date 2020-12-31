using UnityEngine;

namespace CyberpunkSkyscraper.Concept_0
{
    public class CityGenerator : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] House[] buildingsPrefabs;
        [SerializeField] int buildingsInRow = 5;
        [SerializeField] int buildingsInColumn = 5;
        [SerializeField] float distanceBetweenBuildings = 10f;
#pragma warning restore 0649

        public void GenerateCity()
        {
            ClearData();
            for (int x = 0; x < buildingsInRow; ++x)
            {
                for (int y = 0; y < buildingsInColumn; ++y)
                {
                    Vector3 position = new Vector3(
                        x * distanceBetweenBuildings,
                        transform.position.y,
                        y * distanceBetweenBuildings);
                    House prefab = buildingsPrefabs[Random.Range(0, buildingsPrefabs.Length)];

                    House building = CreateBuilding(prefab, position);
                    building.SetRandomColors();
                    building.name = (buildingsInRow * x + y).ToString();
                    building.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f * Random.Range(0, 4), 0f));
                }
            }
        }

        public void ClearData()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; ++i)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }

        public void SetCheckMode(bool state)
        {
            for (int i = 0; i < transform.childCount; ++i)
                transform.GetChild(i).GetComponent<House>().checkVisibility = state;
        }

        private House CreateBuilding(House prefab, Vector3 position)
        {
            return Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }
}

