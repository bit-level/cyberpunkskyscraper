using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    [SerializeField] private Buyable buyable;
    [SerializeField] private Text costLabel;
    [SerializeField] private int cost = 0;

    private Cost _cost;

    private void Awake()
    {
        _cost = new Cost(costLabel);
        _cost.UpdateLabel(cost);
    }

    private void Update()
    {
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
            buyable.OnBuy();
        }
        else
            costLabel.GetComponent<Animation>().Play("NotEnoughMoney");
    }

    public void SetCost(int value)
    {
        cost = value;
        _cost.UpdateLabel(value);
    }

    public int GetCost() => cost;

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
}