using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class PerfectMark : MonoBehaviour
{
    [SerializeField] float stayTime = 1f;
    [SerializeField] Text text;

    private Animation _animation;
    private bool hiding = false;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        _animation.Play("Show");
        text.text = YG2.lang == "ru" ? "Идеально!" : "Perfect!";
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
