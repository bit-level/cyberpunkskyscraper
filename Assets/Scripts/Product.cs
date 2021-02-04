using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Buyable buyable;
    [SerializeField] Text costLabel;
    [SerializeField] Text watchAd;
    [SerializeField] int cost = 0;
#pragma warning restore 0649

    private struct Cost
    {
        public readonly Text label;

        public readonly Color DefaultColor;
        public readonly Color NotEnoughMoney;

        public Cost(Text label)
        {
            this.label = label;
            DefaultColor = label.color;
            NotEnoughMoney = DefaultColor; NotEnoughMoney.a = .6f;
        }

        public void UpdateLabel(int costValue)
        {
            var nfi = new NumberFormatInfo();
            nfi.CurrencySymbol = "$";
            label.text = costValue.ToString("C0", nfi);
        }
    }

    private bool _used = false;
    private Cost _cost;

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        _cost = new Cost(costLabel);
        _cost.UpdateLabel(cost);
    }

    private void Start()
    {
        if (watchAd != null)
        {
            Ad.Instance.OnRewardedAdsSuccessfulWatch += () =>
            {
                GetComponent<SlowMotion>().Activate();
                _used = true;
            };
        }

        Skyscraper.Instance.OnGameStart += () => _used = false;
    }

    private void Update()
    {
        if (watchAd != null)
        {
            if (cost > TotalMoney.Instance.Amount && Ad.Instance.IsReady(Ad.Type.Rewarded) && !_used)
            {
                if (!watchAd.gameObject.activeSelf)
                {
                    watchAd.gameObject.SetActive(true);
                }
            }
            else if (watchAd.gameObject.activeSelf)
            {
                watchAd.gameObject.SetActive(false);
            }
        }

        if (cost > TotalMoney.Instance.Amount && SlowMotion.Instance.IsShowed)
        {
            _cost.label.color = _cost.NotEnoughMoney;
        }
    }
    #endregion

    #region Public Functions

    public void Buy()
    {
        if (cost <= TotalMoney.Instance.Amount)
        {
            TotalMoney.Instance.TakeMoney(cost);
            buyable.OnBuy();
        }
        else
        {
            if (watchAd != null && !_used)
                Ad.Instance.ShowIfReady(Ad.Type.Rewarded);
            else
                costLabel.GetComponent<Animation>().Play("NotEnoughMoney");
        }
    }

    public void SetCost(int value)
    {
        cost = value;
        _cost.UpdateLabel(value);
    }

    public int GetCost() => cost;
    #endregion
}

public abstract class Buyable : MonoBehaviour
{
    public abstract void OnBuy();
}
