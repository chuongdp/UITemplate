#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Services.Permissions
{
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Services.Permissions.Signals;
    using Zenject;

    public class PermissionServiceInstaller : Installer<PermissionServiceInstaller>
    {
        public override void InstallBindings()
        {
#if UNITY_ANDROID
            this.Container.Bind<IPermissionService>().To<AndroidPermissionService>().AsSingle();
#elif UNITY_IOS
            this.Container.Bind<IPermissionService>().To<IOSPermissionService>().AsSingle();
#else
            this.Container.Bind<IPermissionService>().To<DummyPermissionService>().AsSingle();
#endif
            this.Container.DeclareSignal<OnRequestPermissionStartSignal>();
            this.Container.DeclareSignal<OnRequestPermissionCompleteSignal>();
        }
    }
}
#endif