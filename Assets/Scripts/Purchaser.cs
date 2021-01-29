using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CyberpunkSkyscraper
{
    public class Purchaser : MonoBehaviour, IStoreListener
    {
        private class Item
        {
            public readonly string generalId;
            public readonly string appleStoreId;
            public readonly string googleStoreId;

            public Action PurchaseCallback;

            public Item(string generalId, string appleStoreId, string googleStoreId)
            {
                this.generalId = generalId;
                this.appleStoreId = appleStoreId;
                this.googleStoreId = googleStoreId;
            }

            public void OnPurchase()
            {
                if (PurchaseCallback != null)
                    PurchaseCallback.Invoke();
            }
        }

        private static IStoreController storeController;
        private static IExtensionProvider storeExtensionProvider;

        public static Purchaser Instance { get; private set; }

        private Item _noAds = new Item("no_ads", "no_ads", "no_ads");

        private void Awake()
        {
            Instance = this;
            if (storeController == null)
            {
                InitializePurchasing();
            }
        }

        private void Start()
        {
            _noAds.PurchaseCallback = new Action(() =>
            {
                NoAds.Activate();
            });
        }

        private void InitializePurchasing()
        {
            if (IsInitialized()) return;
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(_noAds.generalId, ProductType.NonConsumable, new IDs()
            {
                { _noAds.appleStoreId, AppleAppStore.Name },
                { _noAds.googleStoreId, GooglePlay.Name }
            });

            UnityPurchasing.Initialize(this, builder);
        }

        private static bool IsInitialized()
        {
            return storeController != null && storeExtensionProvider != null;
        }

        private static void BuyProductID(string productId)
        {
            if (!IsInitialized()) return;
            UnityEngine.Purchasing.Product product
                = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                print("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchases");
            }
        }

        public void BuyNoAds()
        {
            BuyProductID(_noAds.generalId);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            print("OnInitialized: PASS");

            storeController = controller;
            storeExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            print("OnInitializeFailed: InitializationFailureReason: " + error);
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
            print(string.Format(
                "OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
                product.definition.storeSpecificId, failureReason));
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            _noAds.OnPurchase();
            return PurchaseProcessingResult.Complete;
        }
    }
}

