namespace HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle.AllRewards
{
    using UnityEngine;

    public interface IUnityTemplateRewardExecutor
    {
        string RewardId { get; }
        void   ReceiveReward(int value, RectTransform startPosAnimation);
    }

    public abstract class UnityTemplateRewardExecutorBase : IUnityTemplateRewardExecutor
    {
        public abstract string RewardId { get; }

        public abstract void ReceiveReward(int value, RectTransform startPosAnimation);
    }
}