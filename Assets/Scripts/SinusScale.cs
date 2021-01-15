using UnityEngine;

[ExecuteInEditMode]
public class SinusScale : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] float scale;
    [SerializeField] float speed;
#pragma warning restore 0649

    private Vector3 defaultScale;

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        float sin = Mathf.Sin(Time.time * speed) + 1f;
        sin /= 2f;
        transform.localScale = defaultScale + Vector3.one * scale * sin;
    }
}
