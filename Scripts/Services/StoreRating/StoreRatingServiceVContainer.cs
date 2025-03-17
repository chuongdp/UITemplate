#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.StoreRating
{
    using HyperGames.UnityTemplate.UnityTemplate.Services.StoreRating;
    using VContainer;

    public static class StoreRatingServiceVContainer
    {
        public static void RegisterStoreRatingService(this IContainerBuilder builder)
        {
            #if !UNITY_EDITOR && UNITY_ANDROID && HYPERGAMES_STORE_RATING
            builder.Register<AndroidStoreRatingService>(Lifetime.Singleton).AsImplementedInterfaces();
            #elif !UNITY_EDITOR && UNITY_IOS && HYPERGAMES_STORE_RATING
            builder.Register<IosStoreRatingService>(Lifetime.Singleton).AsImplementedInterfaces();
            #else
            builder.Register<DummyStoreRatingService>(Lifetime.Singleton).AsImplementedInterfaces();
            #endif
            builder.Register<UnityTemplateStoreRatingHandler>(Lifetime.Singleton);
        }
    }
}
#endif