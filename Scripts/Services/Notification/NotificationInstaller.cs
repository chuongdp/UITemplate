#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Services
{
    using System;
    using Zenject;

    public class NotificationInstaller : Installer<NotificationInstaller>
    {
        public override void InstallBindings()
        {
#if HYPERGAMES_NOTIFICATION && UNITY_ANDROID
            this.Container.Bind(typeof(INotificationService), typeof(IDisposable)).To<AndroidUnityNotificationService>().AsCached().NonLazy();
#elif HYPERGAMES_NOTIFICATION && UNITY_IOS
            this.Container.Bind(typeof(INotificationService), typeof(IDisposable)).To<IOSUnityNotificationService>().AsCached().NonLazy();
#else
            this.Container.Bind<INotificationService>().To<DummyNotificationService>().AsCached().NonLazy();
#endif
            this.Container.Bind<NotificationMappingHelper>().AsCached().NonLazy();
        }
    }
}
#endif