using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Utils : MonoBehaviour
{
    [SerializeField] AnimationCurve acelerationUp = null;
    [SerializeField] AnimationCurve acelerationDown = null;

    public static class Curves
    {
        public static class Aceleration
        {
            public static AnimationCurve Up;
            public static AnimationCurve Down;
        }
    }

    private void Awake()
    {
        Curves.Aceleration.Up = acelerationUp;
        Curves.Aceleration.Down = acelerationDown;
    }

    public static IEnumerator ChangeGraphicColor(Graphic graphic, Color startColor, Color destColor, float duration)
    {
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            graphic.color = Color.Lerp(startColor, destColor, t / duration);
            yield return null;
        }

        graphic.color = destColor;
    }

    public static IEnumerator ChangeGraphicColor(Graphic graphic, Color destColor, float duration)
    {
        Color startColor = graphic.color;

        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            graphic.color = Color.Lerp(startColor, destColor, t / duration);
            yield return null;
        }

        graphic.color = destColor;
    }
}
