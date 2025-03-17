namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using UnityEngine;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateDailyReward", true)]
    public class UnityTemplateDailyRewardBlueprint : GenericBlueprintReaderByRow<int, UnityTemplateDailyRewardRecord>
    {
    }

    [Preserve]
    [CsvHeaderKey("Day")]
    public class UnityTemplateDailyRewardRecord
    {
        public int                                            Day       { get; [Preserve] private set; }
        public string                                         PackImage { get; [Preserve] private set; }
        public BlueprintByRow<string, UnityTemplateRewardRecord> Reward    { get; [Preserve] private set; }
    }

    [Preserve]
    [CsvHeaderKey("RewardId")]
    public class UnityTemplateRewardRecord
    {
        public string   RewardId    { get; [Preserve] private set; }
        public int      RewardValue { get; [Preserve] private set; }
        public string   RewardImage { get; [Preserve] private set; }
        public Vector2? Position    { get; [Preserve] private set; }
        public Vector2? Size        { get; [Preserve] private set; }
        public bool     SpoilReward { get; [Preserve] private set; }
        public bool     ShowValue   { get; [Preserve] private set; }
    }
}