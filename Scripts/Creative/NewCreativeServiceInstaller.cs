#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Creative
{
    using HyperGames.UnityTemplate.UnityTemplate.Creative.CheatLevel;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Services;
    using Zenject;

    public class NewCreativeServiceInstaller : Installer<NewCreativeServiceInstaller>
    {
        public override void InstallBindings()
        {
#if !CREATIVE
            return;
#endif
            this.Container.BindInterfacesAndSelfTo<NewCreativeService>().AsCached().NonLazy();

            this.Container.Resolve<CreativeService>().DisableTripleTap();
        }
    }
}
#endif