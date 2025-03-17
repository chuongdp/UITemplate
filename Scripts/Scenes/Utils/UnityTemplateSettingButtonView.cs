namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Popups;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnityTemplateSettingButtonView : MonoBehaviour
    {
        public UnityTemplateOnOffButton MusicButton;
        public UnityTemplateOnOffButton SoundButton;
        public UnityTemplateOnOffButton VibrateButton;

        [SerializeField] private Button SettingButton;
        [SerializeField] private bool   IsDropdown;

        /// <summary>
        ///     Dropdown animation
        /// </summary>
        [SerializeField]
        private RectTransform BG;

        [SerializeField] private List<RectTransform> ButtonList;

        private bool IsDropped;

        private List<RectTransform>             reverseButtonList;
        private IScreenManager                  screenManager;
        private UnityTemplateSettingDataController UnityTemplateSettingDataController;

        private void Awake()
        {
            var container = this.GetCurrentContainer();
            this.screenManager                   = container.Resolve<IScreenManager>();
            this.UnityTemplateSettingDataController = container.Resolve<UnityTemplateSettingDataController>();

            this.SettingButton.onClick.AddListener(this.OnClick);
            this.MusicButton.Button.onClick.AddListener(this.OnClickMusicButton);
            this.SoundButton.Button.onClick.AddListener(this.OnClickSoundButton);
            this.VibrateButton.Button.onClick.AddListener(this.OnVibrationButton);

            this.reverseButtonList = new(this.ButtonList);
            this.reverseButtonList.Reverse();

            this.InitDropdown();
            this.InitButton();
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
            this.SoundButton.SetOnOff(this.UnityTemplateSettingDataController.IsSoundOn);
            this.MusicButton.SetOnOff(this.UnityTemplateSettingDataController.IsMusicOn);
            this.VibrateButton.SetOnOff(this.UnityTemplateSettingDataController.IsVibrationOn);
        }

        private void InitDropdown()
        {
            this.IsDropped     = false;
            this.BG.localScale = new(1, 0, 1);
            foreach (var rectTransform in this.ButtonList) rectTransform.localScale = Vector3.zero;
        }

        private async void OnClick()
        {
            if (this.IsDropdown)
            {
                //TODO need to to animation here
                const float duration          = 0.15f;
                const float endValue          = 1f;
                const float droppingTime      = 0.4f;
                const int   millisecondsDelay = 100;

                if (this.IsDropped)
                {
                    this.BG.DOScaleY(0, droppingTime).SetEase(Ease.InBack);
                    foreach (var buttonTransform in this.reverseButtonList)
                    {
                        buttonTransform.DOScale(0, duration).SetEase(Ease.InBack);
                        await UniTask.Delay(millisecondsDelay);
                    }
                }
                else
                {
                    this.InitDropdown();
                    this.BG.DOScaleY(1, droppingTime).SetEase(Ease.OutBack);
                    foreach (var buttonTransform in this.ButtonList)
                    {
                        await UniTask.Delay(millisecondsDelay);
                        buttonTransform.DOScale(endValue, duration).SetEase(Ease.OutBounce);
                    }
                }

                this.IsDropped = !this.IsDropped;
            }
            else
            {
                _ = this.screenManager.OpenScreen<UnityTemplateSettingPopupPresenter>();
            }
        }
    }
}