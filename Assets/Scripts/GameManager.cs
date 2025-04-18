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

        Skyscraper.Instance.OnGameOver += (score, bestScore) =>
        {
            bool showRateUs = score >= 40 && !rateUs.DoNotShowAgain && !rateUs.ShowLater && !gameConfig.DisableRateUsPrompt;
            if (showRateUs) rateUs.Show();
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
