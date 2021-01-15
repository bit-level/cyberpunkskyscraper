using System.Collections;
using UnityEngine;

public class PerfectMark : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] float stayTime = 1f;
#pragma warning restore 0649

    private Animation _animation;
    private bool hiding = false;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        _animation.Play("Show");
    }

    private void Update()
    {
        if (stayTime <= 0f && !hiding)
        {
            StartCoroutine(Hide());
            hiding = true;
        }
        stayTime -= Time.deltaTime;
    }

    private IEnumerator Hide()
    {
        _animation.Play("Hide");

        Vector3 startPos = transform.localPosition;
        Vector3 destPos = transform.localPosition + Vector3.up * 100f;
        float duration = .5f;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, destPos, Utils.Curves.Aceleration.Down.Evaluate(t / duration));
            yield return null;
        }

        Destroy(gameObject);
    }
}
