using UnityEngine;
using UnityEngine.UI;

public class BestScore : MyText
{
    private Text label;
    private const string PREFSKEY = "best_score";
    private int value;

    private void Awake()
    {
        label = GetComponent<Text>();
        if (PlayerPrefs.HasKey(PREFSKEY))
        {
            value = PlayerPrefs.GetInt(PREFSKEY);
            label.text = string.Format("Best Score: {0}", value.ToString());
        }
        else
        {
            value = 0;
            label.text = "Best Score: 0";
        }
    }

    private void Update()
    {
        if (Skyscraper.Instance.CurrentState == Skyscraper.State.Built && value < Score.Instance.Value)
            UpdateValue();
    }

    private void UpdateValue()
    {
        value = Score.Instance.Value;
        PlayerPrefs.SetInt(PREFSKEY, value);
        label.text = string.Format("Best Score: {0}", value.ToString());
    }
}
