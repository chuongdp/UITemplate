namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Play.End
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateLoseScreenView : BaseView
    {
        public Button                 HomeButton;
        public Button                 ReplayButton;
        public UnityTemplateAdsButton    SkipButton;
        public UnityTemplateCurrencyView CurrencyView;
    }

    [ScreenInfo(nameof(UnityTemplateLoseScreenView))]
    public class UnityTemplateLoseScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateLoseScreenView>
    {
        #region Inject

        protected readonly UnityTemplateAdServiceWrapper        adService;
        private readonly   UnityTemplateSoundServices           soundServices;
        protected readonly IScreenManager                    screenManager;
        protected readonly UnityTemplateInventoryDataController inventoryDataController;

        [Preserve]
        public UnityTemplateLoseScreenPresenter(
            SignalBus                         signalBus,
            ILogService                       logger,
            UnityTemplateAdServiceWrapper        adService,
            UnityTemplateSoundServices           soundServices,
            IScreenManager                    screenManager,
            UnityTemplateInventoryDataController inventoryDataController
        ) : base(signalBus, logger)
        {
            this.adService               = adService;
            this.soundServices           = soundServices;
            this.screenManager           = screenManager;
            this.inventoryDataController = inventoryDataController;
        }

        #endregion

        protected virtual string AdPlacement => "replay";

        protected override void OnViewReady()
        {
            base.OnViewReady();

            if (this.View.SkipButton != null) this.View.SkipButton.OnViewReady(this.adService);

            if (this.View.HomeButton != null) this.View.HomeButton.onClick.AddListener(this.OnClickHome);

            if (this.View.ReplayButton != null) this.View.ReplayButton.onClick.AddListener(this.OnClickReplay);

            if (this.View.SkipButton != null) this.View.SkipButton.onClick.AddListener(this.OnClickSkip);
        }

        public override UniTask BindData()
        {
            if (this.View.SkipButton != null) this.View.SkipButton.BindData(this.AdPlacement);

            this.soundServices.PlaySoundLose();
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.View.SkipButton != null) this.View.SkipButton.Dispose();
        }

        protected virtual void OnClickHome()
        {
            this.screenManager.OpenScreen<UnityTemplateHomeSimpleScreenPresenter>();
        }

        protected virtual void OnClickReplay()
        {
        }

        protected virtual void OnClickSkip()
        {
        }
    }
}