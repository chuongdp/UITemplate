namespace HyperGames.UnityTemplate.Quests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlueprintFlow.BlueprintReader;
    using BlueprintFlow.BlueprintReader.Converter;
    using BlueprintFlow.BlueprintReader.Converter.TypeConversion;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.Quests.Conditions;
    using HyperGames.UnityTemplate.Quests.Rewards;
    using HyperGames.UnityTemplate.Quests.TargetHandler;
    using Newtonsoft.Json;
    using UnityEngine.Scripting;

    [Preserve]
    [BlueprintReader("UnityTemplateQuest")]
    public sealed class UnityTemplateQuestBlueprint : GenericBlueprintReaderByRow<string, QuestRecord>
    {
        static UnityTemplateQuestBlueprint()
        {
            CsvHelper.RegisterTypeConverter(typeof(IReward), new JsonConverter<IReward>());
            CsvHelper.RegisterTypeConverter(typeof(ICondition), new JsonConverter<ICondition>());
            CsvHelper.RegisterTypeConverter(typeof(IRedirectTarget), new JsonConverter<IRedirectTarget>());
            CsvHelper.RegisterTypeConverter(typeof(List<IReward>), new ListGenericConverter(';'));
            CsvHelper.RegisterTypeConverter(typeof(List<ICondition>), new ListGenericConverter(';'));
            CsvHelper.RegisterTypeConverter(typeof(List<IRedirectTarget>), new ListGenericConverter(';'));
            CsvHelper.RegisterTypeConverter(typeof(HashSet<string>), new HashSetConverter());
        }

        private sealed class JsonConverter<T> : ITypeConverter
        {
            private readonly Dictionary<string, Type> typeMap;

            public JsonConverter()
            {
                var postFix = typeof(T).Name[1..];
                this.typeMap = ReflectionUtils.GetAllDerivedTypes<T>()
                    .ToDictionary(type =>
                        type.Name.EndsWith(postFix)
                            ? type.Name[..^postFix.Length]
                            : type.Name
                    );
            }

            object ITypeConverter.ConvertFromString(string str, Type _)
            {
                var index = str.IndexOf(":", StringComparison.Ordinal);
                var type  = this.typeMap[str[..index]];
                return JsonConvert.DeserializeObject(str[(index + 1)..], type);
            }

            string ITypeConverter.ConvertToString(object obj, Type type)
            {
                var typeStr = this.typeMap.First(kv => kv.Value == type).Key;
                return $"{typeStr}:{JsonConvert.SerializeObject(obj)}";
            }
        }

        private sealed class HashSetConverter : ITypeConverter
        {
            object ITypeConverter.ConvertFromString(string str, Type _)
            {
                return new HashSet<string>(str.Split(';'));
            }

            string ITypeConverter.ConvertToString(object obj, Type type)
            {
                return string.Join(';', (HashSet<string>)obj);
            }
        }
    }

    [Preserve]
    [CsvHeaderKey(nameof(Id))]
    public sealed class QuestRecord
    {
        public string          Id          { get; [Preserve] private set; }
        public string          Name        { get; [Preserve] private set; }
        public string          Description { get; [Preserve] private set; }
        public string          Image       { get; [Preserve] private set; }
        public HashSet<string> Tags        { get; [Preserve] private set; }

        public List<IReward>         Rewards            { get; [Preserve] private set; }
        public List<ICondition>      StartConditions    { get; [Preserve] private set; }
        public List<ICondition>      ShowConditions     { get; [Preserve] private set; }
        public List<ICondition>      CompleteConditions { get; [Preserve] private set; }
        public List<ICondition>      ResetConditions    { get; [Preserve] private set; }
        public List<IRedirectTarget> Target             { get; [Preserve] private set; }

        public bool HasTag(string tag)
        {
            return this.Tags.Contains(tag);
        }
    }
}