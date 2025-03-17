namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using HyperGames.UnityTemplate.Scripts.Services;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateDailyRewardController : IUnityTemplateControllerData
    {
        private const int TotalDayInWeek = 7;

        #region inject

        private readonly IInternetService                    internetService;
        private readonly UnityTemplateDailyRewardData           UnityTemplateDailyRewardData;
        private readonly UnityTemplateDailyRewardBlueprint      unityTemplateDailyRewardBlueprint;
        private readonly UnityTemplateInventoryDataController   UnityTemplateInventoryDataController;
        private readonly UnityTemplateFlyingAnimationController UnityTemplateFlyingAnimationController;

        #endregion

        private SemaphoreSlim mySemaphoreSlim = new(1, 1);

        [Preserve]
        public UnityTemplateDailyRewardController(
            IInternetService                    internetService,
            UnityTemplateDailyRewardData           UnityTemplateDailyRewardData,
            UnityTemplateDailyRewardBlueprint      unityTemplateDailyRewardBlueprint,
            UnityTemplateInventoryDataController   UnityTemplateInventoryDataController,
            UnityTemplateFlyingAnimationController UnityTemplateFlyingAnimationController
        )
        {
            this.internetService                     = internetService;
            this.UnityTemplateDailyRewardData           = UnityTemplateDailyRewardData;
            this.unityTemplateDailyRewardBlueprint      = unityTemplateDailyRewardBlueprint;
            this.UnityTemplateInventoryDataController   = UnityTemplateInventoryDataController;
            this.UnityTemplateFlyingAnimationController = UnityTemplateFlyingAnimationController;
        }

        public async UniTask CheckRewardStatus()
        {
            await this.mySemaphoreSlim.WaitAsync();

            try
            {
                // var currentTime = await this.internetService.GetCurrentTimeAsync();
                var currentTime = DateTime.Now; // Because the internet service getting time doesn't work stable I use this instead, btw, we allow hyper casual players cheat the game.
                var issDiffDay  = this.internetService.IsDifferentDay(this.UnityTemplateDailyRewardData.LastRewardedDate, currentTime);

                if (!issDiffDay) return;

                var firstLockedDayIndex = this.FindFirstLockedDayIndex();

                if (firstLockedDayIndex == -1)
                {
                    if (!this.CanClaimReward) this.InitRewardStatus(currentTime);
                }
                else
                {
                    if (firstLockedDayIndex / TotalDayInWeek == firstLockedDayIndex / TotalDayInWeek)
                    {
                        this.UnityTemplateDailyRewardData.RewardStatus[firstLockedDayIndex] = RewardStatus.Unlocked;
                        this.UnityTemplateDailyRewardData.LastRewardedDate                  = currentTime;
                    }
                }
            }
            finally
            {
                this.mySemaphoreSlim.Release();
            }
        }

        private int FindFirstLockedDayIndex()
        {
            return this.UnityTemplateDailyRewardData.RewardStatus.FirstIndex(status => status is RewardStatus.Locked);
        }

        public int GetCurrentDayIndex()
        {
            var firstLockedDayIndex = this.FindFirstLockedDayIndex();

            return firstLockedDayIndex == -1 ? this.UnityTemplateDailyRewardData.RewardStatus.Count - 1 : firstLockedDayIndex - 1;
        }

        public void UnlockDailyReward(int day)
        {
            this.UnityTemplateDailyRewardData.RewardStatus[day - 1] = RewardStatus.Unlocked;
        }

        /// <param name="day"> start from 1</param>
        /// <returns></returns>
        public RewardStatus GetDateRewardStatus(int day)
        {
            return this.UnityTemplateDailyRewardData.RewardStatus[day - 1];
        }

        public async void ClaimAllAvailableReward(Dictionary<int, RectTransform> dayToView, string claimSoundKey = null)
        {
            var playAnimTask = UniTask.CompletedTask;

            for (var i = 0; i < this.UnityTemplateDailyRewardData.RewardStatus.Count; i++)
            {
                if (this.UnityTemplateDailyRewardData.RewardStatus[i] == RewardStatus.Unlocked)
                {
                    this.UnityTemplateDailyRewardData.RewardStatus[i] = RewardStatus.Claimed;

                    var reward = this.unityTemplateDailyRewardBlueprint.GetDataById(i + 1);

                    foreach (var (key, item) in reward.Reward) this.UnityTemplateInventoryDataController.AddGenericReward(item.RewardId, item.RewardValue, dayToView[reward.Day], claimSoundKey).Forget();
                }
            }

            await UniTask.WhenAny(playAnimTask);
        }

        private void InitRewardStatus(DateTime currentTime)
        {
            this.UnityTemplateDailyRewardData.RewardStatus = new();

            for (var i = 0; i < this.unityTemplateDailyRewardBlueprint.Values.Count; i++) this.UnityTemplateDailyRewardData.RewardStatus.Add(RewardStatus.Locked);

            this.UnityTemplateDailyRewardData.RewardStatus[0]  = RewardStatus.Unlocked;
            this.UnityTemplateDailyRewardData.LastRewardedDate = currentTime;
        }

        public bool CanClaimReward => this.UnityTemplateDailyRewardData.RewardStatus.Any(t => t == RewardStatus.Unlocked);

        public DateTime GetFirstTimeOpenedDate => this.UnityTemplateDailyRewardData.FirstTimeOpenedDate;
    }
}