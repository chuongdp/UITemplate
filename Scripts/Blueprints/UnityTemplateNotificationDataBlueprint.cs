namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateNotificationData", true)]
    public class UnityTemplateNotificationDataBlueprint : GenericBlueprintReaderByRow<string, UnityTemplateNotificationDataRecord>
    {
    }

    [Preserve]
    [CsvHeaderKey("Id")]
    public class UnityTemplateNotificationDataRecord
    {
        public string Id         { get; [Preserve] private set; }
        public string Title      { get; [Preserve] private set; }
        public string Body       { get; [Preserve] private set; }
        public bool   RandomAble { get; [Preserve] private set; }
    }
}