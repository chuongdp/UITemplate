namespace HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using Sirenix.Serialization;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateRewardData : ILocalData, IUnityTemplateLocalData
    {
        //PackId can be any thing you want, it's just a key to store reward data
        [OdinSerialize] public Dictionary<string, Dictionary<string, UnityTemplateRewardItemData>> PackIdToIdToRewardData { get; set; } = new();

        public void Init()
        {
        }

        public Type ControllerType => typeof(UnityTemplateRewardDataController);
    }

    [Serializable]
    public class UnityTemplateRewardItemData
    {
        [OdinSerialize] public int      RewardValue           { get; set; }
        [OdinSerialize] public int      Repeat                { get; set; }
        [OdinSerialize] public string   AddressableFlyingItem { get; set; }
        [OdinSerialize] public DateTime LastTimeReceive       { get; set; }

        public UnityTemplateRewardItemData(int rewardValue, int repeat, string addressableFlyingItem)
        {
            this.AddressableFlyingItem = addressableFlyingItem;
            this.RewardValue           = rewardValue;
            this.Repeat                = repeat;
        }
    }
}