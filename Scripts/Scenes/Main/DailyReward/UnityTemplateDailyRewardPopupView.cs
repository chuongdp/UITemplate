namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Configs.GameEvents;
    using HyperGames.UnityTemplate.UnityTemplate.Models;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Pack;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    public class UnityTemplateDailyRewardPopupView : BaseView
    {
        [FormerlySerializedAs("dailyRewardItemAdapter")] [FormerlySerializedAs("dailyRewardAdapter")] public UnityTemplateDailyRewardPackAdapter dailyRewardPackAdapter;

        public Button btnClaim;
        public Button btnClose;
        public string claimSoundKey;
    }

    public class UnityTemplateDailyRewardPopupModel
    {
    }

    [PopupInfo(nameof(UnityTemplateDailyRewardPopupView), false, isOverlay: true)]
    public class UnityTemplateDailyRewardPopupPresenter : UnityTemplateBasePopupPresenter<UnityTemplateDailyRewardPopupView, UnityTemplateDailyRewardPopupModel>
    {
        #region inject

        private readonly UnityTemplateDailyRewardController UnityTemplateDailyRewardController;
        private readonly UnityTemplateDailyRewardBlueprint  unityTemplateDailyRewardBlueprint;
        private readonly UnityTemplateLevelDataController   levelDataController;
        private readonly UnityTemplateAdServiceWrapper      UnityTemplateAdServiceWrapper;
        private readonly DailyRewardAnimationHelper      dailyRewardAnimationHelper;
        private readonly GameFeaturesSetting             gameFeaturesSetting;

        #endregion

        private UnityTemplateDailyRewardPopupModel      popupModel;
        private List<UnityTemplateDailyRewardPackModel> listRewardModel;
        private CancellationTokenSource              closeViewCts;

        [Preserve]
        public UnityTemplateDailyRewardPopupPresenter(
            SignalBus                       signalBus,
            ILogService                     logger,
            UnityTemplateDailyRewardController UnityTemplateDailyRewardController,
            UnityTemplateDailyRewardBlueprint  unityTemplateDailyRewardBlueprint,
            UnityTemplateLevelDataController   levelDataController,
            UnityTemplateAdServiceWrapper      UnityTemplateAdServiceWrapper,
            DailyRewardAnimationHelper      dailyRewardAnimationHelper,
            GameFeaturesSetting             gameFeaturesSetting
        ) : base(signalBus, logger)
        {
            this.UnityTemplateDailyRewardController = UnityTemplateDailyRewardController;
            this.unityTemplateDailyRewardBlueprint  = unityTemplateDailyRewardBlueprint;
            this.levelDataController             = levelDataController;
            this.UnityTemplateAdServiceWrapper      = UnityTemplateAdServiceWrapper;
            this.dailyRewardAnimationHelper      = dailyRewardAnimationHelper;
            this.gameFeaturesSetting             = gameFeaturesSetting;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.levelDataController.UnlockFeature(UnityTemplateItemData.UnlockType.DailyReward);
            this.View.btnClaim.onClick.AddListener(this.ClaimReward);
            this.View.btnClose.onClick.AddListener(this.CloseView);
            this.View.btnClose.onClick.AddListener(() => this.View.btnClose.gameObject.SetActive(false));
        }

        public override UniTask BindData(UnityTemplateDailyRewardPopupModel param)
        {
            this.popupModel = param;

            this.listRewardModel = this.unityTemplateDailyRewardBlueprint.Values
                .Select(UnityTemplateDailyRewardRecord =>
                    new UnityTemplateDailyRewardPackModel(
                        UnityTemplateDailyRewardRecord,
                        this.UnityTemplateDailyRewardController.GetDateRewardStatus(UnityTemplateDailyRewardRecord.Day),
                        this.OnItemClick
                    )
                )
                .ToList();

            this.SetUpItemCanPreReceiveWithAds();
            this.InitListDailyReward(this.listRewardModel);

            var hasRewardCanClaim = this.UnityTemplateDailyRewardController.CanClaimReward;
            this.View.btnClaim.gameObject.SetActive(hasRewardCanClaim);
            this.View.btnClose.gameObject.SetActive(!hasRewardCanClaim);
            return UniTask.CompletedTask;
        }

        private void OnItemClick(UnityTemplateDailyRewardPackPresenter presenter)
        {
            var model = presenter.Model;
            switch (model.RewardStatus)
            {
                case RewardStatus.Locked when model.IsGetWithAds:
                    this.ClaimAdsReward(model);
                    break;
                case RewardStatus.Unlocked:
                    this.ClaimReward();
                    break;
            }
        }

        private void ClaimAdsReward(UnityTemplateDailyRewardPackModel model)
        {
            this.UnityTemplateAdServiceWrapper.ShowRewardedAd(this.gameFeaturesSetting.DailyRewardConfig.dailyRewardAdPlacementId,
                () =>
                {
                    this.UnityTemplateDailyRewardController.UnlockDailyReward(model.DailyRewardRecord.Day);
                    this.listRewardModel[model.DailyRewardRecord.Day - 1].RewardStatus = RewardStatus.Unlocked;

                    this.ClaimReward();
                });
        }

        private void SetUpItemCanPreReceiveWithAds()
        {
            switch (this.gameFeaturesSetting.DailyRewardConfig.preReceiveDailyRewardStrategy)
            {
                case PreReceiveDailyRewardStrategy.None: break;
                case PreReceiveDailyRewardStrategy.NextDay:
                    NextDayPreReceiveReward();
                    break;
                case PreReceiveDailyRewardStrategy.Custom:
                    CustomPreReceiveReward();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            return;

            void NextDayPreReceiveReward()
            {
                foreach (var model in this.listRewardModel)
                {
                    if (model.RewardStatus == RewardStatus.Locked)
                    {
                        model.IsGetWithAds = true;
                        break;
                    }
                }
            }

            void CustomPreReceiveReward()
            {
                var preReceiveConfig = this.gameFeaturesSetting.DailyRewardConfig.preReceiveConfig;
                foreach (var model in this.listRewardModel)
                    if (model.RewardStatus == RewardStatus.Locked && preReceiveConfig.TryGetValue(model.DailyRewardRecord.Day, out var canReceive))
                        model.IsGetWithAds = canReceive;
            }
        }

        private void InitListDailyReward(List<UnityTemplateDailyRewardPackModel> dailyRewardModels)
        {
            this.View.dailyRewardPackAdapter.InitItemAdapter(dailyRewardModels).Forget();
        }

        private void ClaimReward()
        {
            this.ClaimRewardAsync().Forget();
        }

        private async UniTask ClaimRewardAsync()
        {
            this.View.btnClaim.gameObject.SetActive(false);
            this.View.btnClose.gameObject.SetActive(true);

            await this.dailyRewardAnimationHelper.PlayPreClaimRewardAnimation(this);

            var dayToView = new Dictionary<int, RectTransform>();
            for (var i = 0; i < this.listRewardModel.Count; i++)
                if (this.listRewardModel[i].RewardStatus == RewardStatus.Unlocked)
                    dayToView.Add(this.listRewardModel[i].DailyRewardRecord.Day, this.View.dailyRewardPackAdapter.GetPresenterAtIndex(i).View.transform as RectTransform);

            this.UnityTemplateDailyRewardController.ClaimAllAvailableReward(dayToView, this.View.claimSoundKey);

            var claimedPresenter = new List<UnityTemplateDailyRewardPackPresenter>();

            for (var i = 0; i < this.listRewardModel.Count; i++)
            {
                if (this.listRewardModel[i].RewardStatus == RewardStatus.Unlocked)
                {
                    claimedPresenter.Add(this.View.dailyRewardPackAdapter.GetPresenterAtIndex(i));
                    this.listRewardModel[i].RewardStatus = RewardStatus.Claimed;
                }
            }

            await this.dailyRewardAnimationHelper.PlayPostClaimRewardAnimation(this);

            this.SetUpItemCanPreReceiveWithAds();
            this.RefreshAdapter();

            // call claim reward after refresh adapter for animation
            claimedPresenter.ForEach(presenter => presenter.ClaimReward());

            this.AutoClosePopup();
        }

        private void AutoClosePopup()
        {
            if (this.gameFeaturesSetting.DailyRewardConfig.preReceiveDailyRewardStrategy != PreReceiveDailyRewardStrategy.None) return;

            UniTask.Delay(TimeSpan.FromSeconds(1.5f),
                    cancellationToken: (this.closeViewCts = new()).Token,
                    ignoreTimeScale: true)
                .ContinueWith(this.CloseViewAsync)
                .Forget();
        }

        private void RefreshAdapter()
        {
            this.View.dailyRewardPackAdapter.Refresh();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.closeViewCts?.Cancel();
            this.closeViewCts?.Dispose();
            this.closeViewCts = null;
        }
    }
}