using UnityEngine;

public class GameManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] RateUs rateUs;
#pragma warning restore 0649

    private void Start()
    {
        rateUs.Initialize();
        Skyscraper.Instance.OnGameOver += (score) =>
        {
            bool showRateUs = (score >= 40) && (!rateUs.DoNotShowAgain) && (!rateUs.ShowLater);

            if (showRateUs) rateUs.Show();
            else if (score >= 20) Ad.Instance.ShowIfReady(Ad.Type.Interstitial);
        };
    }
}
