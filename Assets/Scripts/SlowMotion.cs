using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SlowMotion : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Image buttonImage;
    [SerializeField] Image progressBar;
    [SerializeField] float activeTime = 5f;
    [SerializeField] float timeScale = .5f;
#pragma warning restore 0649

    private bool state = false;

    private readonly Color ACTIVE = new Color(1f, 1f, 1f, 1f);
    private readonly Color DISACTIVE = new Color(.5f, .5f, .5f, 1f);
    private readonly Color HIDEN = new Color(1f, 1f, 1f, 0f);

    private void Update()
    {
        if (state == false && Skyscraper.Instance.CurrentState == Skyscraper.State.UnderBuild)
            Show();
        else if (state == true && Skyscraper.Instance.CurrentState == Skyscraper.State.Built)
        {
            Hide();
            Time.timeScale = 1f;
        }
    }

    public void OnClick()
    {
        ActionList al = new ActionList(GetComponent<ActionSequencer>());

        al.Add(() =>
        {
            Time.timeScale = .5f;
            GetComponent<Button>().interactable = false;
            LaunchProgressBar(activeTime);
        }, .3f);

        al.Add(() =>
        {
            StartCoroutine(ImageSetColor(buttonImage, DISACTIVE));
        }, activeTime - .3f);

        al.Add(() =>
        {
            Time.timeScale = 1f;
            GetComponent<Button>().interactable = true;
            StartCoroutine(ImageSetColor(buttonImage, ACTIVE));
        });

        al.Execute();
    }

    public void Show()
    {
        state = true;
        GetComponent<Button>().interactable = true;
        StartCoroutine(ImageSetColor(buttonImage, ACTIVE));
    }

    public void Hide()
    {
        state = false;
        GetComponent<Button>().interactable = false;
        StartCoroutine(ImageSetColor(buttonImage, HIDEN));
        StartCoroutine(ImageSetColor(progressBar, HIDEN));
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

    private IEnumerator ImageSetColor(Image image, Color newColor)
    {
        Color startColor = buttonImage.color;
        float duration = .3f;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            image.color = Color.Lerp(startColor, newColor, t / duration);
            yield return null;
        }

        image.color = newColor;
    }
}
