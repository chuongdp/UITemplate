namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Item
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using UnityEngine;
    using UnityEngine.Scripting;

    // Rebind this class to your own item view
    public class UnityTemplateDailyRewardItemViewHelper
    {
        protected readonly IGameAssets                     GameAssets;
        protected readonly UnityTemplateDailyRewardController DailyRewardController;

        [Preserve]
        public UnityTemplateDailyRewardItemViewHelper(IGameAssets gameAssets, UnityTemplateDailyRewardController dailyRewardController)
        {
            this.GameAssets            = gameAssets;
            this.DailyRewardController = dailyRewardController;
        }

        public virtual void BindDataItem(UnityTemplateDailyRewardItemModel model, UnityTemplateDailyRewardItemView view, UnityTemplateDailyRewardItemPresenter presenter)
        {
            view.ImgReward.gameObject.SetActive(!string.IsNullOrEmpty(model.RewardRecord.RewardImage));
            if (!string.IsNullOrEmpty(model.RewardRecord.RewardImage))
            {
                var rewardSprite = this.GameAssets.ForceLoadAsset<Sprite>($"{model.RewardRecord.RewardImage}");
                view.ImgReward.sprite = rewardSprite;
            }

            view.TxtValue.text = $"{model.RewardRecord.RewardValue}";
            view.TxtValue.gameObject.SetActive(model.RewardRecord.ShowValue);
            view.UpdateIconRectTransform(model.RewardRecord.Position, model.RewardRecord.Size);
            view.ObjReward.SetActive(model.RewardStatus != RewardStatus.Locked || model.RewardRecord.SpoilReward);
            view.ObjLock.SetActive(!view.ObjReward.activeSelf);
        }

        public virtual void DisposeItem(UnityTemplateDailyRewardItemPresenter presenter)
        {
        }
    }
}