namespace HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle.AllRewards;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateRewardHandler
    {
        private readonly IReadOnlyDictionary<string, IUnityTemplateRewardExecutor> rewardIdToRewardExecutor;
        private readonly UnityTemplateRewardDataController                         UnityTemplateRewardDataController;
        private readonly UnityTemplateInventoryDataController                      UnityTemplateInventoryDataController;

        [Preserve]
        public UnityTemplateRewardHandler(
            IEnumerable<IUnityTemplateRewardExecutor> rewardExecutors,
            UnityTemplateRewardDataController         UnityTemplateRewardDataController,
            UnityTemplateInventoryDataController      UnityTemplateInventoryDataController
        )
        {
            this.rewardIdToRewardExecutor          = rewardExecutors.ToDictionary(rewardExecutor => rewardExecutor.RewardId);
            this.UnityTemplateRewardDataController    = UnityTemplateRewardDataController;
            this.UnityTemplateInventoryDataController = UnityTemplateInventoryDataController;
        }

        public Dictionary<string, int> ClaimRepeatedReward()
        {
            var rewardList = this.UnityTemplateRewardDataController.GetAvailableRepeatedReward();
            var availableRepeatedReward = rewardList
                .GroupBy(keyPairValue => keyPairValue.Key)
                .ToDictionary(group => group.Key, group => group.Sum(keyPairValue => keyPairValue.Value.RewardValue));

            foreach (var (rewardId, value) in availableRepeatedReward) this.ReceiveReward(rewardId, value);

            rewardList.ForEach(keyPairValue => keyPairValue.Value.LastTimeReceive = DateTime.Now);

            return availableRepeatedReward;
        }

        public void AddRewardsWithPackId(string iapPackId, Dictionary<string, UnityTemplateRewardItemData> rewardIdToData, GameObject sourceGameObject)
        {
            this.UnityTemplateRewardDataController.AddRepeatedReward(iapPackId, rewardIdToData);

            this.AddRewards(rewardIdToData, sourceGameObject);
        }

        public void AddRewards(Dictionary<string, UnityTemplateRewardItemData> rewardIdToData, GameObject sourceGameObject)
        {
            foreach (var rewardData in rewardIdToData) this.ReceiveReward(rewardData.Key, rewardData.Value.RewardValue, sourceGameObject == null ? null : sourceGameObject.transform as RectTransform);
        }

        private void ReceiveReward(string rewardId, int rewardValue, RectTransform startPos = null)
        {
            if (this.rewardIdToRewardExecutor.TryGetValue(rewardId, out var dicRewardRecord))
                dicRewardRecord.ReceiveReward(rewardValue, startPos);
            else
                this.UnityTemplateInventoryDataController.AddGenericReward(rewardId, rewardValue, startPos).Forget();
        }
    }
}