#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate
{
    using System.Linq;
    using Core.AnalyticServices;
    using Core.AnalyticServices.Data;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Configs.GameEvents;
    using HyperGames.UnityTemplate.UnityTemplate.ThirdPartyServices.AnalyticEvents;
    using ServiceImplementation.AdsServices;
    using ServiceImplementation.Configs;
    using ServiceImplementation.FirebaseAnalyticTracker;
    using ServiceImplementation.FireBaseRemoteConfig;
    using ServiceImplementation.IAPServices;
    using ServiceImplementation.RemoteConfig;
    using VContainer;
    #if APPSFLYER
    using ServiceImplementation.AppsflyerAnalyticTracker;
    #endif

    #if ADJUST
    using ServiceImplementation.AdjustAnalyticTracker;
    #endif

    public static class UnityTemplateThirdPartyVContainer
    {
        public static void RegisterUnityTemplateThirdPartyServices(this IContainerBuilder builder)
        {
            builder.RegisterAdService();
            builder.RegisterIAPService();
            builder.RegisterAnalyticService();
            builder.RegisterRemoteConfig();

            builder.RegisterResource<ThirdPartiesConfig>(ThirdPartiesConfig.ResourcePath, Lifetime.Singleton);
            builder.RegisterResource<RemoteConfigSetting>(RemoteConfigSetting.ResourcePath, Lifetime.Singleton);
            builder.RegisterResource<GameFeaturesSetting>(GameFeaturesSetting.ResourcePath, Lifetime.Singleton);

            builder.Register(typeof(IAnalyticEventFactory).GetSingleDerivedType(), Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<FirebaseAnalyticTracker>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(container => container.Resolve<IAnalyticEventFactory>().FireBaseAnalyticsEventCustomizationConfig);
            #if APPSFLYER
            builder.Register<AppsflyerTracker>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(container => container.Resolve<IAnalyticEventFactory>().AppsFlyerAnalyticsEventCustomizationConfig);
            #endif
            #if ADJUST
            builder.Register<AdjustTracker>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(container => container.ResolveOrDefault<AnalyticsEventCustomizationConfig>(new()));
            #endif
        }
    }
}
#endif