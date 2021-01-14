using UnityEngine;
using UnityEngine.UI;

public class Score : MyText
{
	private Text label;

	public static Score Instance { get; private set; }
	public int Value { get; private set; }

	private void Awake()
	{
		label = GetComponent<Text>();
		Instance = this;
	}

	private void Update()
	{
		int score = Skyscraper.Instance.FloorsCount;
		label.text = score.ToString();
		Value = score;
		UpdateLevelSystem();
	}

	private void UpdateLevelSystem()
    {
		float progress = (float)Value / Globals.MAXIMUM_SCORE;
		LevelSystem.Instance.CurrentPosition = progress;

		if (progress > LevelSystem.Instance.Progress)
			LevelSystem.Instance.Progress = progress;
    }
}
