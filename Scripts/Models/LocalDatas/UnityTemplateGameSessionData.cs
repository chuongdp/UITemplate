namespace HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas
{
    using System;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using Sirenix.Serialization;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateGameSessionData : ILocalData, IUnityTemplateLocalData
    {
        [OdinSerialize] public DateTime FirstInstallDate { get; set; } = DateTime.Now;
        public                 int      OpenTime;

        public void Init()
        {
        }

        public Type ControllerType => typeof(UnityTemplateGameSessionDataController);
    }
}