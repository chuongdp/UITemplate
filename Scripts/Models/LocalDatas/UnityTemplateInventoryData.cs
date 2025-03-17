namespace HyperGames.UnityTemplate.UnityTemplate.Models
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using Sirenix.Serialization;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateInventoryData : ILocalData, IUnityTemplateLocalData
    {
        [OdinSerialize] public readonly Dictionary<string, string> CategoryToChosenItem = new();

        [OdinSerialize] public Dictionary<string, UnityTemplateItemData> IDToItemData { get; private set; } = new();

        [OdinSerialize] public Dictionary<string, UnityTemplateCurrencyData> IDToCurrencyData { get; private set; } = new();

        public Type ControllerType => typeof(UnityTemplateInventoryDataController);

        public void Init()
        {
        }
    }
}