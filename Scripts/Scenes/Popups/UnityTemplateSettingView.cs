namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateSettingPopupView : BaseView
    {
        public UnityTemplateOnOffButton SoundButton;
        public UnityTemplateOnOffButton MusicButton;
        public UnityTemplateOnOffButton VibrationButton;

        public Button ReplayButton;
        public Button HomeButton;
    }

    [PopupInfo(nameof(UnityTemplateSettingPopupView))]
    public class UnityTemplateSettingPopupPresenter : UnityTemplateBasePopupPresenter<UnityTemplateSettingPopupView>
    {
        #region inject

        private readonly UnityTemplateSettingDataController UnityTemplateSettingDataController;

        #endregion

        [Preserve]
        public UnityTemplateSettingPopupPresenter(
            SignalBus                       signalBus,
            ILogService                     logger,
            UnityTemplateSettingDataController UnityTemplateSettingDataController
        ) : base(signalBus, logger)
        {
            this.UnityTemplateSettingDataController = UnityTemplateSettingDataController;
        }

        protected override void OnViewReady()
        {
            this.View.SoundButton.Button.onClick.AddListener(this.OnClickSoundButton);
            this.View.MusicButton.Button.onClick.AddListener(this.OnClickMusicButton);
            this.View.VibrationButton.Button.onClick.AddListener(this.OnVibrationButton);
        }

        private void OnClickSoundButton()
        {
            this.UnityTemplateSettingDataController.SetSoundOnOff();
            this.InitButton();
        }

        private void OnClickMusicButton()
        {
            this.UnityTemplateSettingDataController.SetMusicOnOff();
            this.InitButton();
        }

        private void OnVibrationButton()
        {
            this.UnityTemplateSettingDataController.SetVibrationOnOff();
            this.InitButton();
        }

        private void InitButton()
        {
            this.View.SoundButton.SetOnOff(this.UnityTemplateSettingDataController.IsSoundOn);
            this.View.MusicButton.SetOnOff(this.UnityTemplateSettingDataController.IsMusicOn);
            this.View.VibrationButton.SetOnOff(this.UnityTemplateSettingDataController.IsVibrationOn);
        }

        public override UniTask BindData()
        {
            this.InitButton();
            return UniTask.CompletedTask;
        }
    }
}