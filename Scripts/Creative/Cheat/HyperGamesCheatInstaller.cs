#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Creative.Cheat
{
#if HYPERGAMES_CHEAT
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Services;
#endif
    using Zenject;

    public class HyperGamesCheatInstaller : Installer<HyperGamesCheatInstaller>
    {
        public override void InstallBindings()
        {
#if HYPERGAMES_CHEAT
#if CREATIVE
            this.Container.Resolve<CreativeService>().DisableTripleTap();
#endif
            this.Container.BindInterfacesTo<HyperGamesCheatGenerate>().AsCached().NonLazy();
#endif
        }
    }
}
#endif