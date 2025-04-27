#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate
{
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Vibration;
    using UnityEngine;
    using VContainer;

    public static class UnityTemplateServicesVContainer
    {
        public static void RegisterUnityTemplateServices(this IContainerBuilder builder, Transform rootTransform)
        {
            //Vibration
            builder.Register<UnityTemplateVibrationService>(Lifetime.Singleton).AsImplementedInterfaces();

            // VFX Spawn
            builder.Register<UnityTemplateVFXSpawnService>(Lifetime.Singleton);
        }
    }
}
#endif