using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlowMotion : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Image progressBar;
    [SerializeField] GameObject adIcon;
    [SerializeField] float activeTime = 5f;
    [SerializeField] float timeScale = .5f;
    [SerializeField] Button button;

    private bool _state = false;

    private readonly Color ACTIVE = new Color(1f, 1f, 1f, 1f);
    private readonly Color DISACTIVE = new Color(.5f, .5f, .5f, 1f);
    private readonly Color HIDEN = new Color(1f, 1f, 1f, 0f);

    public bool IsShowed { get; private set; }
    public static SlowMotion Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        buttonImage.color = HIDEN;
        progressBar.color = HIDEN;
        adIcon.SetActive(false);

        button.onClick.AddListener(OnButtonClicked);
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

    public void Activate()
    {
        ActionList al = new ActionList(GetComponent<ActionSequencer>());

        al.Add(() =>
        {
            Time.timeScale = timeScale;
            button.interactable = false;
            LaunchProgressBar(activeTime);
            adIcon.SetActive(false);
            Skyscraper.Instance.perfectDistance = .2f;
        }, .3f);

        al.Add(() =>
        {
            StartCoroutine(Utils.ChangeGraphicColor(buttonImage, DISACTIVE, .5f));
        }, activeTime - .3f);

        al.Add(() =>
        {
            Time.timeScale = 1f;
            button.interactable = true;
            StartCoroutine(Utils.ChangeGraphicColor(buttonImage, ACTIVE, .5f));
            adIcon.SetActive(true);
            Skyscraper.Instance.perfectDistance = Skyscraper.Instance.PerfectDistanceDefault;
        });

        al.Execute();
    }

    public void Show()
    {
        _state = true;
        button.interactable = true;
        StartCoroutine(Utils.ChangeGraphicColor(buttonImage, ACTIVE, .5f));
        adIcon.SetActive(true);

        IsShowed = true;
    }

    public void Hide()
    {
        _state = false;
        button.interactable = false;
        StartCoroutine(Utils.ChangeGraphicColor(buttonImage, HIDEN, .5f));
        StartCoroutine(Utils.ChangeGraphicColor(progressBar, HIDEN, .5f));
        adIcon.SetActive(false);

        IsShowed = false;
    }

    private void LaunchProgressBar(float activeTime)
    {
        StartCoroutine(ProgressBarSetActive(true));
        StartCoroutine(ProgressBarStartCounting(activeTime));
    }

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

    private void OnButtonClicked()
    {
        Ad.Instance.ShowRewarded("slow-motion", Activate);
    }
}
