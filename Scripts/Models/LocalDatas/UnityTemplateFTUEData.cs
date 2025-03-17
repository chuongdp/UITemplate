namespace HyperGames.UnityTemplate.UnityTemplate.Models
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using Sirenix.Serialization;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateFTUEData : ILocalData, IUnityTemplateLocalData
    {
        [OdinSerialize] public List<string> FinishedStep { get; set; } = new();

        [OdinSerialize] public string CurrentStep { get; set; } = "";

        public void Init()
        {
        }

        public Type ControllerType => typeof(UnityTemplateFTUEDataController);
    }
}