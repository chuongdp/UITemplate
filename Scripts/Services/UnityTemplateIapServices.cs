namespace HyperGames.UnityTemplate.UnityTemplate.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlueprintFlow.Signals;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using ServiceImplementation.IAPServices;
    using ServiceImplementation.IAPServices.Signals;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateIapServices : IInitializable, IDisposable
    {
        #region inject

        private readonly SignalBus                            signalBus;
        private readonly ILogService                          logger;
        private readonly UnityTemplateIAPOwnerPackControllerData UnityTemplateIAPOwnerPackControllerData;
        private readonly UnityTemplateShopPackBlueprint          unityTemplateShopPackBlueprint;
        private readonly IIapServices                         iapServices;
        private readonly UnityTemplateRewardHandler              UnityTemplateRewardHandler;

        #endregion

        [Preserve]
        public UnityTemplateIapServices(
            SignalBus                            signalBus,
            ILogService                          logger,
            UnityTemplateIAPOwnerPackControllerData UnityTemplateIAPOwnerPackControllerData,
            UnityTemplateShopPackBlueprint          unityTemplateShopPackBlueprint,
            IIapServices                         iapServices,
            UnityTemplateRewardHandler              UnityTemplateRewardHandler
        )
        {
            this.signalBus                            = signalBus;
            this.logger                               = logger;
            this.UnityTemplateIAPOwnerPackControllerData = UnityTemplateIAPOwnerPackControllerData;
            this.unityTemplateShopPackBlueprint          = unityTemplateShopPackBlueprint;
            this.iapServices                          = iapServices;
            this.UnityTemplateRewardHandler              = UnityTemplateRewardHandler;
        }

        private void OnBlueprintLoaded(LoadBlueprintDataSucceedSignal obj)
        {
            var dicData = new Dictionary<string, IAPModel>();

            foreach (var record in this.unityTemplateShopPackBlueprint.GetPack())
            {
                dicData.Add(record.Id,
                    new()
                    {
                        Id          = record.Id,
                        ProductType = record.ProductType,
                    });
            }

            this.iapServices.InitIapServices(dicData);
        }

        public void BuyProduct(GameObject source, string productId, Action<string> onComplete = null, Action<string> onFail = null)
        {
            this.logger.Warning($"BuyProduct {productId}");

            this.iapServices.BuyProductID(productId,
                (x) =>
                {
                    this.OnPurchaseComplete(productId, source);
                    onComplete?.Invoke(x);
                },
                onFail);
        }

        private void OnPurchaseComplete(string productId, GameObject source)
        {
            var dataShopPackRecord = this.unityTemplateShopPackBlueprint[productId];
            this.UnityTemplateIAPOwnerPackControllerData.AddPack(productId);

            var rewardItemData = dataShopPackRecord.RewardIdToRewardDatas.ToDictionary(keyPairValue => keyPairValue.Key,
                keyPairValue => new UnityTemplateRewardItemData(keyPairValue.Value.RewardValue, keyPairValue.Value.Repeat, keyPairValue.Value.AddressableFlyingItem));

            if (rewardItemData.Count > 0) this.UnityTemplateRewardHandler.AddRewardsWithPackId(productId, rewardItemData, source);
        }

        private void OnHandleRestorePurchase(OnRestorePurchaseCompleteSignal obj)
        {
            this.OnPurchaseComplete(obj.ProductID, null);
        }

        public void RestorePurchase(Action onComplete = null)
        {
            this.iapServices.RestorePurchases(onComplete);
        }

        public bool IsProductOwned(string productId = "")
        {
            //Todo check with pack ID
            return this.unityTemplateShopPackBlueprint.Values.Where(x => x.RewardIdToRewardDatas.Count > 1).Any(shopPackRecord => this.UnityTemplateIAPOwnerPackControllerData.IsOwnerPack(shopPackRecord.Id));
        }

        public ProductData GetProductData(string productId)
        {
            return this.iapServices.GetProductData(productId);
        }

        public string GetPriceById(string productId, string defaultPrice)
        {
            return this.iapServices.GetPriceById(productId, defaultPrice);
        }

        public void Initialize()
        {
            this.signalBus.Subscribe<LoadBlueprintDataSucceedSignal>(this.OnBlueprintLoaded);
            this.signalBus.Subscribe<OnRestorePurchaseCompleteSignal>(this.OnHandleRestorePurchase);
        }

        public void Dispose()
        {
            this.signalBus.Unsubscribe<LoadBlueprintDataSucceedSignal>(this.OnBlueprintLoaded);
            this.signalBus.Unsubscribe<OnRestorePurchaseCompleteSignal>(this.OnHandleRestorePurchase);
        }
    }
}