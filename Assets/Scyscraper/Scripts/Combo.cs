using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
#pragma warning disable 0649
    [Header("References")]
    [SerializeField] Text amountText;
    [SerializeField] Text multiplierText;
    [SerializeField] Text resultText;
    [SerializeField] Transform wallet;

    [Header("Settings")]
    [SerializeField] float coolingTime = 10f;
    [SerializeField] int maximumMultiplier = 5;
#pragma warning restore 0649

    private struct Item
    {
        public readonly Text Self;
        public readonly Color DefaultColor;
        public readonly Color HidenColor;
        public int Value;

        public Item(Text self, Color defaultColor)
        {
            Self = self;
            DefaultColor = defaultColor;

            Color hiden = defaultColor; hiden.a = 0f;
            HidenColor = hiden;

            Value = 0;
        }
    }

    public static Combo Instance { get; private set; }

    #region Private Fields

    private Item _amount, _multiplier;
    private bool _isComboStarted = false;
    private int _multiplierValue = 0;
    private float _coolingTimer;
    private Coroutine _cooling, _result;
    private int _resultMoney;
    #endregion

    #region Events

    public delegate void MethodContainer();
    public event MethodContainer OnComboEnd;
    #endregion

    #region Monobehaviour Callbacks

    private void Awake()
    {
        Instance = this;

        _amount = new Item(amountText, amountText.color);
        _multiplier = new Item(multiplierText, multiplierText.color);

        Hide(0f);
    }

    private void Start()
    {
        Skyscraper.Instance.OnPerfectTap += RiseCombo;

        Skyscraper.Instance.OnGameOver += (score, bestScore) =>
        {
            if (_cooling != null || _result != null)
            {
                if (_cooling != null)
                {
                    StopCoroutine(_cooling);
                    _cooling = null;

                }

                if (_result != null)
                {
                    StopCoroutine(_result);
                    _result = null;
                    resultText.gameObject.SetActive(false);
                }

                Hide(0f);
                int money = _multiplierValue * _amount.Value;
                SessionMoney.Instance.PutMoney(money, false);
            }

            Hide(0f);
            SessionMoney.Instance.PutOnTotalAccount();

            _isComboStarted = false;
            _multiplier.Self.text = "x1";
            _multiplierValue = 0;
        };
    }
    #endregion

    #region Private Functions

    private void StartCombo()
    {
        _multiplierValue = 1;
        Show(0f);
        _isComboStarted = true;

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.CurrencySymbol = "$";
        _amount.Value = (Skyscraper.Instance.FloorsCount+1) * 100;
        _amount.Self.text = _amount.Value.ToString("C0", nfi);
    }

    private void RiseCombo()
    {
        _coolingTimer = 0f;

        if (!_isComboStarted)
        {
            if (_result != null)
            {
                SkipResultAnim();
            }

            StartCombo();
            _cooling = StartCoroutine(Cooling());
            return;
        }

        _multiplierValue = Mathf.Clamp(_multiplierValue + 1, 1, maximumMultiplier);
        _multiplier.Self.text = "x" + _multiplierValue.ToString();
        _multiplier.Self.GetComponent<Animation>().Play("Rise Combo");

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.CurrencySymbol = "$";
        _amount.Value = (Skyscraper.Instance.FloorsCount+1) * 100;
        _amount.Self.text = _amount.Value.ToString("C0", nfi);
    }

    private void StopCombo()
    {
        _isComboStarted = false;
        _result = StartCoroutine(Result(_multiplierValue * _amount.Value));
        OnComboEnd();
    }

    private void SkipResultAnim()
    {
        StopCoroutine(_result);
        _result = null;
        SessionMoney.Instance.PutMoney(_resultMoney);
        resultText.gameObject.SetActive(false);
    }

    private void Hide(float duration)
    {
        if (duration == 0f)
        {
            amountText.gameObject.SetActive(false);
            multiplierText.gameObject.SetActive(false);
        }
        else throw new NotImplementedException();
    }

    private void Show(float duration)
    {
        _amount.Self.gameObject.SetActive(true);
        _multiplier.Self.gameObject.SetActive(true);

        if (duration == 0f)
        {
            _amount.Self.color = _amount.DefaultColor;
            _multiplier.Self.color = _multiplier.DefaultColor;
        }
        else
        {
            StartCoroutine(Utils.ChangeGraphicColor(_amount.Self, _amount.Self.color, _amount.DefaultColor, duration));
            StartCoroutine(Utils.ChangeGraphicColor(_multiplier.Self, _multiplier.Self.color, _multiplier.DefaultColor, duration));
        }
    }
    #endregion

    #region Coroutines

    private IEnumerator Cooling()
    {
        _coolingTimer = 0f;

        for (; _coolingTimer < coolingTime; _coolingTimer += Time.deltaTime)
        {
            _amount.Self.color = Color.Lerp(_amount.DefaultColor, _amount.HidenColor, _coolingTimer / coolingTime);
            _multiplier.Self.color = Color.Lerp(_multiplier.DefaultColor, _multiplier.HidenColor, _coolingTimer / coolingTime);
            yield return null;
        }

        StopCombo();

        _cooling = null;
    }

    private IEnumerator Result(int resultMoney)
    {
        _resultMoney = resultMoney;

        Show(0f);

        Vector2 startPosition = _multiplier.Self.transform.localPosition;
        Vector2 destPosition = _amount.Self.transform.localPosition;

        float duration = .8f;

        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            _multiplier.Self.transform.localPosition = Vector2.Lerp(
                startPosition,
                destPosition,
                Utils.Curves.Aceleration.Up.Evaluate(t / duration));
            yield return null;
        }

        Hide(0f);
        _multiplier.Self.transform.localPosition = startPosition;
        _multiplier.Self.text = "x1";

        resultText.text = "$" + resultMoney;
        resultText.gameObject.SetActive(true);
        resultText.GetComponent<Animation>().Play("Show Result");

        yield return new WaitForSecondsRealtime(.5f);

        duration = .4f;
        startPosition = resultText.transform.localPosition;

        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            resultText.rectTransform.localPosition = Vector3.Lerp(
                startPosition,
                wallet.localPosition,
                Utils.Curves.Aceleration.Down.Evaluate(t / duration));
            yield return null;
        }

        resultText.gameObject.SetActive(false);
        resultText.transform.localPosition = startPosition;

        SessionMoney.Instance.PutMoney(resultMoney);

        _result = null;
    }
    #endregion
}
