namespace HyperGames.UnityTemplate.UnityTemplate.Services
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Signals;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Configs.GameEvents;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Popups;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateNoInternetService : IInitializable
    {
        private readonly SignalBus                           signalBus;
        private readonly GameFeaturesSetting                 gameFeaturesSetting;
        private readonly IScreenManager                      screenManager;
        private readonly UnityTemplateGameSessionDataController UnityTemplateGameSessionDataController;

        [Preserve]
        public UnityTemplateNoInternetService(
            SignalBus                           signalBus,
            GameFeaturesSetting                 gameFeaturesSetting,
            IScreenManager                      screenManager,
            UnityTemplateGameSessionDataController UnityTemplateGameSessionDataController
        )
        {
            this.signalBus                           = signalBus;
            this.gameFeaturesSetting                 = gameFeaturesSetting;
            this.screenManager                       = screenManager;
            this.UnityTemplateGameSessionDataController = UnityTemplateGameSessionDataController;
        }

        private bool IsAbleToCheck  => this.IsSessionValid && this.IsTimeValid && this.isScreenValid;
        private bool IsSessionValid => this.UnityTemplateGameSessionDataController.OpenTime >= this.gameFeaturesSetting.NoInternetConfig.SessionToShow;
        private bool IsTimeValid    => this.gameFeaturesSetting.NoInternetConfig.DelayToCheck < Time.realtimeSinceStartup;
        private bool isScreenValid;

        private int   continuousNoInternetChecked = 0;
        private float CheckInterval => this.gameFeaturesSetting.NoInternetConfig.CheckInterval;

        public void Initialize()
        {
            this.signalBus.Subscribe<ScreenShowSignal>(this.OnScreenShow);
            this.CheckInternetInterval().Forget();
        }

        #region Check Internet

        private async UniTaskVoid CheckInternetInterval()
        {
            if (this.IsAbleToCheck)
            {
                if (this.CheckInternet())
                    this.continuousNoInternetChecked = 0;
                else
                    this.continuousNoInternetChecked++;

                if (this.continuousNoInternetChecked >= this.gameFeaturesSetting.NoInternetConfig.ContinuesFailToShow)
                {
                    this.continuousNoInternetChecked = 0;
                    this.screenManager.OpenScreen<UnityTemplateConnectErrorPresenter>().Forget();
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(this.CheckInterval), true);
            this.CheckInternetInterval().Forget();
        }

        private bool CheckInternet()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        #endregion

        private void OnScreenShow(ScreenShowSignal obj)
        {
            if (this.gameFeaturesSetting.NoInternetConfig.isCustomScreenTrigger)
                this.isScreenValid = this.gameFeaturesSetting.NoInternetConfig.screenTriggerIds.Contains(obj.ScreenPresenter.GetType().Name);
            else
                this.isScreenValid = obj.ScreenPresenter.GetType().Name != "UnityTemplateConnectErrorPresenter";
        }
    }
}