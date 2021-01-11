using UnityEngine;

namespace CyberpunkSkyscraper.Highway
{
    public class Car : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private float speed = 1f;
#pragma warning restore 0649

        private void FixedUpdate()
        {
            transform.position = transform.position + transform.forward * speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
    }
}
