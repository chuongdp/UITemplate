#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Installers
{
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services.AnalyticHandler;
    using Zenject;
#if APPLOVIN
#endif
#if ADMOB || IRONSOURCE
#endif

    public class UnityTemplateAdsInstaller : Installer<UnityTemplateAdsInstaller>
    {
        private const string MinPauseSecondsToShowAoaRemoteConfigKey = "min_pause_seconds_to_show_aoa";

        public override void InstallBindings()
        {
#if !HYPERGAMES_PLAYABLE_ADS
            this.Container.BindInterfacesAndSelfTo<UnityTemplateAnalyticHandler>().AsCached();
#endif
#if CREATIVE
            this.Container.Bind<UnityTemplateAdServiceWrapper>().To<UnityTemplateAdServiceWrapperCreative>().AsCached();
#else
            this.Container.BindInterfacesAndSelfTo<UnityTemplateAdServiceWrapper>().AsCached();
#endif

#if CREATIVE
            this.Container.BindInterfacesAndSelfTo<CreativeService>().AsCached().NonLazy();
#endif
        }
    }
}
#endif