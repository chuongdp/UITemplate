#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Installers
{
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Creative.CheatLevel;
    using ServiceImplementation.FireBaseRemoteConfig;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.Signal;
    using HyperGames.UnityTemplate.UnityTemplate.Others.StateMachine.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Signals;
    using Zenject;

    /// <summary>
    /// Installer for declaring signals.
    /// We use this installer to declare signals in the container.
    /// We don't you reflection to declare signals.
    /// </summary>
    public class UnityTemplateDeclareSignalInstaller : Installer<UnityTemplateDeclareSignalInstaller>
    {
        public override void InstallBindings()
        {
            //FTUE
            this.Container.DeclareSignal<TutorialCompletionSignal>();
            this.Container.DeclareSignal<FTUEButtonClickSignal>();
            this.Container.DeclareSignal<FTUEDoActionSignal>();
            this.Container.DeclareSignal<FTUETriggerSignal>();

            //Signal
            this.Container.DeclareSignal<OnNotEnoughCurrencySignal>();
            this.Container.DeclareSignal<OnUpdateCurrencySignal>();
            this.Container.DeclareSignal<OnFinishCurrencyAnimationSignal>();
            this.Container.DeclareSignal<LevelStartedSignal>();
            this.Container.DeclareSignal<LevelEndedSignal>();
            this.Container.DeclareSignal<LevelSkippedSignal>();
            this.Container.DeclareSignal<RemoteConfigFetchedSucceededSignal>();
            this.Container.DeclareSignal<OnRemoveAdsSucceedSignal>();
            this.Container.DeclareSignal<UnityTemplateOnUpdateBannerStateSignal>();

            //State machine
            this.Container.DeclareSignal<OnStateEnterSignal>();
            this.Container.DeclareSignal<OnStateExitSignal>();

            //Creative, Cheat
            this.Container.DeclareSignal<ChangeLevelCreativeSignal>();
        }
    }
}
#endif