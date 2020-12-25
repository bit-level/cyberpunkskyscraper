using UnityEngine;

public class SkyscraperCamera : MonoBehaviour
{
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 newPosition = startPosition + Vector3.up * Skyscraper.Instance.FloorHeight * Skyscraper.Instance.FloorsCount;
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime);
    }
}
