namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.EndGame
{
    using Core.AdsServices;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateLoseOP2Screen : BaseEndGameScreenView
    {
        public Button btnContinue;
    }

    [ScreenInfo(nameof(UnityTemplateLoseOP2Screen))]
    public class UnityTemplateLoseOp2Presenter : BaseEndGameScreenPresenter<UnityTemplateLoseOP2Screen>
    {
        [Preserve]
        public UnityTemplateLoseOp2Presenter(SignalBus signalBus, ILogService logger, UnityTemplateAdServiceWrapper UnityTemplateAdService, UnityTemplateSoundServices soundServices) : base(signalBus, logger, UnityTemplateAdService, soundServices)
        {
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.btnContinue.onClick.AddListener(this.OnContinue);
        }

        public override UniTask BindData()
        {
            base.BindData();
            this.UnityTemplateAdService.ShowMREC(AdViewPosition.Centered);
            this.SoundServices.PlaySoundLose();
            return UniTask.CompletedTask;
        }

        protected virtual void OnContinue()
        {
            this.UnityTemplateAdService.ShowRewardedAd("Lose_Continue", this.AfterWatchAd);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.UnityTemplateAdService.HideMREC(AdViewPosition.Centered);
        }

        protected virtual void AfterWatchAd()
        {
        }

        protected override void OnClickNext()
        {
        }
    }
}