using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
#pragma warning disable 0649
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
    }
    
    private NumberFormatInfo _nfi;
    private bool _used = false;
    private Cost _cost;

    private void Awake()
    {
        _nfi = new NumberFormatInfo();
        _nfi.CurrencySymbol = "$";

        costLabel.text = cost.ToString("C0", _nfi);

        _cost = new Cost(costLabel);
    }

    private void Start()
    {
        Ad.Instance.OnRewardedAdsSuccessfulWatch += () =>
        {
            print("Rewarded ads watched");
            GetComponent<SlowMotion>().Activate();
            _used = true;
        };

        Skyscraper.Instance.OnGameStart += () => _used = false;
    }

    private void Update()
    {
        if (cost >= TotalMoney.Instance.Amount && Ad.Instance.IsReady(Ad.Type.Rewarded) && !_used)
        {
            if (!watchAd.gameObject.activeSelf)
            {
                print("Activate");
                watchAd.gameObject.SetActive(true);
            }
        }
        else if (watchAd.gameObject.activeSelf)
        {
            print("Deactivate");
            watchAd.gameObject.SetActive(false);
        }

        if (cost > TotalMoney.Instance.Amount && SlowMotion.Instance.IsShowed)
        {
            _cost.label.color = _cost.NotEnoughMoney;
        }
    }

    public void Buy()
    {
        if (cost <= TotalMoney.Instance.Amount)
        {
            TotalMoney.Instance.TakeMoney(cost);
            GetComponent<SlowMotion>().Activate();
        }
        else
        {
            if (!_used)
                Ad.Instance.ShowIfReady(Ad.Type.Rewarded);
            else
                costLabel.GetComponent<Animation>().Play("NotEnoughMoney");
        }
    }
}
