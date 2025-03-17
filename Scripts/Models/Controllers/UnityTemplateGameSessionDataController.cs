namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using System;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.UserData;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using UnityEngine.Scripting;

    public class UnityTemplateGameSessionDataController : IUnityTemplateControllerData, IInitializable
    {
        private readonly UnityTemplateGameSessionData gameSessionData;
        private readonly SignalBus                 signalBus;

        [Preserve]
        public UnityTemplateGameSessionDataController(UnityTemplateGameSessionData gameSessionData, SignalBus signalBus)
        {
            this.gameSessionData = gameSessionData;
            this.signalBus       = signalBus;
        }

        public DateTime FirstInstallDate => this.gameSessionData.FirstInstallDate;
        public int      OpenTime         => this.gameSessionData.OpenTime;

        public void Initialize()
        {
            this.signalBus.Subscribe<UserDataLoadedSignal>(this.OnUserDataLoadedHandler);
        }

        private void OnUserDataLoadedHandler()
        {
            this.gameSessionData.OpenTime++;
        }
    }
}