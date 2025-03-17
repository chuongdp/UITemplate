namespace HyperGames.UnityTemplate.UnityTemplate.Services
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Signals;
    using GameFoundation.Scripts.UIModule.Utilities.GameQueueAction;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Configs.GameEvents;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.FeaturesConfig;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward;
    using UnityEngine.Scripting;

    public class UnityTemplateDailyRewardService : IInitializable
    {
        private const string FirstOpenAppKey = "FirstOpenApp";

        #region inject

        private readonly SignalBus                           signalBus;
        private readonly UnityTemplateDailyRewardController     UnityTemplateDailyRewardController;
        private readonly INotificationService                notificationServices;
        private readonly GameQueueActionContext              gameQueueActionContext;
        private readonly UnityTemplateFeatureConfig             UnityTemplateFeatureConfig;
        private readonly UnityTemplateGameSessionDataController sessionDataController;
        private readonly GameFeaturesSetting                 gameFeaturesSetting;

        #endregion

        private bool canShowReward = true;

        [Preserve]
        public UnityTemplateDailyRewardService(
            SignalBus                           signalBus,
            UnityTemplateDailyRewardController     UnityTemplateDailyRewardController,
            INotificationService                notificationServices,
            GameQueueActionContext              gameQueueActionContext,
            UnityTemplateFeatureConfig             UnityTemplateFeatureConfig,
            UnityTemplateGameSessionDataController sessionDataController,
            GameFeaturesSetting                 gameFeaturesSetting
        )
        {
            this.signalBus                       = signalBus;
            this.UnityTemplateDailyRewardController = UnityTemplateDailyRewardController;
            this.notificationServices            = notificationServices;
            this.gameQueueActionContext          = gameQueueActionContext;
            this.UnityTemplateFeatureConfig         = UnityTemplateFeatureConfig;
            this.sessionDataController           = sessionDataController;
            this.gameFeaturesSetting             = gameFeaturesSetting;
        }

        public void Initialize()
        {
            this.signalBus.Subscribe<ScreenShowSignal>(this.OnScreenShow);
        }

        private bool IsFirstOpenGame()
        {
            return this.sessionDataController.OpenTime == 1;
        }

        public UniTask ShowDailyRewardPopupAsync(bool force = false)
        {
            this.ShowDailyRewardPopup(force);
            return UniTask.CompletedTask;
        }

        private void ShowDailyRewardPopup(bool force)
        {
            if (!force)
            {
                if (!this.canShowReward) return;

                if (!this.gameFeaturesSetting.DailyRewardConfig.showOnFirstOpen && this.IsFirstOpenGame())
                {
                    this.canShowReward = false;
                    return;
                }

                if (!this.UnityTemplateDailyRewardController.CanClaimReward) return;
            }

            this.notificationServices.SetupCustomNotification(this.gameFeaturesSetting.DailyRewardConfig.notificationId);
            this.gameQueueActionContext.AddScreenToQueueAction<UnityTemplateDailyRewardPopupPresenter, UnityTemplateDailyRewardPopupModel>(new());
        }

        private async void OnScreenShow(ScreenShowSignal obj)
        {
            if (this.IsScreenCanShowDailyReward(obj.ScreenPresenter))
            {
                await this.UnityTemplateDailyRewardController.CheckRewardStatus();
                if (!this.UnityTemplateFeatureConfig.IsDailyRewardEnable) return;
                await this.ShowDailyRewardPopupAsync();
            }
        }

        private bool IsScreenCanShowDailyReward(IScreenPresenter screenPresenter)
        {
            if (this.gameFeaturesSetting.DailyRewardConfig.isCustomScreenTrigger) return this.gameFeaturesSetting.DailyRewardConfig.screenTriggerIds.Contains(screenPresenter.GetType().Name);

            return screenPresenter is UnityTemplateHomeSimpleScreenPresenter or UnityTemplateHomeTapToPlayScreenPresenter;
        }
    }
}