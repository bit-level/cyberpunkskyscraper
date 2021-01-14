using UnityEngine;
using UnityEngine.UI;

public class BestScore : MyText
{
    private Text label;
    private const string PREFSKEY = "best_score";
    private int value;

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        // Initialization
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

        UpdateLevelSystem();
    }

    private void Update()
    {
        if (Skyscraper.Instance.CurrentState == Skyscraper.State.Built && value < Score.Instance.Value)
            NewBestScore();
    }
    #endregion

    private void NewBestScore()
    {
        value = Score.Instance.Value;
        PlayerPrefs.SetInt(PREFSKEY, value);
        label.text = string.Format("Best Score: {0}", value.ToString());

        UpdateLevelSystem();

#if !UNITY_EDITOR
        if (!Skyscraper.Instance.HasCheatActiveOnce)
        {
            AppMetrica.Instance.ReportEvent("new_best_score", new System.Collections.Generic.Dictionary<string, object>()
            {
                { "total_play_time", Mathf.RoundToInt(GameManager.Instance.TotalPlayTime)}
            });
            AppMetrica.Instance.SendEventsBuffer();
        }
#endif
    }

    private void UpdateLevelSystem()
    {
        float newValue = (float)value / Globals.MAXIMUM_SCORE;
        LevelSystem.Instance.Progress = newValue;
    }
}