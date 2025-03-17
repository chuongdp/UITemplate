namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.EndGame
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using UnityEngine.UI;

    public abstract class BaseEndGameScreenView : BaseView
    {
        public Button btnNext;
    }

    public abstract class BaseEndGameScreenPresenter<TView> : UnityTemplateBaseScreenPresenter<TView> where TView : BaseEndGameScreenView
    {
        protected readonly UnityTemplateSoundServices    SoundServices;
        protected readonly UnityTemplateAdServiceWrapper UnityTemplateAdService;

        protected BaseEndGameScreenPresenter(
            SignalBus                  signalBus,
            ILogService                logger,
            UnityTemplateAdServiceWrapper UnityTemplateAdService,
            UnityTemplateSoundServices    soundServices
        ) : base(signalBus, logger)
        {
            this.UnityTemplateAdService = UnityTemplateAdService;
            this.SoundServices       = soundServices;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.btnNext.onClick.AddListener(this.OnClickNext);
        }

        public override UniTask BindData()
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnClickNext()
        {
        }
    }
}