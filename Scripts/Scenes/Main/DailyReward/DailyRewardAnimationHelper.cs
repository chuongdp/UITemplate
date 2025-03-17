namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Item;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Pack;
    using UnityEngine.Scripting;

    public interface IDailyRewardAnimationHelper
    {
        public UniTask PlayPreClaimRewardAnimation(UnityTemplateDailyRewardPopupPresenter  dailyRewardPopupPresenter);
        public UniTask PlayPostClaimRewardAnimation(UnityTemplateDailyRewardPopupPresenter dailyRewardPopupPresenter);
    }

    [Preserve]
    public class DailyRewardAnimationHelper : IDailyRewardAnimationHelper
    {
        public virtual UniTask PlayPreClaimRewardAnimation(UnityTemplateDailyRewardPopupPresenter dailyRewardPopupPresenter)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask PlayPostClaimRewardAnimation(UnityTemplateDailyRewardPopupPresenter dailyRewardPopupPresenter)
        {
            return UniTask.CompletedTask;
        }

        protected List<UnityTemplateDailyRewardPackPresenter> GetPackPresenters(UnityTemplateDailyRewardPopupPresenter dailyRewardPopupPresenter)
        {
            return dailyRewardPopupPresenter.View.dailyRewardPackAdapter.GetPresenters();
        }

        protected List<UnityTemplateDailyRewardItemPresenter> GetItemPresenters(UnityTemplateDailyRewardPackPresenter packPresenter)
        {
            return packPresenter.View.DailyRewardItemAdapter.GetPresenters();
        }
    }
}