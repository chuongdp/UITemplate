#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.DeepLinking
{
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Services.DeepLinking;
    using VContainer;

    public static class DeepLinkServiceVContainer
    {
        public static void RegisterDeepLinkService(this IContainerBuilder builder)
        {
            builder.Register<DeepLinkService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.DeclareSignal<OnDeepLinkActiveSignal>();
        }
    }
}
#endif