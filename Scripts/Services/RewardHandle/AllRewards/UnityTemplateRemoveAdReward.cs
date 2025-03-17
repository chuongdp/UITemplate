namespace HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle.AllRewards
{
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateRemoveAdRewardExecutorBase : UnityTemplateRewardExecutorBase
    {
        public const string REWARD_ID = "remove_ads";

        #region inject

        private readonly UnityTemplateAdServiceWrapper UnityTemplateAdServiceWrapper;

        #endregion

        public override string RewardId => REWARD_ID;

        [Preserve]
        public UnityTemplateRemoveAdRewardExecutorBase(UnityTemplateAdServiceWrapper UnityTemplateAdServiceWrapper)
        {
            this.UnityTemplateAdServiceWrapper = UnityTemplateAdServiceWrapper;
        }

        public override void ReceiveReward(int value, RectTransform startPosAnimation)
        {
            this.UnityTemplateAdServiceWrapper.RemoveAds();
        }
    }
}