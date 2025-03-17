#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate.UnityTemplate.Creative.Cheat
{
    #if HYPERGAMES_CHEAT
    using GameFoundation.DI;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Services;
    #endif
    using UnityEngine;
    using VContainer;

    public static class HyperGamesCheatVContainer
    {
        public static void RegisterCheatView(this IContainerBuilder builder)
        {
            #if HYPERGAMES_CHEAT
            #if CREATIVE
            builder.RegisterBuildCallback(container => container.Resolve<CreativeService>().DisableTripleTap());
            #endif
            builder.Register<HyperGamesCheatGenerate>(Lifetime.Singleton).AsImplementedInterfaces();
            #endif
        }
    }
}
#endif