﻿using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class BestScore : MyText
{
    [SerializeField] new ParticleSystem particleSystem = null;

    private Text label;
    private const string PREFSKEY = "best_score";

    public static BestScore Instance { get; private set; }

    public int Value { get; private set; }

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;    

        // Initialization
        label = GetComponent<Text>();
        if (PlayerPrefs.HasKey(PREFSKEY))
        {
            Value = PlayerPrefs.GetInt(PREFSKEY);
            label.text = string.Format("Best Score: {0}", Value.ToString());
        }
        else
        {
            Value = 0;
            label.text = "Best Score: 0";
        }
    }

    private new void Start()
    {
        base.Start();
        UpdateLevelSystem();

        Skyscraper.Instance.OnGameStart += () => label.color = Color.white;
        Skyscraper.Instance.OnGameOver += (score, bestScore) => { if (bestScore) NewBestScore(); };
    }
    #endregion

    #region Functions

    private void NewBestScore()
    {
        Value = Score.Instance.Value;
        PlayerPrefs.SetInt(PREFSKEY, Value);
        label.text = string.Format("Best Score: {0}", Value.ToString());

        label.color = Color.green;
        particleSystem.Play();
        GetComponent<Animation>().Play("NewBestScore");

        UpdateLevelSystem();
    }

    private void UpdateLevelSystem()
    {
        float newValue = (float)Value / Globals.MAXIMUM_SCORE;
        LevelSystem.Instance.Progress = newValue;
    }
    #endregion
}