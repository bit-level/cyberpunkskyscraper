using UnityEngine;
using UnityEngine.UI;

public class TapToPlayButton : MyText
{
    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    private void Update()
    {
        if (Skyscraper.Instance.CurrentState == Skyscraper.State.UnderBuild && isShowed)
            Hide();
        else if (Skyscraper.Instance.CurrentState != Skyscraper.State.UnderBuild && !isShowed)
            Show();
        else if (Skyscraper.Instance.CurrentState == Skyscraper.State.Built && label.text != "Restart")
            label.text = "Restart";
        else if (Skyscraper.Instance.CurrentState == Skyscraper.State.ReadyToBuild && label.text != "Tap To Play")
            label.text = "Tap To Play";
    }
}
