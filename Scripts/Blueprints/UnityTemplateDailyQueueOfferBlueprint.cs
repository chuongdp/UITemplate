#if HYPERGAMES_DAILY_QUEUE_REWARD
namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateDailyQueueOffer", true)]
    public class UnityTemplateDailyQueueOfferBlueprint : GenericBlueprintReaderByRow<int, UnityTemplateDailyQueueOfferRecord>
    {
    }

    [Preserve]
    [CsvHeaderKey("Day")]
    public class UnityTemplateDailyQueueOfferRecord
    {
        public int                                                         Day        { get; [Preserve] private set; }
        public BlueprintByRow<string, UnityTemplateDailyQueueOfferItemRecord> OfferItems { get; [Preserve] private set; }
    }

    [Preserve]
    [CsvHeaderKey("OfferId")]
    public class UnityTemplateDailyQueueOfferItemRecord
    {
        public string OfferId       { get; [Preserve] private set; }
        public string ItemId        { get; [Preserve] private set; }
        public string ImageId       { get; [Preserve] private set; }
        public int    Value         { get; [Preserve] private set; }
        public bool   IsRewardedAds { get; [Preserve] private set; }
    }
}
#endif