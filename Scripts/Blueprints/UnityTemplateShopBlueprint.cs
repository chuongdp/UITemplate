namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using UnityEngine.Scripting;
    using BlueprintFlow.BlueprintReader;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;

    [Preserve]
    [BlueprintReader("UnityTemplateShop", true)]
    public class UnityTemplateShopBlueprint : GenericBlueprintReaderByRow<string, UnityTemplateShopRecord>
    {
    }

    [Preserve]
    [CsvHeaderKey("Id")]
    public class UnityTemplateShopRecord
    {
        public string                        Id         { get; [Preserve] private set; }
        public UnityTemplateItemData.UnlockType UnlockType { get; [Preserve] private set; }
        public string                        CurrencyID { get; [Preserve] private set; }
        public int                           Price      { get; [Preserve] private set; }
    }
}