namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateItem", true)]
    public class UnityTemplateItemBlueprint : GenericBlueprintReaderByRow<string, UnityTemplateItemRecord>
    {
    }

    [Preserve]
    [CsvHeaderKey("Id")]
    public class UnityTemplateItemRecord
    {
        public string Id            { get; [Preserve] private set; }
        public string Name          { get; [Preserve] private set; }
        public string Description   { get; [Preserve] private set; }
        public string ImageAddress  { get; [Preserve] private set; }
        public string Category      { get; [Preserve] private set; }
        public bool   IsDefaultItem { get; [Preserve] private set; }
    }
}