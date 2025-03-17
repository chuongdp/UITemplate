#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.Quests
{
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Quests.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Quests.Signals;
    using UnityEngine;
    using Zenject;

    public class UnityTemplateQuestInstaller : Installer<UnityTemplateQuestInstaller>
    {
        public override void InstallBindings()
        {
#if HYPERGAMES_QUEST_SYSTEM
            this.Container.BindInterfacesAndSelfTo<UnityTemplateQuestManager>().AsSingle();

            if (Object.FindObjectOfType<UnityTemplateQuestNotificationView>() is { } notificationView)
            {
                this.Container.BindInterfacesAndSelfTo<UnityTemplateQuestNotificationView>()
                    .FromInstance(notificationView)
                    .AsSingle();
            }

            this.Container.DeclareSignal<QuestStatusChangedSignal>();
            this.Container.DeclareSignal<ClaimAllQuestSignal>();
#endif
        }
    }
}
#endif