using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public abstract class Money : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Text textField;
#pragma warning restore 0649

    #region Fields

    protected int amount;
    private Coroutine _animCoroutine;
    #endregion

    #region Properties

    public int Amount => amount;
    #endregion

    #region Functions

    public void PutMoney(int amount, bool animation = true)
    {
        int newAmountValue = this.amount + amount;

        if (animation) RenderAmount(newAmountValue);
        else SetTextFieldValue(newAmountValue);

        this.amount = newAmountValue;
    }

    public bool TakeMoney(int amount)
    {
        if (amount > this.amount) return false;
        RenderAmount(this.amount - amount);
        this.amount -= amount;
        return true;
    }

    protected void RenderAmount(int amount)
    {
        int oldValue = this.amount;
        int newValue = amount;

        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(ValueChangeAnimation(oldValue, newValue));
    }
    
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

    private void SetTextFieldValue(int value)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.CurrencySymbol = "$";
        textField.text = value.ToString("C0", nfi);
    }
    #endregion
}
