namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.Scripts.Models.LocalDatas;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Models;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.Level;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateLevelSelectScreenView : BaseView
    {
        public Button                     HomeButton;
        public UnityTemplateCurrencyView     CoinText;
        public UnityTemplateLevelGridAdapter LevelGridAdapter;
    }

    [ScreenInfo(nameof(UnityTemplateLevelSelectScreenView))]
    public class UnityTemplateLevelSelectScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateLevelSelectScreenView>
    {
        [Preserve]
        public UnityTemplateLevelSelectScreenPresenter(
            SignalBus                         signalBus,
            ILogService                       logger,
            IScreenManager                    screenManager,
            UnityTemplateInventoryDataController UnityTemplateInventoryDataController,
            UnityTemplateLevelDataController     UnityTemplateLevelDataController
        ) : base(signalBus, logger)
        {
            this.UnityTemplateInventoryDataController = UnityTemplateInventoryDataController;
            this.UnityTemplateLevelDataController     = UnityTemplateLevelDataController;
            this.screenManager                     = screenManager;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.HomeButton.onClick.AddListener(this.OnClickHome);
        }

        protected virtual void OnClickHome()
        {
            this.screenManager.OpenScreen<UnityTemplateHomeSimpleScreenPresenter>();
        }

        public override async UniTask BindData()
        {
            var levelList    = this.getLevelList();
            var currentLevel = this.UnityTemplateLevelDataController.GetCurrentLevelData.Level;
            await this.View.LevelGridAdapter.InitItemAdapter(levelList);
            this.View.LevelGridAdapter.SmoothScrollTo(currentLevel, 1);
        }

        private List<LevelData> getLevelList()
        {
            return this.UnityTemplateLevelDataController.GetAllLevels();
        }

        #region inject

        protected readonly IScreenManager                    screenManager;
        private readonly   UnityTemplateInventoryDataController UnityTemplateInventoryDataController;
        private readonly   UnityTemplateLevelDataController     UnityTemplateLevelDataController;

        #endregion
    }
}