using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SessionMoney : Money
{
    [SerializeField] private Text lastResult;
    [SerializeField] private Text view;

    public static SessionMoney Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        ResetToZero();
    }

    private void Start()
    {
        Skyscraper.Instance.OnGameStart += Show;
    }

    public void PutOnTotalAccount()
    {
        ActionList al = new ActionList(GetComponent<ActionSequencer>());

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.CurrencySymbol = "$";
        lastResult.text = "+" + amount.ToString("C0", nfi);

        if (amount > 0)
        {
            al.AddWaiting(1f);
            al.Add(() => StartCoroutine(MoveToTotalAccount()));
        }
        al.Add(() => TapToPlayButton.Instance.ShowRestart());
        al.Add(() => Skyscraper.Instance.builtLock = false);

        al.Execute();
    }

    #region Private Functions

    private void ResetToZero()
    {
        RenderAmount(0);
        amount = 0;
    }

    private IEnumerator MoveToTotalAccount()
    {
        Vector2 startPosition = transform.localPosition;
        Vector2 destPosition = TotalMoney.Instance.transform.localPosition
            + Vector3.right * 150f;

        const float DURATION = .5f;

        for (float t = 0f; t < DURATION; t += Time.unscaledDeltaTime)
        {
            transform.localPosition = Vector3.Lerp(
                startPosition,
                destPosition,
                Utils.Curves.Aceleration.Down.Evaluate(t / DURATION));
            yield return null;
        }

        Hide();
        transform.localPosition = startPosition;

        TotalMoney.Instance.PutMoney(amount);
        ResetToZero();
    }

    private void Hide()
    {
        Color hiden = view.color;
        hiden.a = 0f;
        view.color = hiden;
    }
    

    private void Show()
    {
        Color showed = view.color;
        showed.a = 1f;
        view.color = showed;
    }

    #endregion
}
