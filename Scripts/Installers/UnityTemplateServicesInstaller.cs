#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Installers
{
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Helpers;
    using HyperGames.UnityTemplate.UnityTemplate.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.FeaturesConfig;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle;
    using HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle.AllRewards;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Toast;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Vibration;
    using HyperGames.UnityTemplate.UnityTemplate.Utils;
    using Zenject;

    public class UnityTemplateServicesInstaller : Installer<ToastController, UnityTemplateServicesInstaller>
    {
        private readonly ToastController toastController;

        public UnityTemplateServicesInstaller(ToastController toastController)
        {
            this.toastController = toastController;
        }

        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<UnityTemplateFeatureConfig>().AsCached().NonLazy();
            // Master Audio
            this.Container.Bind<UnityTemplateSoundServices>().AsCached();
            //Build-in service
            this.Container.BindInterfacesAndSelfTo<InternetService>().AsSingle().NonLazy();
            //HandleScreenShow
            this.Container.BindInterfacesAndSelfTo<UnityTemplateScreenShowServices>().AsCached();
            this.Container.BindInterfacesAndSelfToAllTypeDriveFrom<UnityTemplateBaseScreenShow>();
            //FlyingAnimation Currency
            this.Container.Bind<UnityTemplateFlyingAnimationController>().AsCached().NonLazy();
            //Utils
            this.Container.Bind<GameAssetUtil>().AsCached();
            //Vibration
            this.Container.Bind<IVibrationService>().To<UnityTemplateVibrationService>().AsCached();

            this.Container.Bind<UnityTemplateHandleSoundWhenOpenAdsServices>().AsCached().NonLazy();
            //Reward Handle
            this.Container.BindInterfacesAndSelfToAllTypeDriveFrom<IUnityTemplateRewardExecutor>();
            this.Container.BindInterfacesAndSelfTo<UnityTemplateRewardHandler>().AsCached().NonLazy();

            // this.Container.BindInterfacesTo<UnityTemplateAutoOpenStartedPackServices>().AsCached().NonLazy();

            // VFX Spawn
            this.Container.Bind<UnityTemplateVFXSpawnService>().AsCached().NonLazy();

            // Toast
            this.Container.Bind<ToastController>().FromComponentInNewPrefab(this.toastController).AsCached().NonLazy();

            //Button experience helper
            this.Container.BindInterfacesAndSelfTo<UnityTemplateButtonExperienceHelper>().AsSingle().NonLazy();
        }
    }
}
#endif