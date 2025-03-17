namespace HyperGames.UnityTemplate.Quests.Conditions
{
    using System;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using Newtonsoft.Json;
    using UnityEngine.Scripting;

    [Preserve]
    public sealed class WatchAdCountCondition : BaseCondition
    {
        [JsonProperty] private int Count { get; [Preserve] set; }

        protected override ICondition.IProgress SetupProgress()
        {
            return new Progress();
        }

        [Preserve]
        private sealed class Progress : BaseProgress
        {
            [JsonProperty] private int? StartCount { get; set; }

            protected override Type HandlerType => typeof(Handler);

            private sealed class Handler : BaseHandler<WatchAdCountCondition, Progress>
            {
                private readonly UnityTemplateAdsController adsDataController;

                [Preserve]
                public Handler(UnityTemplateAdsController adsDataController)
                {
                    this.adsDataController = adsDataController;
                }

                protected override float CurrentProgress => this.adsDataController.WatchRewardedAds - this.Progress.StartCount!.Value;
                protected override float MaxProgress     => this.Condition.Count;

                protected override void Initialize()
                {
                    this.Progress.StartCount ??= this.adsDataController.WatchRewardedAds;
                }
            }
        }
    }
}