namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.CollectionNew
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using HyperGames.UnityTemplate.UnityTemplate.Models;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Extension;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class ItemCollectionItemModel
    {
        public int                  ItemIndex           { get; set; }
        public int                  IndexItemUsed       { get; set; }
        public int                  IndexItemSelected   { get; set; }
        public UnityTemplateItemRecord ItemBlueprintRecord => this.ItemData.ItemBlueprintRecord;
        public UnityTemplateShopRecord ShopBlueprintRecord => this.ItemData.ShopBlueprintRecord;
        public UnityTemplateItemData   ItemData            { get; set; }

        public Action<ItemCollectionItemModel> OnSelectItem { get; set; }
        public Action<ItemCollectionItemModel> OnBuyItem    { get; set; }
        public Action<ItemCollectionItemModel> OnUseItem    { get; set; }
    }

    public class ItemCollectionItemView : TViewMono
    {
        public GameObject      objChoose, objNormal, objUsed, objStaredPack, objChooseStaredPack;
        public Image           imgIcon,   imgLockBuyCoin;
        public TextMeshProUGUI txtPrice;
        public Button          btnBuyCoin, btnBuyAds, btnBuyIap, btnDailyReward, btnLuckySpin, btnSelect, btnUse, btnStartPack;

        public Action OnBuyCoin, OnBuyAds, OnBuyIap, OnSelect, OnUse, OnBuyDailyReward, OnBuyLuckySpin, OnBuyStartPack;

        private void Awake()
        {
            this.btnBuyCoin.onClick.AddListener(() =>
            {
                this.OnBuyCoin?.Invoke();
            });
            this.btnBuyAds.onClick.AddListener(() =>
            {
                this.OnBuyAds?.Invoke();
            });
            this.btnBuyIap.onClick.AddListener(() =>
            {
                this.OnBuyIap?.Invoke();
            });
            this.btnSelect.onClick.AddListener(() =>
            {
                this.OnSelect?.Invoke();
            });
            this.btnUse.onClick.AddListener(() =>
            {
                this.OnUse?.Invoke();
            });
            this.btnDailyReward.onClick.AddListener(() =>
            {
                this.OnBuyDailyReward?.Invoke();
            });
            this.btnLuckySpin.onClick.AddListener(() =>
            {
                this.OnBuyLuckySpin?.Invoke();
            });
            this.btnStartPack.onClick.AddListener(() =>
            {
                this.OnBuyStartPack?.Invoke();
            });
        }
    }

    public class ItemCollectionItemPresenter : BaseUIItemPresenter<ItemCollectionItemView, ItemCollectionItemModel>
    {
        private readonly UnityTemplateCollectionItemViewHelper UnityTemplateCollectionItemViewHelper;

        [Preserve]
        public ItemCollectionItemPresenter(
            IGameAssets                        gameAssets,
            UnityTemplateInventoryDataController  UnityTemplateInventoryDataController,
            UnityTemplateCollectionItemViewHelper UnityTemplateCollectionItemViewHelper
        ) : base(gameAssets)
        {
            this.UnityTemplateCollectionItemViewHelper = UnityTemplateCollectionItemViewHelper;
        }

        public override void BindData(ItemCollectionItemModel param)
        {
            this.UnityTemplateCollectionItemViewHelper.BindDataItem(param, this.View);
        }

        public override void Dispose()
        {
            this.UnityTemplateCollectionItemViewHelper.DisposeItem(this.View);
            base.Dispose();
        }
    }
}