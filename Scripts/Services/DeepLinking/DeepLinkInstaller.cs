#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Services.DeepLinking
{
    using GameFoundation.Signals;
    using Zenject;

    public class DeepLinkInstaller : Installer<DeepLinkInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesTo<DeepLinkService>().AsSingle();
            this.Container.DeclareSignal<OnDeepLinkActiveSignal>();
        }
    }
}
#endif