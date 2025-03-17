#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Signals;

    public class UnityTemplateMainSceneInstaller : BaseSceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            this.DeclareSignals();
            this.Container.InitScreenManually<UnityTemplateHomeSimpleScreenPresenter>();
        }

        private void DeclareSignals()
        {
            this.Container.DeclareSignal<OnUpdateCurrencySignal>();
        }
    }
}
#endif