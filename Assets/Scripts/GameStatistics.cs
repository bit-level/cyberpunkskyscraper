using System.Collections.Generic;
using UnityEngine;

public class GameStatistics : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Options")]
    [SerializeField] bool time = true;
    [SerializeField] bool minScore, maxScore, avgScore;
    [SerializeField] bool earnedMoney = true;
    [SerializeField] bool gamesCount = true;
    [SerializeField] bool sendToAppMetrica = false;
#pragma warning restore 0649

    #region Private Fields

    private float _timer = 0f;
    private List<int> _scoreList = new List<int>();
    private int _earnedMoney = 0;
    private int _gamesCount = 0;
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        Money.Instance.OnPutMoney += (amount) => _earnedMoney += amount;
        Skyscraper.Instance.OnGameOver += (score) =>
        {
            _scoreList.Add(score);
            _gamesCount++;
        };
    }

    private void Update()
    {
        _timer += Time.unscaledDeltaTime;
    }

    private void OnApplicationQuit()
    {
        if (time) PrintTime();

        int min, max, avg;
        CalculateScore(out min, out max, out avg);

        if (minScore) print("Min score: " + min);
        if (maxScore) print("Max score: " + max);
        if (avgScore) print("Avg score: " + avg);

        if (earnedMoney) print("Earned money: " + _earnedMoney);
        if (gamesCount) print("Games Count: " + _gamesCount);

        if (!Skyscraper.Instance.HasCheatActiveOnce && _timer >= 15f && sendToAppMetrica)
        {
            AppMetrica.Instance.ReportEvent("Session result", new Dictionary<string, object>()
            {
                { "Playing Time (min)", _timer / 60f },
                { "Miminum Score", min },
                { "Maximum Score", max },
                { "Average Score", avg },
                { "Earned Money", _earnedMoney },
                { "Games Count", _gamesCount },
                { "Platform",  Application.platform }
            });
            AppMetrica.Instance.SendEventsBuffer();
        }
    }
    #endregion

    #region Functions

    private void PrintTime()
    {
        int m = Mathf.FloorToInt(_timer / 60f);
        int s = Mathf.RoundToInt(((_timer / 60f) % Mathf.Floor(_timer)) * 60f);

        print(string.Format("Playing time: {0}m {1}s", m, s));
    }
   
    private void CalculateScore(out int min, out int max, out int avg)
    {
        min = int.MaxValue;
        max = int.MinValue;
        avg = 0;

        foreach (int score in _scoreList)
        {
            if (score < min) min = score;
            if (score > max) max = score;
            avg += score;
        }

        if (min == int.MaxValue) min = 0;
        if (max == int.MinValue) max = 0;
        if (_scoreList.Count > 0) avg /= _scoreList.Count;
    }
    #endregion
}
