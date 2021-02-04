using UnityEngine;

namespace CyberpunkSkyscraper
{
    public class FloorSizeLevelUp : Buyable
    {
#pragma warning disable 0649
        [SerializeField] int[] prices = new int[4];
        [SerializeField] ColorStates image;
        [SerializeField] AudioSource playOnBuy;
#pragma warning restore 0649

        private Product _product;

        private void Start()
        {
            _product = GetComponent<Product>();
            UpdateProduct();
        }

        private void Update()
        {
            int walletAmount = TotalMoney.Instance.Amount;
            bool mayBuy = _product.GetCost() <= walletAmount;
            image.SetState(mayBuy ? ColorStates.State.Active : ColorStates.State.Inactive);
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
    }
}
