using System;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefsVariables;

public class RateUs : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Image[] stars = new Image[5];
    [SerializeField] Color activeStar = Color.yellow;
    [SerializeField] Image rateButton;
    [SerializeField] Text rateButtonText;

    [Header("Store Links")]
    [SerializeField] string linkAppStore;
    [SerializeField] string linkGooglePlay;
#pragma warning restore 0649

    #region Private Fields

    private bool _isStarsSetted = false;
    private int _rating = 0;
    private PlayerPrefsBool _doNotShowAgain;
    private PlayerPrefsBool _showLater;
    private PlayerPrefsDate _showLaterReset;
    #endregion

    #region Properties

    public bool DoNotShowAgain => _doNotShowAgain.GetValue();
    public bool ShowLater => _showLater.GetValue();
    #endregion

    public static RateUs Instance { get; private set; }

    #region MonoBehaviour Callbacks
    
    private void Update()
    {
        // Rate Button
        rateButton.color = (_isStarsSetted) ? Color.white : Utils.Colors.Inactive;
        rateButtonText.color = (_isStarsSetted) ? Color.white : Utils.Colors.Inactive;
    }
    #endregion

    #region Public Functions

    public void Initialize()
    {
        Instance = this;

        _doNotShowAgain = new PlayerPrefsBool("RateUs::DoNotShowAgain");
        _showLater = new PlayerPrefsBool("RateUs::ShowLater");
        _showLaterReset = new PlayerPrefsDate("RateUs::ShowLaterReset");

        if (ShowLater)
        {
            if (DateTime.Compare(DateTime.Now, _showLaterReset.GetValue()) >= 0)
                _showLater.SetValue(false);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        GetComponent<Animation>().Play("Show");
    }

    public void SetStars(int rate)
    {
        int index = rate - 1;
        
        for (int i = 0; i < 5; i++)
        {
            if (i <= index) StartCoroutine(Utils.ChangeGraphicColor(stars[i], activeStar, .2f));
            else StartCoroutine(Utils.ChangeGraphicColor(stars[i], Color.white, .2f));
        }

        _isStarsSetted = true;
        _rating = rate;
    }

    public void Rate()
    {
        if (!_isStarsSetted) return;
        if (_rating == 5)
        {
            string url = Application.platform == RuntimePlatform.Android ? linkGooglePlay : linkAppStore;
            Application.OpenURL(url);
        }
        _doNotShowAgain.SetValue(true);
        Hide();
    }

    public void NeverShowClick()
    {
        _doNotShowAgain.SetValue(true);
        Hide();
    }

    public void ShowLaterClick()
    {
        _showLater.SetValue(true);
        _showLaterReset.SetValue(DateTime.Today.AddDays(1));
        Hide();
    }

    public void Hide()
    {
        ActionList al = new ActionList(GetComponent<ActionSequencer>());
        al.Add(() => GetComponent<Animation>().Play("Hide"), GetComponent<Animation>()["Hide"].length);
        al.Add(() => gameObject.SetActive(false));
        al.Execute();
    }
    #endregion
}
