#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Loading
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;

    public class UnityTemplateLoadingSceneInstaller : BaseSceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            this.Container.InitScreenManually<UnityTemplateLoadingScreenPresenter>();
        }
    }
}
#endif