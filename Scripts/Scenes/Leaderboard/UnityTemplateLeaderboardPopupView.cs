namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Leaderboard
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services.CountryFlags.CountryFlags.Scripts;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;

    public class UnityTemplateLeaderboardPopupView : BaseView
    {
        public UnityTemplateLeaderboardAdapter Adapter;
        public Button                       CloseButton;
        public Transform                    YourRankerParentTransform;
        public CountryFlags                 CountryFlags;
        public TMP_Text                     BetterThanText;
        public int                          MaxLevel    = 200;
        public int                          LowestRank  = 68365;
        public int                          HighestRank = 156;
        public int                          RankRange => this.LowestRank - this.HighestRank;
    }

    [PopupInfo(nameof(UnityTemplateLeaderboardPopupView), false)]
    public class UnityTemplateLeaderBoardPopupPresenter : UnityTemplateBasePopupPresenter<UnityTemplateLeaderboardPopupView>
    {
        private const string SFXLeaderboard = "sfx_leaderboard";

        #region inject

        private readonly UnityTemplateLevelDataController UnityTemplateLevelDataController;
        private readonly UnityTemplateSoundServices       UnityTemplateSoundServices;

        #endregion

        private GameObject              yourClone;
        private CancellationTokenSource animationCancelTokenSource;
        private List<Tween>             animationTweenList = new();

        [Preserve]
        public UnityTemplateLeaderBoardPopupPresenter(
            SignalBus                     signalBus,
            ILogService                   logger,
            UnityTemplateLevelDataController UnityTemplateLevelDataController,
            UnityTemplateSoundServices       UnityTemplateSoundServices
        ) : base(signalBus, logger)
        {
            this.UnityTemplateLevelDataController = UnityTemplateLevelDataController;
            this.UnityTemplateSoundServices       = UnityTemplateSoundServices;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CloseButton.onClick.AddListener(this.CloseView);
        }

        private int GetRankWithLevel(int level)
        {
            return (int)(this.View.LowestRank - Mathf.Sqrt(Mathf.Sqrt(level * 1f / this.View.MaxLevel)) * this.View.RankRange);
        }

        public override UniTask BindData()
        {
            this.DoAnimation().Forget();
            return UniTask.CompletedTask;
        }

        private async UniTaskVoid DoAnimation()
        {
            var indexPadding   = 4;
            var scrollDuration = 3;
            var scaleTime      = 1f;

            var TestList = new List<UnityTemplateLeaderboardItemModel>();

            var currentLevel = this.UnityTemplateLevelDataController.GetCurrentLevelData.Level;
            var oldRank      = this.GetRankWithLevel(currentLevel - 1);
            var newRank      = this.GetRankWithLevel(currentLevel);
            var newIndex     = indexPadding;
            var oldIndex     = oldRank - newRank - indexPadding;

            for (var i = newRank - indexPadding; i < oldRank + indexPadding; i++) TestList.Add(new(i, this.View.CountryFlags.GetRandomFlag(), NVJOBNameGen.GiveAName(Random.Range(1, 8)), false));

            TestList[newIndex].IsYou       = true;
            TestList[oldIndex].IsYou       = true;
            TestList[oldIndex].CountryFlag = this.View.CountryFlags.GetLocalDeviceFlagByDeviceLang();
            TestList[oldIndex].Name        = "You";

            this.UnityTemplateSoundServices.PlaySound(SFXLeaderboard);

            //Setup view
            await this.View.Adapter.InitItemAdapter(TestList);
            this.View.Adapter.ScrollTo(oldIndex - indexPadding);

            //Create your clone
            this.yourClone                                   = Object.Instantiate(this.View.Adapter.GetItemViewsHolderIfVisible(oldIndex).root.gameObject, this.View.YourRankerParentTransform);
            this.yourClone.GetComponent<CanvasGroup>().alpha = 1;
            var cloneView = this.yourClone.GetComponent<UnityTemplateLeaderboardItemView>();
            this.View.BetterThanText.text = this.GetBetterThanText(oldRank);

            this.animationCancelTokenSource = new();
            //Do animation
            //Do scale up
            this.animationTweenList.Clear();
            this.animationTweenList.Add(this.yourClone.transform.DOScale(Vector3.one * 1.1f, scaleTime).SetEase(Ease.InOutBack).SetUpdate(true));
            await UniTask.Delay(TimeSpan.FromSeconds(scaleTime), cancellationToken: this.animationCancelTokenSource.Token, ignoreTimeScale: true);
            //Do move to new rank
            cloneView.ShowRankUP();
            this.animationTweenList.Add(DOTween.To(() => 0, setValue => cloneView.SetRankUp(setValue), oldRank - newRank, scrollDuration).SetUpdate(true));
            this.animationTweenList.Add(DOTween.To(() => oldRank,
                setValue =>
                {
                    cloneView.SetRank(setValue);
                    this.View.BetterThanText.text = this.GetBetterThanText(setValue);
                },
                newRank,
                scrollDuration).SetUpdate(true));
            this.View.Adapter.SmoothScrollTo(newIndex - indexPadding, scrollDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(scrollDuration), cancellationToken: this.animationCancelTokenSource.Token, ignoreTimeScale: true);
            //Do scale down
            this.animationTweenList.Add(this.yourClone.transform.DOScale(Vector3.one, scaleTime).SetEase(Ease.InOutBack).SetUpdate(true));
            await UniTask.Delay(TimeSpan.FromSeconds(scaleTime + 2), cancellationToken: this.animationCancelTokenSource.Token, ignoreTimeScale: true);
            this.CloseView();
        }

        private string GetBetterThanText(int currentRank)
        {
            return $"you are better than <color=#2DF2FF><size=120%>{(this.View.LowestRank * 1.5f - currentRank) / (this.View.LowestRank * 1.5f) * 100:F2}%</size ></color > of people";
        }

        public override void Dispose()
        {
            base.Dispose();
            this.UnityTemplateSoundServices.StopSound(SFXLeaderboard);
            this.animationCancelTokenSource.Cancel();
            this.animationCancelTokenSource.Dispose();
            this.View.Adapter.StopScrollingIfAny();
            foreach (var tween in this.animationTweenList) tween.Kill();

            Object.Destroy(this.yourClone);
        }
    }
}