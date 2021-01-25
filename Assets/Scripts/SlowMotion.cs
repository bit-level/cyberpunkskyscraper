using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlowMotion : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Image buttonImage;
    [SerializeField] Image progressBar;
    [SerializeField] Text cost;
    [SerializeField] Text watchAd;
    [SerializeField] float activeTime = 5f;
    [SerializeField] float timeScale = .5f;
#pragma warning restore 0649

    private bool _state = false;

    private readonly Color ACTIVE = new Color(1f, 1f, 1f, 1f);
    private readonly Color DISACTIVE = new Color(.5f, .5f, .5f, 1f);
    private readonly Color HIDEN = new Color(1f, 1f, 1f, 0f);

    public bool IsShowed { get; private set; }
    public static SlowMotion Instance { get; private set; }

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;

        buttonImage.color = HIDEN;
        progressBar.color = HIDEN;
        cost.color = HIDEN;
        watchAd.color = HIDEN;
    }

    private void Start()
    {
        Skyscraper.Instance.OnGameOver += (score, bestScore) =>
        {
            Time.timeScale = 1f;
        };
    }

    private void Update()
    {
        if (_state == false && Skyscraper.Instance.CurrentState == Skyscraper.State.UnderBuild)
            Show();
        else if (_state == true && Skyscraper.Instance.CurrentState == Skyscraper.State.Built)
        {
            Hide();
            Time.timeScale = 1f;
        }
    }
    #endregion

    #region Public Functions

    public void Activate()
    {
        ActionList al = new ActionList(GetComponent<ActionSequencer>());

        al.Add(() =>
        {
            Time.timeScale = timeScale;
            GetComponent<Button>().interactable = false;
            LaunchProgressBar(activeTime);
            StartCoroutine(Utils.ChangeGraphicColor(cost, HIDEN, .5f));
            StartCoroutine(Utils.ChangeGraphicColor(watchAd, HIDEN, .5f));
            Skyscraper.Instance.perfectDistance = .2f;
        }, .3f);

        al.Add(() =>
        {
            StartCoroutine(Utils.ChangeGraphicColor(buttonImage, DISACTIVE, .5f));
        }, activeTime - .3f);

        al.Add(() =>
        {
            Time.timeScale = 1f;
            GetComponent<Button>().interactable = true;
            StartCoroutine(Utils.ChangeGraphicColor(buttonImage, ACTIVE, .5f));
            StartCoroutine(Utils.ChangeGraphicColor(cost, ACTIVE, .5f));
            StartCoroutine(Utils.ChangeGraphicColor(watchAd, ACTIVE, .5f));
            Skyscraper.Instance.perfectDistance = .125f;
        });

        al.Execute();
    }

    public void Show()
    {
        _state = true;
        GetComponent<Button>().interactable = true;
        StartCoroutine(Utils.ChangeGraphicColor(buttonImage, ACTIVE, .5f));
        StartCoroutine(Utils.ChangeGraphicColor(cost, ACTIVE, .5f));
        StartCoroutine(Utils.ChangeGraphicColor(watchAd, ACTIVE, .5f));

        IsShowed = true;
    }

    public void Hide()
    {
        _state = false;
        GetComponent<Button>().interactable = false;
        StartCoroutine(Utils.ChangeGraphicColor(buttonImage, HIDEN, .5f));
        StartCoroutine(Utils.ChangeGraphicColor(progressBar, HIDEN, .5f));
        StartCoroutine(Utils.ChangeGraphicColor(cost, HIDEN, .5f));
        StartCoroutine(Utils.ChangeGraphicColor(watchAd, HIDEN, .5f));

        IsShowed = false;
    }
    #endregion

    #region Private Functions

    private void LaunchProgressBar(float activeTime)
    {
        StartCoroutine(ProgressBarSetActive(true));
        StartCoroutine(ProgressBarStartCounting(activeTime));
    }
    #endregion

    #region Coroutines

    private IEnumerator ProgressBarSetActive(bool active)
    {
        Color startColor = progressBar.color;
        Color destColor = progressBar.color;

        Vector3 startScale = Vector3.one;
        Vector3 destScale = Vector3.one;

        if (active)
        {
            startColor.a = 0f;
            destColor.a = 1f;
            startScale *= .7f;
        }
        else
        {
            startColor.a = 1f;
            destColor.a = 0f;
            destScale *= .7f;
        }

        float duration = .3f;

        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            progressBar.color = Color.Lerp(startColor, destColor, t / duration);
            progressBar.transform.localScale = Vector3.Lerp(startScale, destScale, t / duration);
            yield return null;
        }

        progressBar.color = destColor;
        progressBar.transform.localScale = destScale;

        if (active) progressBar.fillAmount = 1f;
    }

    private IEnumerator ProgressBarStartCounting(float activeTime)
    {
        for (float t = activeTime; t >= 0f; t -= Time.unscaledDeltaTime)
        {
            progressBar.fillAmount = t / activeTime;
            yield return null;
        }

        progressBar.fillAmount = 0f;
    }
    #endregion
}
