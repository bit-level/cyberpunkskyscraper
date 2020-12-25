using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Properties

    public static GameManager Instance { get; private set; }
    public float TotalPlayTime { get; private set; }
    #endregion

    #region Constants
    private const string TOTAL_PLAY_TIME_PREFS_KEY = "TotalPlayTime";
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey(TOTAL_PLAY_TIME_PREFS_KEY))
        {
            TotalPlayTime = PlayerPrefs.GetFloat(TOTAL_PLAY_TIME_PREFS_KEY);
        }
        else
        {
            PlayerPrefs.SetFloat(TOTAL_PLAY_TIME_PREFS_KEY, TotalPlayTime = 0f);
        }
    }

    private void Update()
    {
        if (Skyscraper.Instance.CurrentState == Skyscraper.State.UnderBuild)
            TotalPlayTime += Time.deltaTime;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(TOTAL_PLAY_TIME_PREFS_KEY, TotalPlayTime);
    }
    #endregion
}
