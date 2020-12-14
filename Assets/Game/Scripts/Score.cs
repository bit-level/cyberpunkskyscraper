using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	private Text label;

	private void Awake()
	{
		label = GetComponent<Text>();
	}

	private void Update()
	{
		label.text = Skyscraper.Instance.FloorsCount.ToString();
	}
}
