namespace HyperGames.UnityTemplate.Quests.Conditions
{
    using System;
    using HyperGames.UnityTemplate.Quests.Data;
    using Newtonsoft.Json;
    using UnityEngine.Scripting;

    [Preserve]
    public sealed class CompleteQuestCondition : BaseCondition
    {
        [JsonProperty] private string QuestId { get; [Preserve] set; }

        protected override ICondition.IProgress SetupProgress()
        {
            return new Progress();
        }

        [Preserve]
        private sealed class Progress : BaseProgress
        {
            protected override Type HandlerType => typeof(Handler);

            private sealed class Handler : BaseHandler<CompleteQuestCondition, Progress>
            {
                private readonly UnityTemplateQuestManager questManager;

                [Preserve]
                public Handler(UnityTemplateQuestManager questManager)
                {
                    this.questManager = questManager;
                }

                protected override float CurrentProgress => this.otherQuest.Progress.Status.HasFlag(QuestStatus.Completed) ? 1f : 0f;
                protected override float MaxProgress     => 1f;

                private UnityTemplateQuestController otherQuest;

                protected override void Initialize()
                {
                    this.otherQuest = this.questManager.GetController(this.Condition.QuestId);
                }
            }
        }
    }
}