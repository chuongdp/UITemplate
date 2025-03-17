namespace HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas
{
    using System;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateCommonData : ILocalData, IUnityTemplateLocalData
    {
        public bool IsFirstTimeOpenGame { get; set; } = true;
        public Type ControllerType      => typeof(UnityTemplateCommonController);

        public void Init()
        {
        }
    }
}