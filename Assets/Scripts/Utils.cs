using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = RedefineYG.PlayerPrefs;

namespace PlayerPrefsVariables
{
    public abstract class PlayerPrefsVariable<T>
    {
        protected readonly string key;

        public PlayerPrefsVariable(string key, T defaultValue)
        {
            this.key = key;
            if (!PlayerPrefs.HasKey(key)) SetValue(defaultValue);
        }

        public abstract void SetValue(T value);
        public abstract T GetValue();
    }

    public class PlayerPrefsBool : PlayerPrefsVariable<bool>
    {
        public PlayerPrefsBool(string key) : base(key, false) { }

        public override void SetValue(bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            Saver.Instance.Save();
        }

        public override bool GetValue() => PlayerPrefs.GetInt(key) == 1;
    }

    public class PlayerPrefsDate : PlayerPrefsVariable<DateTime>
    {
        public PlayerPrefsDate(string key) : base(key, DateTime.Now) { }

        public override void SetValue(DateTime value)
        {
            PlayerPrefs.SetString(key, value.ToString());
            Saver.Instance.Save();
        }

        public override DateTime GetValue() => DateTime.Parse(PlayerPrefs.GetString(key));
    }
}

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

    public static class Colors
    {
        public static Color Inactive = new Color(1f, 1f, 1f, .6f);
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

    public static IEnumerator ChangeGraphicsColor(Graphic[] graphics, Color startColor, Color destColor, float duration)
    {
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            Color newColor = Color.Lerp(startColor, destColor, t / duration);
            foreach (Graphic graphic in graphics)
                graphic.color = newColor;
            yield return null;
        }

        foreach (Graphic graphic in graphics)
            graphic.color = destColor;
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
