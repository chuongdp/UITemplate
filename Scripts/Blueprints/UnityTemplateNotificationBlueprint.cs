namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateNotification", true)]
    public class UnityTemplateNotificationBlueprint : GenericBlueprintReaderByRow<string, UnityTemplateNotificationRecord>
    {
    }

    [Preserve]
    [CsvHeaderKey("Id")]
    public class UnityTemplateNotificationRecord
    {
        public string    Id            { get; [Preserve] private set; }
        public List<int> HourRangeShow { get; [Preserve] private set; }
        public List<int> TimeToShow    { get; [Preserve] private set; }
        public bool      RandomAble    { get; [Preserve] private set; }
        public string    Title         { get; [Preserve] private set; }
        public string    Body          { get; [Preserve] private set; }
    }
}