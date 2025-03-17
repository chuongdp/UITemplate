#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate.Quests
{
    using GameFoundation.DI;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Quests.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Quests.Signals;
    using VContainer;

    public static class UnityTemplateQuestVContainer
    {
        public static void RegisterQuestManager(this IContainerBuilder builder)
        {
            #if HYPERGAMES_QUEST_SYSTEM
            builder.Register<UnityTemplateQuestManager>(Lifetime.Singleton).AsInterfacesAndSelf();

            builder.DeclareSignal<QuestStatusChangedSignal>();
            builder.DeclareSignal<ClaimAllQuestSignal>();
            #endif
        }
    }
}
#endif