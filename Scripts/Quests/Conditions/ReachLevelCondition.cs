namespace HyperGames.UnityTemplate.Quests.Conditions
{
    using System;
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using Newtonsoft.Json;
    using UnityEngine.Scripting;

    [Preserve]
    public sealed class ReachLevelCondition : BaseCondition
    {
        [JsonProperty] private int Level { get; [Preserve] set; }

        protected override ICondition.IProgress SetupProgress()
        {
            return new Progress();
        }

        [Preserve]
        private sealed class Progress : BaseProgress
        {
            protected override Type HandlerType => typeof(Handler);

            private sealed class Handler : BaseHandler<ReachLevelCondition, Progress>
            {
                private readonly UnityTemplateLevelDataController levelDataController;

                [Preserve]
                public Handler(UnityTemplateLevelDataController levelDataController)
                {
                    this.levelDataController = levelDataController;
                }

                protected override float CurrentProgress => this.levelDataController.CurrentLevel < this.Condition.Level ? 0f : 1f;
                protected override float MaxProgress     => 1;
            }
        }
    }
}