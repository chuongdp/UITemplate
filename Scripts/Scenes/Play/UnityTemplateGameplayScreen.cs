namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Play
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateGameplayScreen : BaseView
    {
        [SerializeField] private Button                 btnHome;
        [SerializeField] private Button                 btnReplay;
        [SerializeField] private UnityTemplateAdsButton    btnSkip;
        [SerializeField] private UnityTemplateCurrencyView currencyView;
        [SerializeField] private TextMeshProUGUI        levelText;

        public Button                 BtnHome      => this.btnHome;
        public Button                 BtnReplay    => this.btnReplay;
        public UnityTemplateAdsButton    BtnSkip      => this.btnSkip;
        public UnityTemplateCurrencyView CurrencyView => this.currencyView;
        public TextMeshProUGUI        LevelText    => this.levelText;
    }

    [ScreenInfo(nameof(UnityTemplateGameplayScreen))]
    public class UnityTemplateGameplayScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateGameplayScreen>
    {
        #region Inject

        protected readonly SceneDirector                     SceneDirector;
        protected readonly IScreenManager                    ScreenManager;
        protected readonly UnityTemplateAdServiceWrapper        adService;
        protected readonly UnityTemplateSoundServices           SoundServices;
        protected readonly UnityTemplateInventoryDataController inventoryDataController;
        protected readonly UnityTemplateLevelDataController     levelDataController;

        [Preserve]
        public UnityTemplateGameplayScreenPresenter(
            SignalBus                         signalBus,
            ILogService                       logger,
            SceneDirector                     sceneDirector,
            IScreenManager                    screenManager,
            UnityTemplateAdServiceWrapper        adService,
            UnityTemplateSoundServices           soundServices,
            UnityTemplateInventoryDataController inventoryDataController,
            UnityTemplateLevelDataController     levelDataController
        ) : base(signalBus, logger)
        {
            this.SceneDirector           = sceneDirector;
            this.ScreenManager           = screenManager;
            this.adService               = adService;
            this.SoundServices           = soundServices;
            this.inventoryDataController = inventoryDataController;
            this.levelDataController     = levelDataController;
        }

        #endregion

        protected virtual string NextSceneToLoad => "1.MainScene";
        protected virtual string AdPlacement     => "skip_level";

        protected override void OnViewReady()
        {
            base.OnViewReady();
            if (this.View.BtnSkip != null) this.View.BtnSkip.OnViewReady(this.adService);

            if (this.View.BtnHome != null) this.View.BtnHome.onClick.AddListener(this.OnOpenHome);

            if (this.View.BtnReplay != null) this.View.BtnReplay.onClick.AddListener(this.OnClickReplay);

            if (this.View.BtnSkip != null) this.View.BtnSkip.onClick.AddListener(this.OnClickSkip);
        }

        public override UniTask BindData()
        {
            if (this.View.BtnSkip != null) this.View.BtnSkip.BindData(this.AdPlacement);

            if (this.View.LevelText != null) this.View.LevelText.text = "Level " + this.levelDataController.GetCurrentLevelData.Level;
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            if (this.View.BtnSkip != null) this.View.BtnSkip.Dispose();
        }

        protected virtual async void OnOpenHome()
        {
            await this.ScreenManager.OpenScreen<UnityTemplateHomeTapToPlayScreenPresenter>();
        }

        protected virtual void OnClickReplay()
        {
        }

        protected virtual void OnClickSkip()
        {
        }

        protected virtual void OpenNextScene()
        {
            this.SceneDirector.LoadSingleSceneAsync(this.NextSceneToLoad);
        }
    }
}