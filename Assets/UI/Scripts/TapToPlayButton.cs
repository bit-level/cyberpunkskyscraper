using UnityEngine.UI;

public class TapToPlayButton : MyText
{
    private Text label;

    public static TapToPlayButton Instance { get; private set; }

    private void Awake()
    {
        label = GetComponent<Text>();
        Instance = this;
    }

    private new void Start()
    {
        base.Start();
        Skyscraper.Instance.OnGameStart += Hide;
    }

    public new void Show()
    {
        base.Show();
    }

    public void ShowRestart()
    {
        label.text = "Restart";
        base.Show();
    }
}
