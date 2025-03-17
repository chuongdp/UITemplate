namespace HyperGames.UnityTemplate.UnityTemplate.FTUE.FTUEListen
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Signals;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using UnityEngine.Scripting;

    public class FTUEListenScreenShow : FTUEBaseListen
    {
        [Preserve]
        public FTUEListenScreenShow(SignalBus signalBus, UnityTemplateFTUEBlueprint ftueBlueprint) : base(signalBus, ftueBlueprint)
        {
        }

        protected override void InitInternal()
        {
            this.SignalBus.Subscribe<ScreenShowSignal>(this.OnScreenShow);
        }

        private void OnScreenShow(ScreenShowSignal obj)
        {
            var currentScreen = obj.ScreenPresenter;

            if (currentScreen == null) return;

            foreach (var ftue in this.FtueBlueprint.Values)
            {
                if (!ftue.EnableTrigger) continue;

                if (currentScreen.GetType().Name.Equals(ftue.ScreenLocation)) this.FireFtueTriggerSignal(ftue.Id);
            }
        }
    }
}