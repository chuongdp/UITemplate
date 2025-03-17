namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using UnityEngine.Scripting;

    public class UnityTemplateRewardDataController : IUnityTemplateControllerData
    {
        private readonly UnityTemplateRewardData UnityTemplateRewardData;

        [Preserve]
        public UnityTemplateRewardDataController(UnityTemplateRewardData UnityTemplateRewardData)
        {
            this.UnityTemplateRewardData = UnityTemplateRewardData;
        }

        public void AddRepeatedReward(string packID, Dictionary<string, UnityTemplateRewardItemData> rewardIdToData)
        {
            var storedRewardIdToData
                = this.UnityTemplateRewardData.PackIdToIdToRewardData.GetOrAdd(packID, () => new Dictionary<string, UnityTemplateRewardItemData>());

            foreach (var (rewardId, rewardData) in rewardIdToData)
            {
                if (rewardData.Repeat <= 0) continue;

                if (storedRewardIdToData.TryGetValue(rewardId, out var currentRewardData))
                {
                    currentRewardData.RewardValue += rewardData.RewardValue;
                }
                else
                {
                    rewardData.LastTimeReceive = DateTime.Now;
                    storedRewardIdToData.Add(rewardId, rewardData);
                }
            }
        }

        public List<KeyValuePair<string, UnityTemplateRewardItemData>> GetAvailableRepeatedReward()
        {
            return this.UnityTemplateRewardData.PackIdToIdToRewardData.Values
                .SelectMany(rewardIdToData => rewardIdToData.ToList())
                .Where(keyPairValue => keyPairValue.Value.LastTimeReceive.DayOfYear + keyPairValue.Value.Repeat <= DateTime.Now.DayOfYear)
                .ToList();
        }

        public bool IsExistAvailableRepeatedReward()
        {
            return this.GetAvailableRepeatedReward().Count > 0;
        }
    }
}