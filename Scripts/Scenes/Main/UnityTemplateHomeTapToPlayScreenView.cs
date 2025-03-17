namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.CollectionNew;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Play;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateHomeTapToPlayScreenView : BaseView
    {
        public Button                      TaptoplayButton;
        public Button                      ShopButton;
        public UnityTemplateCurrencyView      CoinText;
        public UnityTemplateSettingButtonView SettingButtonView;
    }

    [ScreenInfo(nameof(UnityTemplateHomeTapToPlayScreenView))]
    public class UnityTemplateHomeTapToPlayScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateHomeTapToPlayScreenView>
    {
        [Preserve]
        public UnityTemplateHomeTapToPlayScreenPresenter(
            SignalBus                         signalBus,
            ILogService                       logger,
            IScreenManager                    screenManager,
            UnityTemplateInventoryDataController UnityTemplateInventoryDataController,
            UnityTemplateSoundServices           soundServices
        ) : base(signalBus, logger)
        {
            this.ScreenManager                     = screenManager;
            this.UnityTemplateInventoryDataController = UnityTemplateInventoryDataController;
            this.SoundServices                     = soundServices;
        }

        protected override async void OnViewReady()
        {
            base.OnViewReady();
            await this.OpenViewAsync();
            this.View.TaptoplayButton.onClick.AddListener(this.OnClickTapToPlayButton);
            this.View.ShopButton.onClick.AddListener(this.OnClickShopButton);
        }

        public override UniTask BindData()
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnClickShopButton()
        {
            this.ScreenManager.OpenScreen<UnityTemplateNewCollectionScreenPresenter>();
        }

        protected virtual void OnClickTapToPlayButton()
        {
            this.ScreenManager.OpenScreen<UnityTemplateGameplayScreenPresenter>();
        }

        #region inject

        protected readonly IScreenManager                    ScreenManager;
        private readonly   UnityTemplateInventoryDataController UnityTemplateInventoryDataController;
        protected readonly UnityTemplateSoundServices           SoundServices;

        #endregion
    }
}