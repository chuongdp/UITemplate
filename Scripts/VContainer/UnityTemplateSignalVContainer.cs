#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate
{
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Creative.CheatLevel;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.Signal;
    using HyperGames.UnityTemplate.UnityTemplate.Others.StateMachine.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Signals;
    using ServiceImplementation.FireBaseRemoteConfig;
    using VContainer;

    public static class UnityTemplateSignalVContainer
    {
        public static void DeclareUnityTemplateSignals(this IContainerBuilder builder)
        {
            //FTUE
            builder.DeclareSignal<TutorialCompletionSignal>();
            builder.DeclareSignal<FTUEButtonClickSignal>();
            builder.DeclareSignal<FTUEDoActionSignal>();
            builder.DeclareSignal<FTUETriggerSignal>();

            //Signal
            builder.DeclareSignal<OnNotEnoughCurrencySignal>();
            builder.DeclareSignal<OnUpdateCurrencySignal>();
            builder.DeclareSignal<OnFinishCurrencyAnimationSignal>();
            builder.DeclareSignal<LevelStartedSignal>();
            builder.DeclareSignal<LevelEndedSignal>();
            builder.DeclareSignal<LevelSkippedSignal>();
            builder.DeclareSignal<RemoteConfigFetchedSucceededSignal>();
            builder.DeclareSignal<OnRemoveAdsSucceedSignal>();
            builder.DeclareSignal<UnityTemplateOnUpdateBannerStateSignal>();

            //State machine
            builder.DeclareSignal<OnStateEnterSignal>();
            builder.DeclareSignal<OnStateExitSignal>();

            //Creative, Cheat
            builder.DeclareSignal<ChangeLevelCreativeSignal>();
        }
    }
}
#endif