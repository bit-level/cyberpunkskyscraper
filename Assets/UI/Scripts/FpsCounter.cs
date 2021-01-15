using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MyText
{
    private Text _label;
    private float _refreshTime = 1f;
    private float _timer;
    private float _totalFramesCount;

    private void Awake()
    {
        _label = GetComponent<Text>();
    }

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _label.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _refreshTime;
            _totalFramesCount += fps;
        }
    }

#if !UNITY_EDITOR
    private void OnApplicationQuit()
    {
        if (_timer < 30f) return;

        float averageFps = (float)_totalFramesCount / _timer;
        AppMetrica.Instance.ReportEvent("Application Quit", new System.Collections.Generic.Dictionary<string, object>()
        {
            { "Average FPS", averageFps }
        });
        AppMetrica.Instance.SendEventsBuffer();
    }
#endif
}
