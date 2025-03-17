#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.Permission
{
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Permissions;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Permissions.Signals;
    using VContainer;

    public static class PermissionServiceVContainer
    {
        public static void RegisterPermissionService(this IContainerBuilder builder)
        {
            #if UNITY_ANDROID
            builder.Register<AndroidPermissionService>(Lifetime.Singleton).AsImplementedInterfaces();
            #elif UNITY_IOS
            builder.Register<IOSPermissionService>(Lifetime.Singleton).AsImplementedInterfaces();
            #else
            builder.Register<DummyPermissionService>(Lifetime.Singleton).AsImplementedInterfaces();
            #endif

            builder.DeclareSignal<OnRequestPermissionStartSignal>();
            builder.DeclareSignal<OnRequestPermissionCompleteSignal>();
        }
    }
}
#endif