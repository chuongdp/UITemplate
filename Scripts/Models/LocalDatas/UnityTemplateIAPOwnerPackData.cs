namespace HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateIAPOwnerPackData : ILocalData, IUnityTemplateLocalData
    {
        public List<string> OwnedPacks { get; set; } = new();

        public void Init()
        {
        }

        public Type ControllerType => typeof(UnityTemplateIAPOwnerPackControllerData);
    }
}