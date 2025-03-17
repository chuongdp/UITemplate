namespace HyperGames.UnityTemplate.UnityTemplate.Services
{
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Signals;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Configs.GameEvents;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Popups;
    using HyperGames.UnityTemplate.UnityTemplate.Services.StoreRating;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateRateUsService : IInitializable
    {
        private readonly SignalBus                           signalBus;
        private readonly GameFeaturesSetting                 gameFeaturesSetting;
        private readonly UnityTemplateGameSessionDataController UnityTemplateGameSessionDataController;
        private readonly IScreenManager                      screenManager;
        private readonly UnityTemplateStoreRatingHandler        storeRatingHandler;

        [Preserve]
        public UnityTemplateRateUsService(
            SignalBus                           signalBus,
            GameFeaturesSetting                 gameFeaturesSetting,
            UnityTemplateGameSessionDataController UnityTemplateGameSessionDataController,
            IScreenManager                      screenManager,
            UnityTemplateStoreRatingHandler        storeRatingHandler
        )
        {
            this.signalBus                           = signalBus;
            this.gameFeaturesSetting                 = gameFeaturesSetting;
            this.UnityTemplateGameSessionDataController = UnityTemplateGameSessionDataController;
            this.screenManager                       = screenManager;
            this.storeRatingHandler                  = storeRatingHandler;
        }

        public void Initialize()
        {
            this.signalBus.Subscribe<ScreenShowSignal>(this.OnScreenShow);
            this.isShownInCurrentSession = false;
        }

        private async void OnScreenShow(ScreenShowSignal obj)
        {
            if (!this.IsScreenCanShowRateUs(obj.ScreenPresenter)) return;
            await this.screenManager.OpenScreen<UnityTemplateRateGamePopupPresenter>();
            this.isShownInCurrentSession = true;
        }

        private bool IsScreenCanShowRateUs(IScreenPresenter screenPresenter)
        {
            if (this.isShownInCurrentSession) return false;
            if (Time.realtimeSinceStartup < this.gameFeaturesSetting.RateUsConfig.DelayInSecondsTillShow) return false;
            if (!this.gameFeaturesSetting.RateUsConfig.isUsingCommonLogic) return false;
            if (this.storeRatingHandler.IsRated) return false;

            if (this.UnityTemplateGameSessionDataController.OpenTime < this.gameFeaturesSetting.RateUsConfig.SessionToShow) return false;
            if (this.gameFeaturesSetting.RateUsConfig.isCustomScreenTrigger) return this.gameFeaturesSetting.RateUsConfig.screenTriggerIds.Contains(screenPresenter.GetType().Name);

            return true; // Show everywhere
        }

        private bool isShownInCurrentSession = false;
    }
}