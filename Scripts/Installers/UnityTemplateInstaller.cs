#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Installers
{
    using ServiceImplementation.IAPServices;
    using HyperGames.UnityTemplate.UnityTemplate.Creative.Cheat;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.BadgeNotify;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.CollectionNew;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Item;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Pack;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services.BreakAds;
    using HyperGames.UnityTemplate.UnityTemplate.Services.DeepLinking;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Permissions;
    using HyperGames.UnityTemplate.UnityTemplate.Services.StoreRating;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Toast;
    using UnityEngine;
    using Zenject;

    public class UnityTemplateInstaller : Installer<ToastController, UnityTemplateInstaller>
    {
        private readonly ToastController toastCanvas;

        public UnityTemplateInstaller(ToastController toastCanvas) { this.toastCanvas = toastCanvas; }

        public override void InstallBindings()
        {
            Application.targetFrameRate = 60;
            //Helper
            this.Container.Bind<UnityTemplateAnimationHelper>().AsCached();
            this.Container.Bind<UnityTemplateCollectionItemViewHelper>().AsCached();
            this.Container.Bind<BreakAdsViewHelper>().AsCached();

            UnityTemplateDailyRewardInstaller.Install(this.Container);
            UnityTemplateDeclareSignalInstaller.Install(this.Container);
            UnityTemplateServicesInstaller.Install(this.Container, this.toastCanvas);
            IapInstaller.Install(this.Container);
            UnityTemplateLocalDataInstaller.Install(this.Container); // bind after FBInstantInstaller for remote user data
            UnityTemplateThirdPartyInstaller.Install(this.Container); // bind after UnityTemplateLocalDataInstaller for local data analytics
            UnityTemplateAdsInstaller.Install(this.Container); // this depend on third party service signals
            NotificationInstaller.Install(this.Container);
            StoreRatingServiceInstaller.Install(this.Container);
            PermissionServiceInstaller.Install(this.Container);
            DeepLinkInstaller.Install(this.Container);
            this.Container.BindInterfacesAndSelfTo<UnityTemplateIapServices>().AsCached().NonLazy();

            HyperGamesCheatInstaller.Install(this.Container);

            // not lock in editor because interstitial fake ads can not close
#if !UNITY_EDITOR
            this.Container.BindInterfacesAndSelfTo<LockInputService>().AsCached().NonLazy();
#endif

            //Feature
#if HYPERGAMES_DAILY_REWARD
            this.Container.BindInterfacesAndSelfTo<UnityTemplateDailyRewardService>().AsCached().NonLazy();
#endif

#if HYPERGAMES_NO_INTERNET
            this.Container.BindInterfacesAndSelfTo<UnityTemplateNoInternetService>().AsCached().NonLazy();
#endif

#if HYPERGAMES_RATE_US
            this.Container.BindInterfacesAndSelfTo<UnityTemplateRateUsService>().AsCached().NonLazy();
#endif

#if HYPERGAMES_BADGE_NOTIFY
            this.Container.BindInterfacesAndSelfTo<UnityTemplateBadgeNotifySystem>().AsCached().NonLazy();
#endif

#if HYPERGAMES_DEBUG && !PRODUCTION
            this.Container.Bind<Reporter>().FromComponentInNewPrefabResource("LogsViewer").AsCached().NonLazy();
#endif
        }
    }
}
#endif