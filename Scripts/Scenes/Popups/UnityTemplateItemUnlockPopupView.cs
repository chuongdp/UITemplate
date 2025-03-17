namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Popups
{
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
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateItemUnlockPopupModel
    {
        public UnityTemplateItemUnlockPopupModel(string itemId)
        {
            this.ItemId = itemId;
        }

        public string ItemId { get; set; }
    }

    public class UnityTemplateItemUnlockPopupView : BaseView
    {
        [SerializeField] private UnityTemplateCurrencyView currencyView;
        [SerializeField] private Image                  imgItem;
        [SerializeField] private Button                 btnHome;
        [SerializeField] private Button                 btnSkip;
        [SerializeField] private UnityTemplateAdsButton    btnGet;

        public UnityTemplateCurrencyView CurrencyView => this.currencyView;
        public Image                  ImgItem      => this.imgItem;
        public Button                 BtnHome      => this.btnHome;
        public UnityTemplateAdsButton    BtnGet       => this.btnGet;
        public Button                 BtnSkip      => this.btnSkip;
    }

    [PopupInfo(nameof(UnityTemplateItemUnlockPopupView))]
    public class UnityTemplateItemUnlockPopupPresenter : UnityTemplateBasePopupPresenter<UnityTemplateItemUnlockPopupView, UnityTemplateItemUnlockPopupModel>
    {
        #region Inject

        protected readonly IScreenManager                    screenManager;
        protected readonly IGameAssets                       gameAssets;
        protected readonly UnityTemplateAdServiceWrapper        adService;
        protected readonly UnityTemplateItemBlueprint           itemBlueprint;
        protected readonly UnityTemplateInventoryDataController inventoryDataController;

        [Preserve]
        public UnityTemplateItemUnlockPopupPresenter(
            SignalBus                         signalBus,
            ILogService                       logService,
            IScreenManager                    screenManager,
            IGameAssets                       gameAssets,
            UnityTemplateAdServiceWrapper        adService,
            UnityTemplateItemBlueprint           itemBlueprint,
            UnityTemplateInventoryDataController inventoryDataController
        ) : base(signalBus, logService)
        {
            this.screenManager           = screenManager;
            this.gameAssets              = gameAssets;
            this.adService               = adService;
            this.itemBlueprint           = itemBlueprint;
            this.inventoryDataController = inventoryDataController;
        }

        #endregion

        protected virtual string AdPlacement => "unlock";

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.BtnGet.OnViewReady(this.adService);
            this.InitButtonListener();
        }

        public override async UniTask BindData(UnityTemplateItemUnlockPopupModel popupModel)
        {
            this.View.BtnGet.BindData(this.AdPlacement);
            var itemImageAddress = this.itemBlueprint.Values.First(record => record.Id.Equals(popupModel.ItemId)).ImageAddress;
            var itemSprite       = await this.gameAssets.LoadAssetAsync<Sprite>(itemImageAddress);
            this.View.ImgItem.sprite = itemSprite;
        }

        public override void Dispose()
        {
            this.View.BtnGet.Dispose();
        }

        protected virtual void OnClickHome()
        {
            this.screenManager.OpenScreen<UnityTemplateHomeSimpleScreenPresenter>();
        }

        protected virtual void OnClickGet()
        {
            if (!this.adService.IsRewardedAdReady("")) return;
            this.adService.ShowRewardedAd("",
                () =>
                {
                    this.inventoryDataController.UpdateStatusItemData(this.Model.ItemId, UnityTemplateItemData.Status.Owned);
                    this.CloseView();
                });
        }

        protected virtual void OnClickSkip()
        {
            this.CloseView();
        }

        private void InitButtonListener()
        {
            this.View.BtnHome.onClick.AddListener(this.OnClickHome);
            this.View.BtnGet.onClick.AddListener(this.OnClickGet);
            this.View.BtnSkip.onClick.AddListener(this.OnClickSkip);
        }
    }
}