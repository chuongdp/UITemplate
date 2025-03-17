#if HYPERGAMES_DAILY_QUEUE_REWARD
namespace HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using Sirenix.Serialization;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateDailyQueueOfferData : ILocalData, IUnityTemplateLocalData
    {
        [OdinSerialize] public DateTime                         LastOfferDate          { get; set; } = DateTime.MinValue;
        [OdinSerialize] public Dictionary<string, RewardStatus> OfferToStatusDuringDay { get; set; } = new();
        [OdinSerialize] public DateTime                         FirstTimeOpen          { get; set; } = DateTime.Now;

        public void Init() { }

        public Type ControllerType => typeof(UnityTemplateDailyQueueOfferDataController);
    }
}
#endif