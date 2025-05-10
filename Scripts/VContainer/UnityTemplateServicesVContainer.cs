#if GDK_VCONTAINER
#nullable enable
namespace GameTemplate
{
    using GameTemplate.UnityTemplate.Services;
    using GameTemplate.UnityTemplate.Services.Vibration;
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