namespace HyperGames.UnityTemplate.Scripts.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateMiscParam", true)]
    public class UnityTemplateMiscParamBlueprint : GenericBlueprintReaderByCol
    {
        public string PolicyAddress { get; [Preserve] private set; }
        public string TermsAddress  { get; [Preserve] private set; }
    }
}