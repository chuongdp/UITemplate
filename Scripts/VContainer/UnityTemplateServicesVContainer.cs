#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate
{
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.Scripts.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Helpers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.FeaturesConfig;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle;
    using HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle.AllRewards;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Toast;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Vibration;
    using HyperGames.UnityTemplate.UnityTemplate.Utils;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public static class UnityTemplateServicesVContainer
    {
        public static void RegisterUnityTemplateServices(this IContainerBuilder builder, Transform rootTransform, ToastController toastController)
        {
            builder.Register<UnityTemplateFeatureConfig>(Lifetime.Singleton).AsInterfacesAndSelf();

            // Master Audio
            builder.Register<UnityTemplateSoundServices>(Lifetime.Singleton);

            //Build-in service
            builder.Register<InternetService>(Lifetime.Singleton).AsImplementedInterfaces();

            //HandleScreenShow
            builder.Register<UnityTemplateScreenShowServices>(Lifetime.Singleton).AsImplementedInterfaces();
            typeof(IUnityTemplateScreenShow).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces());

            //FlyingAnimation Currency
            builder.Register<UnityTemplateFlyingAnimationController>(Lifetime.Singleton);

            //Utils
            builder.Register<GameAssetUtil>(Lifetime.Singleton);

            //Vibration
            builder.Register<UnityTemplateVibrationService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<UnityTemplateHandleSoundWhenOpenAdsServices>(Lifetime.Singleton);

            //Reward Handle
            builder.Register<UnityTemplateRewardHandler>(Lifetime.Singleton);
            typeof(IUnityTemplateRewardExecutor).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces());

            // VFX Spawn
            builder.Register<UnityTemplateVFXSpawnService>(Lifetime.Singleton);

            // Toast
            builder.RegisterComponentInNewPrefab(toastController, Lifetime.Singleton).UnderTransform(rootTransform);

            //Button experience helper
            builder.Register<UnityTemplateButtonExperienceHelper>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
#endif