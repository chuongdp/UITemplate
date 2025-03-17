namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.CollectionNew
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Models;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.IapScene;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using ServiceImplementation.IAPServices;
    using HyperGames.UnityTemplate.UnityTemplate.Extension;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateNewCollectionScreen : BaseView
    {
        public Button                    btnHome;
        public Button                    btnUnlockRandom;
        public Button                    btnAddMoreCoin;
        public TopButtonBarAdapter       topButtonBarAdapter;
        public ItemCollectionGridAdapter itemCollectionGridAdapter;
        public UnityTemplateCurrencyView    coinText;
    }

    [ScreenInfo(nameof(UnityTemplateNewCollectionScreen))]
    public class UnityTemplateNewCollectionScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateNewCollectionScreen>
    {
        private static readonly string placement = "Collection";

        protected readonly List<ItemCollectionItemModel> itemCollectionItemModels = new();

        protected readonly List<TopButtonItemModel> topButtonItemModels = new();

        private int         currentSelectedCategoryIndex;
        private IDisposable randomTimerDispose;

        [Preserve]
        public UnityTemplateNewCollectionScreenPresenter(
            SignalBus                         signalBus,
            ILogService                       logger,
            EventSystem                       eventSystem,
            IIapServices                      iapServices,
            UnityTemplateAdServiceWrapper        UnityTemplateAdServiceWrapper,
            IGameAssets                       gameAssets,
            IScreenManager                    screenManager,
            UnityTemplateCategoryItemBlueprint   unityTemplateCategoryItemBlueprint,
            UnityTemplateItemBlueprint           unityTemplateItemBlueprint,
            UnityTemplateInventoryDataController UnityTemplateInventoryDataController,
            UnityTemplateLevelDataController     levelDataController
        ) : base(signalBus, logger)
        {
            this.eventSystem                       = eventSystem;
            this.iapServices                       = iapServices;
            this.UnityTemplateAdServiceWrapper        = UnityTemplateAdServiceWrapper;
            this.gameAssets                        = gameAssets;
            this.ScreenManager                     = screenManager;
            this.unityTemplateCategoryItemBlueprint   = unityTemplateCategoryItemBlueprint;
            this.unityTemplateItemBlueprint           = unityTemplateItemBlueprint;
            this.UnityTemplateInventoryDataController = UnityTemplateInventoryDataController;
            this.levelDataController               = levelDataController;
        }

        protected virtual int CoinAddAmount => 500;

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.btnHome.onClick.AddListener(this.OnClickHomeButton);
            this.View.btnUnlockRandom.onClick.AddListener(this.OnClickUnlockRandomButton);
            this.View.btnAddMoreCoin.onClick.AddListener(this.OnClickAddMoreCoinButton);
        }

        public override async UniTask BindData()
        {
            this.itemCollectionItemModels.Clear();
            this.PrePareModel();
            await this.BindDataCollectionForAdapter();
            this.OnButtonCategorySelected(this.topButtonItemModels[0]);
        }

        protected virtual void OnClickAddMoreCoinButton()
        {
            this.UnityTemplateAdServiceWrapper.ShowRewardedAd(placement,
                this.BuyItemCompleted);
        }

        private async void BuyItemCompleted()
        {
            await this.UnityTemplateInventoryDataController.AddCurrency(this.CoinAddAmount, startAnimationRect: this.View.btnAddMoreCoin.transform as RectTransform);
            this.View.itemCollectionGridAdapter.Refresh();
        }

        protected virtual void OnClickUnlockRandomButton()
        {
            this.UnityTemplateAdServiceWrapper.ShowRewardedAd(placement,
                () =>
                {
                    this.eventSystem.enabled = false;
                    var currentCategory = this.unityTemplateCategoryItemBlueprint.ElementAt(this.currentSelectedCategoryIndex).Value.Id;

                    var collectionModel = this.itemCollectionItemModels
                        .Where(x => x.ItemBlueprintRecord.Category.Equals(currentCategory) && !this.UnityTemplateInventoryDataController.HasItem(x.ItemData.Id)).ToList();

                    foreach (var model in this.itemCollectionItemModels) model.IndexItemSelected = -1;

                    var maxTime = collectionModel.Count == 1 ? 0.3f : 3;

                    collectionModel.GachaItemWithTimer(this.randomTimerDispose,
                        model =>
                        {
                            foreach (var itemCollectionItemModel in collectionModel) itemCollectionItemModel.IndexItemSelected = model.ItemIndex;

                            this.OnRandomItemComplete(model);
                            this.BuyItemCompleted(model);
                            this.eventSystem.enabled = true;
                        },
                        model =>
                        {
                            foreach (var itemCollectionItemModel in collectionModel) itemCollectionItemModel.IndexItemSelected = model.ItemIndex;

                            this.View.itemCollectionGridAdapter.Refresh();
                        },
                        maxTime,
                        0.1f);
                });
        }

        protected virtual void OnRandomItemComplete(ItemCollectionItemModel model)
        {
        }

        protected virtual async void OnClickHomeButton()
        {
            await this.ScreenManager.OpenScreen<UnityTemplateHomeTapToPlayScreenPresenter>();
        }

        private void PrePareModel()
        {
            this.itemCollectionItemModels.Clear();

            var unlockType = this.levelDataController.UnlockedFeature;

            foreach (var record in this.unityTemplateItemBlueprint.Values)
            {
                var itemData = this.UnityTemplateInventoryDataController.GetItemData(record.Id, UnityTemplateItemData.Status.Unlocked);

                if ((itemData.ShopBlueprintRecord.UnlockType & unlockType) == 0) continue;

                var model = new ItemCollectionItemModel
                {
                    OnBuyItem = this.OnBuyItem, OnSelectItem = this.OnSelectItem, OnUseItem = this.OnUseItem, ItemData = itemData,
                };

                this.itemCollectionItemModels.Add(model);
            }
        }

        private async UniTask BindDataCollectionForAdapter()
        {
            //TopBar
            this.currentSelectedCategoryIndex = 0;
            this.topButtonItemModels.Clear();

            var index = 0;

            foreach (var record in this.unityTemplateCategoryItemBlueprint.Values)
            {
                var icon = await this.gameAssets.LoadAssetAsync<Sprite>(record.Icon);

                this.topButtonItemModels.Add(new() { Title = record.Title, Icon = icon, OnSelected = this.OnButtonCategorySelected, SelectedIndex = this.currentSelectedCategoryIndex, Index = index });

                index++;
            }

            //Collection
            for (var i = 0; i < this.unityTemplateCategoryItemBlueprint.Count; i++)
            {
                var currentCategory = this.unityTemplateCategoryItemBlueprint.ElementAt(i).Value.Id;
                var collectionModel = this.itemCollectionItemModels.Where(x => x.ItemBlueprintRecord.Category.Equals(currentCategory)).ToList();

                var currentItemUsed = this.UnityTemplateInventoryDataController.GetCurrentItemSelected(currentCategory);

                var indexUsed = collectionModel.FindIndex(x => x.ItemData.Id.Equals(currentItemUsed));

                for (var j = 0; j < collectionModel.Count; j++)
                {
                    var currentModel = collectionModel[j];
                    currentModel.ItemIndex     = j;
                    currentModel.IndexItemUsed = currentModel.IndexItemSelected = indexUsed == -1 ? 0 : indexUsed;
                }
            }
            this.RebindModelData();
            await this.View.topButtonBarAdapter.InitItemAdapter(this.topButtonItemModels);
        }

        protected virtual void RebindModelData()
        {
        }

        protected virtual async void OnButtonCategorySelected(TopButtonItemModel obj)
        {
            //refresh top button bar
            this.currentSelectedCategoryIndex = obj.Index;

            foreach (var topButtonItemModel in this.topButtonItemModels) topButtonItemModel.SelectedIndex = this.currentSelectedCategoryIndex;

            //Bind Data Collection
            var currentCategory = this.unityTemplateCategoryItemBlueprint.ElementAt(this.currentSelectedCategoryIndex).Value.Id;
            var tempModel       = this.itemCollectionItemModels.Where(x => x.ItemBlueprintRecord.Category.Equals(currentCategory)).ToList();

            await this.View.itemCollectionGridAdapter.InitItemAdapter(tempModel);
            this.View.topButtonBarAdapter.Refresh();
            var hasOwnAllItem = tempModel.All(x => this.UnityTemplateInventoryDataController.HasItem(x.ItemData.Id));
            this.View.btnUnlockRandom.gameObject.SetActive(!hasOwnAllItem);
        }

        private void OnUseItem(ItemCollectionItemModel obj)
        {
            // If the item is not owned, do not use it
            if (!this.UnityTemplateInventoryDataController.TryGetItemData(obj.ItemData.Id, out var itemData) || itemData.CurrentStatus != UnityTemplateItemData.Status.Owned) return;

            var currentCategory = this.unityTemplateCategoryItemBlueprint.ElementAt(this.currentSelectedCategoryIndex).Value.Id;
            var tempModel       = this.itemCollectionItemModels.Where(x => x.ItemBlueprintRecord.Category.Equals(currentCategory)).ToList();

            foreach (var model in tempModel) model.IndexItemUsed = model.IndexItemSelected = obj.ItemIndex;

            // Save the selected item
            this.UnityTemplateInventoryDataController.UpdateCurrentSelectedItem(currentCategory, obj.ItemData.Id);
            this.OnUsedItem(obj.ItemData);

            this.View.itemCollectionGridAdapter.Refresh();
        }

        protected virtual void OnUsedItem(UnityTemplateItemData itemData)
        {
        }

        private void OnSelectItem(ItemCollectionItemModel obj)
        {
            var currentCategory = this.unityTemplateCategoryItemBlueprint.ElementAt(this.currentSelectedCategoryIndex).Value.Id;
            var tempModel       = this.itemCollectionItemModels.Where(x => x.ItemBlueprintRecord.Category.Equals(currentCategory)).ToList();

            foreach (var model in tempModel) model.IndexItemSelected = obj.ItemIndex;

            // Save the selected item
            this.OnSelectedItem(obj.ItemData);

            this.View.itemCollectionGridAdapter.Refresh();
        }

        protected virtual void OnSelectedItem(UnityTemplateItemData itemData)
        {
        }

        private void OnBuyItem(ItemCollectionItemModel obj)
        {
            switch (obj.ShopBlueprintRecord.UnlockType)
            {
                case UnityTemplateItemData.UnlockType.Ads:
                    this.BuyWithAds(obj);

                    break;
                case UnityTemplateItemData.UnlockType.SoftCurrency:
                    this.BuyWithSoftCurrency(obj);

                    break;
                case UnityTemplateItemData.UnlockType.None: break;
                case UnityTemplateItemData.UnlockType.IAP:
                    this.BuyWithIAP(obj);

                    break;
                case UnityTemplateItemData.UnlockType.StartedPack:
                    this.BuyWithStartedPack(obj);

                    break;

                case UnityTemplateItemData.UnlockType.Progression: break;
                case UnityTemplateItemData.UnlockType.Gift:        break;
                case UnityTemplateItemData.UnlockType.DailyReward:
                    this.BuyWithDailyReward(obj);

                    break;
                case UnityTemplateItemData.UnlockType.LuckySpin:
                    this.BuyWithLuckySpin(obj);

                    break;
                case UnityTemplateItemData.UnlockType.All: break;
                default:                                throw new ArgumentOutOfRangeException();
            }
        }

        private void BuyWithStartedPack(ItemCollectionItemModel itemCollectionItemModel)
        {
            this.ScreenManager.OpenScreen<UnityTemplateStartPackScreenPresenter, UnityTemplateStaterPackModel>(new()
            {
                OnComplete = this.OnBuyStartedPackComplete,
            });
        }

        protected virtual void OnBuyStartedPackComplete(string packId)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            this.randomTimerDispose?.Dispose();
        }

        #region inject

        private readonly   UnityTemplateCategoryItemBlueprint   unityTemplateCategoryItemBlueprint;
        private readonly   UnityTemplateItemBlueprint           unityTemplateItemBlueprint;
        private readonly   UnityTemplateInventoryDataController UnityTemplateInventoryDataController;
        private readonly   EventSystem                       eventSystem;
        private readonly   IIapServices                      iapServices;
        private readonly   UnityTemplateAdServiceWrapper        UnityTemplateAdServiceWrapper;
        private readonly   IGameAssets                       gameAssets;
        private readonly   UnityTemplateLevelDataController     levelDataController;
        protected readonly IScreenManager                    ScreenManager;

        #endregion

        #region Buy Item

        protected virtual void BuyWithDailyReward(ItemCollectionItemModel obj)
        {
            // _ = this.UnityTemplateDailyRewardService.ShowDailyRewardPopupAsync(true);
        }

        protected virtual void BuyWithLuckySpin(ItemCollectionItemModel obj)
        {
            // this.UnityTemplateLuckySpinServices.OpenLuckySpin();
        }

        protected virtual void BuyWithSoftCurrency(ItemCollectionItemModel obj, Action onFail = null)
        {
            var currentCoin = this.UnityTemplateInventoryDataController.GetCurrencyValue(obj.ShopBlueprintRecord.CurrencyID);

            if (currentCoin < obj.ShopBlueprintRecord.Price)
            {
                this.Logger.Log($"Not Enough {obj.ShopBlueprintRecord.CurrencyID}\nCurrent: {currentCoin}, Needed: {obj.ShopBlueprintRecord.Price}");
                onFail?.Invoke();
                return;
            }

            this.UnityTemplateInventoryDataController.AddCurrency(-obj.ShopBlueprintRecord.Price, obj.ShopBlueprintRecord.CurrencyID).Forget();
            this.BuyItemCompleted(obj);
        }

        private void BuyWithAds(ItemCollectionItemModel obj)
        {
            this.UnityTemplateAdServiceWrapper.ShowRewardedAd(placement,
                () =>
                {
                    this.BuyItemCompleted(obj);
                });
        }

        private void BuyWithIAP(ItemCollectionItemModel obj)
        {
            this.iapServices.BuyProductID(obj.ShopBlueprintRecord.Id,
                x =>
                {
                    this.BuyItemCompleted(obj);
                });
        }

        private void BuyItemCompleted(ItemCollectionItemModel obj)
        {
            obj.ItemData.RemainingAdsProgress--;

            if (obj.ItemData.RemainingAdsProgress > 0) return;
            this.UnityTemplateInventoryDataController.SetOwnedItemData(obj.ItemData, true);
            this.OnUseItem(obj);
        }

        #endregion
    }
}