#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate
{
    using VContainer;
    using GameFoundation.DI;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;

    public static class UnityTemplateAdsVContainer
    {
        public static void RegisterUnityTemplateAdsService(this IContainerBuilder builder)
        {
            builder.Register<UnityTemplateAnalyticHandler>(Lifetime.Singleton).AsInterfacesAndSelf();

            #if CREATIVE
            builder.Register<UnityTemplateAdServiceWrapper, UnityTemplateAdServiceWrapperCreative>(Lifetime.Singleton);
            #else
            builder.Register<UnityTemplateAdServiceWrapper>(Lifetime.Singleton).AsInterfacesAndSelf();
            #endif

            #if CREATIVE
            builder.Register<CreativeService>(Lifetime.Singleton).AsInterfacesAndSelf();
            #endif
        }
    }
}
#endif