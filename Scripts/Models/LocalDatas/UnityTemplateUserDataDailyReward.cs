namespace HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using Sirenix.Serialization;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateDailyRewardData : ILocalData, IUnityTemplateLocalData
    {
        [OdinSerialize] public List<RewardStatus> RewardStatus        { get; set; } = new();
        [OdinSerialize] public DateTime           LastRewardedDate    { get; set; }
        [OdinSerialize] public DateTime           FirstTimeOpenedDate { get; set; } = DateTime.Now;

        public void Init()
        {
        }

        public Type ControllerType => typeof(UnityTemplateDailyRewardController);
    }

    public enum RewardStatus
    {
        Locked   = 0,
        Unlocked = 1,
        Claimed  = 2,
    }
}