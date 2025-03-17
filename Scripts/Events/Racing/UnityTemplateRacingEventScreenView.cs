namespace HyperGames.UnityTemplate.UnityTemplate.Events.Racing
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.Utilities.UIStuff;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.HyperCasual.GamePlay.Models;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using TMPro;
    using UIModule.Utilities;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateRacingEventScreenView : BaseView
    {
        public Slider                        progressSlider;
        public Button                        closeButton;
        public List<UnityTemplateRacingRowView> playerSliders;
        public TMP_Text                      userCurrentAmountText;

        public TMP_Text countDownText;
    }

    [PopupInfo(nameof(UnityTemplateRacingEventScreenView))]
    public abstract class UnityTemplateRacingEventScreenPresenter : UnityTemplateBasePopupPresenter<UnityTemplateRacingEventScreenView>
    {
        #region inject

        protected readonly UnityTemplateEventRacingDataController UnityTemplateEventRacingDataController;
        private readonly   AutoCooldownTimer                   autoCooldownTimer;

        #endregion

        private List<Tween> tweenList = new();

        [Preserve]
        protected UnityTemplateRacingEventScreenPresenter(
            SignalBus                           signalBus,
            ILogService                         logger,
            UnityTemplateEventRacingDataController UnityTemplateEventRacingDataController,
            AutoCooldownTimer                   autoCooldownTimer
        ) : base(signalBus, logger)
        {
            this.UnityTemplateEventRacingDataController = UnityTemplateEventRacingDataController;
            this.autoCooldownTimer                   = autoCooldownTimer;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.closeButton.onClick.AddListener(this.OnClickClose);
            this.autoCooldownTimer.CountDown(this.UnityTemplateEventRacingDataController.RemainSecond,
                _ =>
                {
                    this.View.countDownText.text = TimeSpan.FromSeconds(this.UnityTemplateEventRacingDataController.RemainSecond).ToShortTimeString();
                });

            this.InitPlayerRowView();
        }

        protected virtual void OnClickClose()
        {
            this.CloseView();
        }

        private void InitPlayerRowView()
        {
            for (var i = 0; i < this.View.playerSliders.Count; i++)
            {
                var rowView    = this.View.playerSliders[i];
                var playerData = this.UnityTemplateEventRacingDataController.GetPlayerData(i);
                rowView.InitView(playerData, i, this.CheckRacingEventComplete);
            }
        }

        public override UniTask BindData()
        {
            var oldShowScore = this.UnityTemplateEventRacingDataController.YourOldShowScore;
            this.UnityTemplateEventRacingDataController.UpdateUserOldShowScore();
            var yourNewScore = this.UnityTemplateEventRacingDataController.YourNewScore;

            var yourOldProgress      = 1f * oldShowScore / this.UnityTemplateEventRacingDataController.RacingScoreMax;
            var yourNewProgress      = 1f * yourNewScore / this.UnityTemplateEventRacingDataController.RacingScoreMax;
            var racingMaxProgression = this.UnityTemplateEventRacingDataController.RacingMaxProgression;

            this.View.userCurrentAmountText.text = $"{yourNewScore}/{this.UnityTemplateEventRacingDataController.RacingScoreMax}";

            this.View.progressSlider.value                                                                   = yourOldProgress;
            this.View.playerSliders[this.UnityTemplateEventRacingDataController.YourIndex].progressSlider.value = Mathf.Clamp(yourOldProgress, 0, racingMaxProgression);

            if (yourNewProgress > yourOldProgress)
            {
                this.tweenList.Add(DOTween.To(() => yourOldProgress,
                    x =>
                    {
                        this.View.progressSlider.value                                                                   = x;
                        this.View.playerSliders[this.UnityTemplateEventRacingDataController.YourIndex].progressSlider.value = x;
                    },
                    yourNewProgress,
                    1f).SetUpdate(true));
                this.tweenList.Add(DOTween.To(() => oldShowScore,
                        x =>
                        {
                            this.View.playerSliders[this.UnityTemplateEventRacingDataController.YourIndex].scoreText.text = x.ToString();
                        },
                        yourNewScore,
                        1f)
                    .SetUpdate(true));
            }

            var simulatePlayerScore = this.UnityTemplateEventRacingDataController.SimulatePlayerScore();

            foreach (var (playerIndex, oldAndNewScore) in simulatePlayerScore)
            {
                var oldProgress = 1f * oldAndNewScore.Item1 / this.UnityTemplateEventRacingDataController.RacingScoreMax;
                var newProgress = 1f * oldAndNewScore.Item2 / this.UnityTemplateEventRacingDataController.RacingScoreMax;

                this.View.playerSliders[playerIndex].progressSlider.value = oldProgress;
                if (newProgress > oldProgress)
                {
                    this.tweenList.Add(DOTween.To(() => oldProgress,
                            x =>
                            {
                                this.View.playerSliders[playerIndex].progressSlider.value = x;
                            },
                            Mathf.Clamp(newProgress, 0, racingMaxProgression),
                            1f)
                        .SetUpdate(true));
                    this.tweenList.Add(DOTween.To(() => oldAndNewScore.Item1,
                            x =>
                            {
                                this.View.playerSliders[playerIndex].scoreText.text = x.ToString();
                            },
                            oldAndNewScore.Item2,
                            1f)
                        .SetUpdate(true));
                }
            }

            this.View.playerSliders.ForEach(item => item.CheckStatus());

            this.CheckRacingEventComplete();
            return UniTask.CompletedTask;
        }

        protected virtual void CheckRacingEventComplete()
        {
            if (!this.UnityTemplateEventRacingDataController.RacingEventComplete()) return;
            // Do something
        }

        public override void Dispose()
        {
            base.Dispose();

            //Clear tween
            foreach (var tween in this.tweenList) tween.Kill();
            this.tweenList.Clear();
        }
    }
}