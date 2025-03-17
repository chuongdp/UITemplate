namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateCategoryItem", true)]
    public class UnityTemplateCategoryItemBlueprint : GenericBlueprintReaderByRow<string, UnityTemplateCategoryItemRecord>
    {
    }

    [Preserve]
    [CsvHeaderKey("Id")]
    public class UnityTemplateCategoryItemRecord
    {
        public string Id    { get; [Preserve] private set; }
        public string Icon  { get; [Preserve] private set; }
        public string Title { get; [Preserve] private set; }
    }
}