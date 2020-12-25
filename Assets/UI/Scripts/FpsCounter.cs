using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MyText
{
    private Text label;
    private float refreshTime = 1f;
    private float timer;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            label.text = "FPS: " + fps;
            timer = Time.unscaledTime + refreshTime;
        }
    }
}
