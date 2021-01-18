using System.Globalization;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Text textField;
#pragma warning restore 0649

    public static Money Instance { get; private set; }

    #region Private Fields

    private int _amount;
    private Coroutine _animCoroutine;
    #endregion

    #region Events

    public delegate void MethodContainer(int arg);
    public event MethodContainer OnPutMoney;
    #endregion

    #region Constants

    private const string PREFSKEY = "wallet";
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
        _amount = LoadWalletAmount();
        RenderAmount(_amount);
    }
    #endregion

    #region Public Functions

    public void PutMoney(int amount)
    {
        RenderAmount(_amount + amount);
        _amount += amount;
        Save();
        OnPutMoney(amount);
    }
    #endregion

    #region Private Functions

    private int LoadWalletAmount()
    {
        const string PREFSKEY = "wallet";
        int amount;

        if (PlayerPrefs.HasKey(PREFSKEY))
            amount = PlayerPrefs.GetInt(PREFSKEY);
        else
            PlayerPrefs.SetInt(PREFSKEY, amount = 0);

        return amount;
    }

    private void RenderAmount(int amount)
    {
        int oldValue = _amount;
        int newValue = amount;

        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(ValueChangeAnimation(oldValue, newValue));
    }

    private void SetTextFieldValue(int value)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.CurrencySymbol = "$";
        textField.text = value.ToString("C0", nfi);
    }
   
    private void Save()
    {
        PlayerPrefs.SetInt(PREFSKEY, _amount);
    }
    #endregion

    #region Coroutines

    private IEnumerator ValueChangeAnimation(int oldValue, int newValue)
    {
        const float DURATION = 1.5f;
       
        for (float t = 0f; t < DURATION; t += Time.deltaTime)
        {
            int value = Mathf.RoundToInt(Mathf.Lerp(oldValue, newValue, t / DURATION));
            SetTextFieldValue(value);
            yield return null;
        }

        SetTextFieldValue(newValue);
    }
    #endregion
}
