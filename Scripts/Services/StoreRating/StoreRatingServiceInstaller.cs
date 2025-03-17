#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Services.StoreRating
{
    using Zenject;

    public class StoreRatingServiceInstaller : Installer<StoreRatingServiceInstaller>
    {
        public override void InstallBindings()
        {
#if !UNITY_EDITOR && UNITY_ANDROID && HYPERGAMES_STORE_RATING
            this.Container.Bind<IStoreRatingService>().To<AndroidStoreRatingService>().AsSingle().NonLazy();
#elif !UNITY_EDITOR && UNITY_IOS && HYPERGAMES_STORE_RATING
            this.Container.Bind<IStoreRatingService>().To<IosStoreRatingService>().AsSingle().NonLazy();
#else
            this.Container.Bind<IStoreRatingService>().To<DummyStoreRatingService>().AsSingle().NonLazy();
#endif
            this.Container.Bind<UnityTemplateStoreRatingHandler>().AsSingle().NonLazy();
        }
    }
}
#endif