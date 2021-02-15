using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] RateUs rateUs;
#pragma warning restore 0649

    private int _adShowsCount = 0;

    private void Start()
    {
        rateUs.Initialize();

        Skyscraper.Instance.OnGameOver += (score, bestScore) =>
        {
            bool showRateUs = (score >= 40) && (!rateUs.DoNotShowAgain) && (!rateUs.ShowLater);

            if (showRateUs) rateUs.Show();
            else if (score >= 10 && !bestScore)
            {
                if (_adShowsCount++ % 3 != 0)
                    Ad.Instance.ShowIfReady(Ad.Type.Interstitial);
            }
        };
    }

    public void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
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

        SceneManager.LoadScene(0);
    }
}
