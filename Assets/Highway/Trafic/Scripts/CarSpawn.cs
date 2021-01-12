using UnityEngine;

namespace CyberpunkSkyscraper.Highway
{
    public class CarSpawn : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private Car[] prefabs;
        [SerializeField] private float spawnDelayMin, spawnDelayMax;
#pragma warning restore 0649

        private float spawnDelay = 0f;

        private void Awake()
        {
            spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
        }

        private void Update()
        {
            if (spawnDelay <= 0f)
            {
                Spawn();
                spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
            }

            spawnDelay -= Time.deltaTime;
        }

        public void Spawn()
        {
            Car car = Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, transform.rotation, transform);
        }
    }
}

