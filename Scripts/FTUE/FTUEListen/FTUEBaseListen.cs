namespace HyperGames.UnityTemplate.UnityTemplate.FTUE.FTUEListen
{
    using GameFoundation.DI;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.Signal;
    using HyperGames.UnityTemplate.Scripts.Blueprints;

    public abstract class FTUEBaseListen : IInitializable
    {
        protected readonly SignalBus               SignalBus;
        protected readonly UnityTemplateFTUEBlueprint FtueBlueprint;

        protected FTUEBaseListen(SignalBus signalBus, UnityTemplateFTUEBlueprint ftueBlueprint)
        {
            this.SignalBus     = signalBus;
            this.FtueBlueprint = ftueBlueprint;
        }

        public void Initialize()
        {
            this.InitInternal();
        }

        protected abstract void InitInternal();

        protected void FireFtueTriggerSignal(string ftueId)
        {
            this.SignalBus.Fire(new FTUETriggerSignal(ftueId));
        }
    }
}