namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Play.End
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
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
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateWinScreenModel
    {
        public readonly string ItemId;
        public readonly float  ItemUnlockLastValue;
        public readonly float  ItemUnlockNewValue;
        public readonly int    StarRate;

        public UnityTemplateWinScreenModel(string itemId, float itemUnlockLastValue, float itemUnlockNewValue, int starRate = 3)
        {
            this.ItemId              = itemId;
            this.ItemUnlockLastValue = itemUnlockLastValue;
            this.ItemUnlockNewValue  = itemUnlockNewValue;
            this.StarRate            = starRate;
        }
    }

    public class UnityTemplateWinScreenView : BaseView
    {
        [SerializeField] private Button                 btnHome;
        [SerializeField] private Button                 btnReplay;
        [SerializeField] private Button                 btnNext;
        [SerializeField] private UnityTemplateAdsButton    btnAds;
        [SerializeField] private UnityTemplateCurrencyView currencyView;

        [SerializeField] private bool useItemUnlockProgressText;

        [SerializeField] [ShowIf("useItemUnlockProgressText")] private TMP_Text txtItemUnlockProgress;

        [SerializeField] private bool useItemUnlockProgressImage;

        [SerializeField] [ShowIf("useItemUnlockProgressImage")] private Image imgItemUnlockProgress;

        [SerializeField] [ShowIf("useItemUnlockProgressImage")] private Image imgItemUnlockProgressBackground;

        [SerializeField] private bool useItemUnlockProgressSlider;

        [SerializeField] [ShowIf("useItemUnlockProgressSlider")] private Slider sliderItemUnlockProgress;

        [SerializeField] private bool useLightGlow;

        [SerializeField] [ShowIf("useLightGlow")] private Image imgLightGlow;

        [SerializeField] private bool useStarRate;

        [SerializeField] [ShowIf("UseStarRate")] private UnityTemplateStarRateView starRateView;

        public Button                 BtnHome                         => this.btnHome;
        public Button                 BtnReplay                       => this.btnReplay;
        public Button                 BtnNext                         => this.btnNext;
        public UnityTemplateAdsButton    BtnAds                          => this.btnAds;
        public UnityTemplateCurrencyView CurrencyView                    => this.currencyView;
        public bool                   UseItemUnlockProgressText       => this.useItemUnlockProgressText;
        public TMP_Text               TxtItemUnlockProgress           => this.txtItemUnlockProgress;
        public bool                   UseItemUnlockProgressImage      => this.useItemUnlockProgressImage;
        public Image                  ImgItemUnlockProgress           => this.imgItemUnlockProgress;
        public Image                  ImgItemUnlockProgressBackground => this.imgItemUnlockProgressBackground;
        public bool                   UseItemUnlockProgressSlider     => this.useItemUnlockProgressSlider;
        public Slider                 SliderItemUnlockProgress        => this.sliderItemUnlockProgress;
        public bool                   UseLightGlow                    => this.useLightGlow;
        public Image                  ImgLightGlow                    => this.imgLightGlow;
        public bool                   UseStarRate                     => this.useStarRate;
        public UnityTemplateStarRateView StarRateView                    => this.starRateView;
    }

    [ScreenInfo(nameof(UnityTemplateWinScreenView))]
    public class UnityTemplateWinScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateWinScreenView, UnityTemplateWinScreenModel>
    {
        #region Inject

        protected readonly IScreenManager                    screenManager;
        protected readonly IGameAssets                       gameAssets;
        protected readonly UnityTemplateInventoryDataController inventoryDataController;
        protected readonly UnityTemplateSoundServices           soundService;
        protected readonly UnityTemplateAdServiceWrapper        adService;

        [Preserve]
        public UnityTemplateWinScreenPresenter(
            SignalBus                         signalBus,
            ILogService                       logService,
            IScreenManager                    screenManager,
            IGameAssets                       gameAssets,
            UnityTemplateInventoryDataController inventoryDataController,
            UnityTemplateSoundServices           soundService,
            UnityTemplateAdServiceWrapper        adService
        ) : base(signalBus, logService)
        {
            this.screenManager           = screenManager;
            this.gameAssets              = gameAssets;
            this.inventoryDataController = inventoryDataController;
            this.soundService            = soundService;
            this.adService               = adService;
        }

        #endregion

        protected virtual string      AdPlacement => "x2_reward";
        private           IDisposable spinDisposable;
        private           Tween       tweenSpin;

        protected override void OnViewReady()
        {
            base.OnViewReady();
            if (this.View.BtnAds != null) this.View.BtnAds.OnViewReady(this.adService);
            if (this.View.BtnHome != null) this.View.BtnHome.onClick.AddListener(this.OnClickHome);
            if (this.View.BtnReplay != null) this.View.BtnReplay.onClick.AddListener(this.OnClickReplay);
            if (this.View.BtnNext != null) this.View.BtnNext.onClick.AddListener(this.OnClickNext);
            if (this.View.BtnAds != null) this.View.BtnAds.onClick.AddListener(this.OnClickAds);
        }

        public override async UniTask BindData(UnityTemplateWinScreenModel model)
        {
            this.View.BtnAds.BindData(this.AdPlacement);
            this.ItemUnlockProgress(model.ItemUnlockLastValue, model.ItemUnlockNewValue);
            this.soundService.PlaySoundWin();

            if (this.View.UseLightGlow) this.tweenSpin = this.View.ImgLightGlow.transform.DORotate(new(0, 0, -360), 5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            if (this.View.UseStarRate) await this.View.StarRateView.SetStarRate(model.StarRate);
        }

        protected async void ItemUnlockProgress(float lastValue, float newValue)
        {
            var sequence = DOTween.Sequence();

            if (this.View.UseItemUnlockProgressText)
            {
                this.View.TxtItemUnlockProgress.text = $"{lastValue * 100:N0}%";
                sequence.Join(
                    DOTween.To(
                        () => lastValue,
                        value =>
                        {
                            this.View.TxtItemUnlockProgress.text = $"{value * 100:N0}%";
                        },
                        newValue,
                        .5f
                    ).SetEase(Ease.Linear)
                );
            }

            if (this.View.UseItemUnlockProgressImage)
            {
                var itemData = this.inventoryDataController.GetItemData(this.Model.ItemId);
                var sprite   = await this.gameAssets.LoadAssetAsync<Sprite>(itemData.ItemBlueprintRecord.ImageAddress);
                this.View.ImgItemUnlockProgress.sprite           = sprite;
                this.View.ImgItemUnlockProgressBackground.sprite = sprite;
                sequence.Join(
                    DOTween.To(
                        () => this.View.ImgItemUnlockProgress.fillAmount    = lastValue,
                        value => this.View.ImgItemUnlockProgress.fillAmount = value,
                        newValue,
                        .5f
                    ).SetEase(Ease.Linear)
                );
            }

            if (this.View.UseItemUnlockProgressSlider)
                sequence.Join(
                    DOTween.To(
                        () => this.View.SliderItemUnlockProgress.value    = lastValue,
                        value => this.View.SliderItemUnlockProgress.value = value,
                        newValue,
                        .5f
                    ).SetEase(Ease.Linear)
                );

            if (newValue < 1f)
                this.inventoryDataController.UpdateStatusItemData(this.Model.ItemId, UnityTemplateItemData.Status.InProgress);
            else
                sequence.onComplete += this.OnItemUnlock;
        }

        protected virtual void OnClickHome()
        {
            this.screenManager.OpenScreen<UnityTemplateHomeSimpleScreenPresenter>();
        }

        protected virtual void OnClickReplay()
        {
            this.screenManager.OpenScreen<UnityTemplateHomeSimpleScreenPresenter>();
        }

        protected virtual void OnClickNext()
        {
            this.screenManager.OpenScreen<UnityTemplateHomeSimpleScreenPresenter>();
        }

        protected virtual void OnClickAds()
        {
            this.adService.ShowRewardedAd(this.AdPlacement,
                () =>
                {
                });
        }

        protected virtual void OnItemUnlock()
        {
            this.inventoryDataController.UpdateStatusItemData(this.Model.ItemId, UnityTemplateItemData.Status.Owned);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.View.BtnAds.Dispose();
            DOTween.Kill(this.tweenSpin);
        }
    }
}