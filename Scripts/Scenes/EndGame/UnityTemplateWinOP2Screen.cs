namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.EndGame
{
    using Core.AdsServices;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateWinOP2Screen : BaseEndGameScreenView
    {
        public Button                 btnX2Reward;
        public UnityTemplateCurrencyView currencyView;
    }

    [ScreenInfo(nameof(UnityTemplateWinOP2Screen))]
    public class UnityTemplateWinOp2ScreenPresenter : BaseEndGameScreenPresenter<UnityTemplateWinOP2Screen>
    {
        [Preserve]
        public UnityTemplateWinOp2ScreenPresenter(
            SignalBus                  signalBus,
            ILogService                logger,
            UnityTemplateAdServiceWrapper UnityTemplateAdService,
            UnityTemplateSoundServices    soundServices
        ) : base(signalBus, logger, UnityTemplateAdService, soundServices)
        {
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.btnX2Reward.onClick.AddListener(this.OnX2Reward);
        }

        public override UniTask BindData()
        {
            base.BindData();
            this.UnityTemplateAdService.ShowMREC(AdViewPosition.Centered);
            this.SoundServices.PlaySoundWin();
            this.UnityTemplateAdService.HideBannerAd();
            return UniTask.CompletedTask;
        }

        protected virtual void OnX2Reward()
        {
            this.UnityTemplateAdService.ShowRewardedAd("x2Reward", this.AfterWatchAdsX2Reward);
        }

        protected virtual void AfterWatchAdsX2Reward()
        {
        }

        protected override void OnClickNext()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            this.UnityTemplateAdService.HideMREC(AdViewPosition.Centered);
            this.UnityTemplateAdService.ShowBannerAd();
        }
    }
}