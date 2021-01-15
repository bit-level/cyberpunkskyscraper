using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowGraphicAtStart : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Graphic graphic;
#pragma warning restore 0649

    private void Awake()
    {
        StartCoroutine(Show());    
    }

    private IEnumerator Show()
    {
        Color start = graphic.color;
        Color dest = graphic.color;

        start.a = 0f;

        float duration = 1f;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            graphic.color = Color.Lerp(start, dest, t / duration);
            yield return null;
        }

        graphic.color = dest;
    }
}
