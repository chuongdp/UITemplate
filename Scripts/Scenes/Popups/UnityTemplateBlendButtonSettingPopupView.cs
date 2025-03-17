namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateBlendButtonSettingPopupView : BaseView
    {
        public Button BtnClose;

        public UnityTemplateBlendButton BtnMusic;
        public UnityTemplateBlendButton BtnSound;
        public UnityTemplateBlendButton BtnVibration;
    }

    [PopupInfo(nameof(UnityTemplateBlendButtonSettingPopupView), isCloseWhenTapOutside: false)]
    public class UnityTemplateBlendButtonSettingPopupPresenter : BasePopupPresenter<UnityTemplateBlendButtonSettingPopupView>
    {
        private readonly UnityTemplateSettingDataController UnityTemplateSettingDataController;

        [Preserve]
        public UnityTemplateBlendButtonSettingPopupPresenter(
            SignalBus                       signalBus,
            ILogService                     logger,
            UnityTemplateSettingDataController UnityTemplateSettingDataController
        ) : base(signalBus, logger)
        {
            this.UnityTemplateSettingDataController = UnityTemplateSettingDataController;
        }

        public override UniTask BindData()
        {
            this.View.BtnMusic.Init(this.UnityTemplateSettingDataController.IsMusicOn);
            this.View.BtnSound.Init(this.UnityTemplateSettingDataController.IsSoundOn);
            this.View.BtnVibration.Init(this.UnityTemplateSettingDataController.IsVibrationOn);

            this.View.BtnClose.onClick.AddListener(this.CloseView);

            return UniTask.CompletedTask;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.BtnMusic.Button.onClick.AddListener(this.OnClickMusicButton);
            this.View.BtnSound.Button.onClick.AddListener(this.OnClickSoundButton);
            this.View.BtnVibration.Button.onClick.AddListener(this.OnClickVibrationButton);
        }

        private void OnClickVibrationButton()
        {
            this.UnityTemplateSettingDataController.SetVibrationOnOff();
        }

        private void OnClickSoundButton()
        {
            this.UnityTemplateSettingDataController.SetSoundOnOff();
        }

        private void OnClickMusicButton()
        {
            this.UnityTemplateSettingDataController.SetMusicOnOff();
        }
    }
}