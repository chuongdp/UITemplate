#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate
{
    using System.Linq;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.CollectionNew;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services.BreakAds;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Toast;
    using HyperGames.DeepLinking;
    using HyperGames.Notification;
    using HyperGames.Permission;
    using HyperGames.StoreRating;
    using HyperGames.UnityTemplate.UnityTemplate.Creative.Cheat;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.BadgeNotify;
    using UnityEngine;
    using VContainer;

    public static class UnityTemplateVContainer
    {
        public static void RegisterUnityTemplate(this IContainerBuilder builder, Transform rootTransform, ToastController toastControllerPrefab)
        {
            Application.targetFrameRate = 60;

            builder.Register<UnityTemplateAnimationHelper>(Lifetime.Singleton);
            builder.Register<UnityTemplateCollectionItemViewHelper>(Lifetime.Singleton);
            builder.Register(typeof(BreakAdsViewHelper).GetDerivedTypes().OrderBy(type => type == typeof(BreakAdsViewHelper)).First(), Lifetime.Singleton).As<BreakAdsViewHelper>();

            builder.RegisterUnityTemplateAdsService();
            builder.RegisterUnityTemplateThirdPartyServices();

            builder.RegisterUnityTemplateLocalData();
            builder.RegisterUnityTemplateServices(rootTransform, toastControllerPrefab);
            builder.RegisterDailyReward();
            builder.RegisterNotificationService();
            builder.RegisterStoreRatingService();
            builder.RegisterPermissionService();
            builder.RegisterDeepLinkService();

            builder.DeclareUnityTemplateSignals();

            builder.Register<UnityTemplateIapServices>(Lifetime.Singleton).AsInterfacesAndSelf();

            builder.RegisterCheatView();

            // not lock in editor because interstitial fake ads can not close
            #if !UNITY_EDITOR && !DISABLE_LOCK_INPUT
            builder.Register<LockInputService>(Lifetime.Singleton).AsInterfacesAndSelf();
            #endif

            //Feature
            #if HYPERGAMES_DAILY_REWARD
            builder.Register<UnityTemplateDailyRewardService>(Lifetime.Singleton).AsInterfacesAndSelf();
            #endif

            #if HYPERGAMES_NO_INTERNET
            builder.Register<UnityTemplateNoInternetService>(Lifetime.Singleton).AsInterfacesAndSelf();
            #endif

            #if HYPERGAMES_RATE_US
            builder.Register<UnityTemplateRateUsService>(Lifetime.Singleton).AsInterfacesAndSelf();
            #endif

            #if HYPERGAMES_BADGE_NOTIFY
            builder.Register<UnityTemplateBadgeNotifySystem>(Lifetime.Singleton).AsInterfacesAndSelf();
            #endif

            #if HYPERGAMES_DEBUG && !PRODUCTION
            builder.RegisterComponentInNewPrefabResource<Reporter>("LogsViewer", Lifetime.Singleton).UnderTransform(rootTransform);
            builder.AutoResolve<Reporter>();
            #endif
        }
    }
}
#endif