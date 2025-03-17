namespace HyperGames.UnityTemplate.UnityTemplate.Models
{
    using System;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using Sirenix.Serialization;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateUserSettingData : ILocalData, IUnityTemplateLocalData
    {
        [OdinSerialize] public bool IsVibrationEnable = true;

        [OdinSerialize] public bool IsFlashLightEnable = true;

        public void Init()
        {
        }

        public Type ControllerType => typeof(UnityTemplateSettingDataController);
    }
}