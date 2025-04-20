using System;
using System.Collections.Generic;
using BitLevel.Core.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private RateUs rateUs;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        rateUs.Initialize();

        string saveId = PlayerPrefs.HasKey("id") ? PlayerPrefs.GetString("id") : Guid.NewGuid().ToString();
        int sessionIndex = PlayerPrefs.HasKey("session_index") ? PlayerPrefs.GetInt("session_index") : -1;
        PlayerPrefs.SetString("id", saveId);
        PlayerPrefs.SetInt("session_index", ++sessionIndex);
        Saver.Instance.Save();
        GameEvents.Init(saveId, sessionIndex, new List<IEventsSender>() { new DebugEventsSender() });
        GameEvents.GameLaunched();

        Skyscraper.Instance.OnGameOver += (score, bestScore) =>
        {
            bool showRateUs = score >= 40 && !rateUs.DoNotShowAgain && !rateUs.ShowLater && !gameConfig.DisableRateUsPrompt;
            if (showRateUs) rateUs.Show();
            GameEvents.GameFailed(score);
        };
    }

    public void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
        Saver.Instance.Save();
    }

    public void DeleteProgress()
    {
        string[] keys = {
            "best_score", "FirstFloorScaleIndex", "FloorSizeLevelUp::Hint",
            "RateUs::DoNotShowAgain", "RateUs::ShowLater", "RateUs::ShowLaterReset",
            "wallet"
        };

        foreach (string key in keys)
            PlayerPrefs.DeleteKey(key);    

        Saver.Instance.Save();
        SceneManager.LoadScene(0);
    }
}
