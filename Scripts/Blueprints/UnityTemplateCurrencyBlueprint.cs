namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateCurrency", true)]
    public class UnityTemplateCurrencyBlueprint : GenericBlueprintReaderByRow<string, UnityTemplateCurrencyRecord>
    {
    }

    [Preserve]
    [CsvHeaderKey("Id")]
    public class UnityTemplateCurrencyRecord
    {
        public string Id              { get; [Preserve] private set; }
        public string Name            { get; [Preserve] private set; }
        public string IconAddressable { get; [Preserve] private set; }
        public int    Max             { get; [Preserve] private set; }
        public string FlyingObject    { get; [Preserve] private set; }
    }
}