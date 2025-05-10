namespace GameTemplate.UnityTemplate.Models
{
    using System;
    using GameFoundation.Scripts.Interfaces;
    using GameTemplate.UnityTemplate.Models.Controllers;
    using GameTemplate.UnityTemplate.Models.LocalDatas;
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