namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.RemoveAdsBottomBar
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Configs.GameEvents;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using TMPro;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateRemoveAdPopupView : BaseView
    {
        public TMP_Text priceText;
        public Button   btnRemoveAds;
        public Button   btnClose;
    }

    [PopupInfo(nameof(UnityTemplateRemoveAdPopupView))]
    public class UnityTemplateRemoveAdPopupPresenter : UnityTemplateBasePopupPresenter<UnityTemplateRemoveAdPopupView>
    {
        protected readonly UnityTemplateIapServices      UnityTemplateIapServices;
        protected readonly GameFeaturesSetting        gameFeaturesSetting;
        protected readonly UnityTemplateAdServiceWrapper adServiceWrapper;

        [Preserve]
        public UnityTemplateRemoveAdPopupPresenter(
            SignalBus                  signalBus,
            ILogService                logger,
            UnityTemplateIapServices      UnityTemplateIapServices,
            GameFeaturesSetting        gameFeaturesSetting,
            UnityTemplateAdServiceWrapper adServiceWrapper
        ) : base(signalBus, logger)
        {
            this.UnityTemplateIapServices = UnityTemplateIapServices;
            this.gameFeaturesSetting   = gameFeaturesSetting;
            this.adServiceWrapper      = adServiceWrapper;
        }

        private string ProductId => this.gameFeaturesSetting.IAPConfig.removeAdsProductId;

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.btnRemoveAds.onClick.AddListener(this.OnRemoveAdsClicked);
            this.View.btnClose.onClick.AddListener(this.OnClickCloseButton);
        }

        private void OnRemoveAdsClicked()
        {
            this.UnityTemplateIapServices.BuyProduct(
                this.View.btnRemoveAds.gameObject,
                this.ProductId,
                _ =>
                {
                    this.adServiceWrapper.RemoveAds();
                    this.CloseView();
                }
            );
        }

        protected virtual void OnClickCloseButton()
        {
            this.CloseView();
        }

        public override UniTask BindData()
        {
            this.View.priceText.text = this.UnityTemplateIapServices.GetPriceById(this.ProductId, "0.99$");

            return UniTask.CompletedTask;
        }
    }
}