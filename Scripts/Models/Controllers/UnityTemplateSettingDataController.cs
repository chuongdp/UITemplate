namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using System;
    using GameFoundation.Scripts.Models;
    using UnityEngine.Scripting;

    public class UnityTemplateSettingDataController : IUnityTemplateControllerData
    {
        #region Inject

        private readonly UnityTemplateUserSettingData UnityTemplateUserSettingData;
        private readonly SoundSetting              soundSetting;

        #endregion

        public bool IsSoundOn => this.soundSetting.SoundValue.Value > 0;

        public bool IsMusicOn     => this.soundSetting.MusicValue.Value > 0;
        public bool IsVibrationOn => this.UnityTemplateUserSettingData.IsVibrationEnable;

        public bool IsFlashLightOn => this.UnityTemplateUserSettingData.IsFlashLightEnable;

        public float MusicValue => this.soundSetting.MusicValue.Value;

        public float SoundValue => this.soundSetting.SoundValue.Value;

        [Preserve]
        public UnityTemplateSettingDataController(UnityTemplateUserSettingData UnityTemplateUserSettingData, SoundSetting soundSetting)
        {
            this.UnityTemplateUserSettingData = UnityTemplateUserSettingData;
            this.soundSetting              = soundSetting;
        }

        public void SetSoundOnOff()
        {
            this.soundSetting.SoundValue.Value = this.IsSoundOn ? 0 : 1;
        }

        public void SetMusicOnOff()
        {
            this.soundSetting.MusicValue.Value = this.IsMusicOn ? 0 : 1;
        }

        public void SetMusicValue(float value)
        {
            this.soundSetting.MusicValue.Value = Math.Clamp(value, 0, 1);
        }

        public void SetSoundValue(float value)
        {
            this.soundSetting.SoundValue.Value = Math.Clamp(value, 0, 1);
        }

        public void SetVibrationOnOff()
        {
            this.UnityTemplateUserSettingData.IsVibrationEnable = !this.UnityTemplateUserSettingData.IsVibrationEnable;
        }

        public void SetFlashLightOnOff()
        {
            this.UnityTemplateUserSettingData.IsFlashLightEnable = !this.UnityTemplateUserSettingData.IsFlashLightEnable;
        }
    }
}