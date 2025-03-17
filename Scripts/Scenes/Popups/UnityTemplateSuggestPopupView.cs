namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateSuggestPopupView : BaseView
    {
        [SerializeField] private TMP_Text textItemName;

        [SerializeField] private Image imageItemIcon;

        [SerializeField] private Button buttonClaim;

        [SerializeField] private Button buttonClose;

        public string ItemName { get => this.textItemName.text; set => this.textItemName.text = value; }

        public Sprite ItemIcon { get => this.imageItemIcon.sprite; set => this.imageItemIcon.sprite = value; }

        public Button ButtonClaim => this.buttonClaim;

        public Button ButtonClose => this.buttonClose;
    }

    public class UnityTemplateSuggestPopupModel
    {
        public string ItemID   { get; set; }
        public string ItemIcon { get; set; }
        public string ItemName { get; set; }
    }

    [PopupInfo(nameof(UnityTemplateSuggestPopupView))]
    public class UnityTemplateSuggestPopupPresenter : UnityTemplateBasePopupPresenter<UnityTemplateSuggestPopupView, UnityTemplateSuggestPopupModel>
    {
        #region Inject

        private readonly IGameAssets                gameAssets;
        private readonly UnityTemplateAdServiceWrapper UnityTemplateAdServiceWrapper;

        [Preserve]
        public UnityTemplateSuggestPopupPresenter(
            SignalBus                  signalBus,
            ILogService                logger,
            IGameAssets                gameAssets,
            UnityTemplateAdServiceWrapper UnityTemplateAdServiceWrapper
        )
            : base(signalBus, logger)
        {
            this.gameAssets                 = gameAssets;
            this.UnityTemplateAdServiceWrapper = UnityTemplateAdServiceWrapper;
        }

        #endregion

        protected UnityTemplateSuggestPopupModel PopupModel { get; private set; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.ButtonClaim.onClick.AddListener(this.OnClaimButtonClicked);
            this.View.ButtonClose.onClick.AddListener(this.OnCloseButtonClicked);
        }

        protected virtual void OnCloseButtonClicked()
        {
            this.CloseView();
        }

        protected virtual void OnClaimButtonClicked()
        {
            this.UnityTemplateAdServiceWrapper.ShowRewardedAd("Suggest", this.OnClaimSuccess);
        }

        protected virtual void OnClaimSuccess()
        {
        }

        public override async UniTask BindData(UnityTemplateSuggestPopupModel popupModel)
        {
            this.PopupModel = popupModel;
            // Bind ItemName
            this.View.ItemName = popupModel.ItemName;

            // Load sprite from addressable using IGameAssets
            var spriteIcon = await this.gameAssets.LoadAssetAsync<Sprite>(popupModel.ItemIcon).Task;
            this.View.ItemIcon = spriteIcon;
        }
    }
}