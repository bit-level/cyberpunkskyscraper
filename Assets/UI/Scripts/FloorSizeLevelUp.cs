using UnityEngine;
using PlayerPrefs = RedefineYG.PlayerPrefs;

namespace CyberpunkSkyscraper
{
    public class FloorSizeLevelUp : Buyable
    {
#pragma warning disable 0649
        [SerializeField] int[] prices = new int[4];
        [SerializeField] ColorStates image;
        [SerializeField] AudioSource playOnBuy;
        [SerializeField] HintScreen hintScreen;
#pragma warning restore 0649

        private Product _product;
        private bool _mayBuy = false;

        private const string HINT_PREFS_KEY = "FloorSizeLevelUp::Hint";

        private void Start()
        {
            _product = GetComponent<Product>();
            UpdateProduct();
        }

        private void Update()
        {
            int walletAmount = TotalMoney.Instance.Amount;
            if (_mayBuy != _product.GetCost() <= walletAmount)
            {
                _mayBuy = _product.GetCost() <= walletAmount;
                image.SetState(_mayBuy ? ColorStates.State.Active : ColorStates.State.Inactive);
                if (_mayBuy) OnBecomeBuyable();
            }
        }

        public override void OnBuy()
        {
            playOnBuy.Play();
            Skyscraper.Instance.FirstFloorSizeLevelUp();
            UpdateProduct();
        }

        private void UpdateProduct()
        {
            var product = GetComponent<Product>();
            var skyscraper = Skyscraper.Instance;
            bool lastLevel = skyscraper.FirstFloorScaleIndex == skyscraper.LevelsCount - 1;
            if (!lastLevel)
                product.SetCost(prices[skyscraper.FirstFloorScaleIndex]);
            else
                Destroy(gameObject);
        }

        private void OnBecomeBuyable()
        {
            bool mustShowHint = !PlayerPrefs.HasKey(HINT_PREFS_KEY);
            if (mustShowHint)
            {
                ShowHint();
                PlayerPrefs.SetInt(HINT_PREFS_KEY, 1);
                Saver.Instance.Save();
            }
        }

        private void ShowHint()
        {
            hintScreen.Show();
        }
    }
}
