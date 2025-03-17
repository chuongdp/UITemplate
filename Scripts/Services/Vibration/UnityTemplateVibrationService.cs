namespace HyperGames.UnityTemplate.UnityTemplate.Services.Vibration
{
    using System;
    using HyperGames.UnityTemplate.UnityTemplate.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using Lofelt.NiceVibrations;
    using UnityEngine.Scripting;

    public class UnityTemplateVibrationService : IVibrationService
    {
        #region inject

        private readonly UnityTemplateSettingDataController UnityTemplateSettingDataController;

        #endregion

        private readonly bool hapticsSupported;

        [Preserve]
        public UnityTemplateVibrationService(UnityTemplateSettingDataController UnityTemplateSettingDataController)
        {
            this.UnityTemplateSettingDataController = UnityTemplateSettingDataController;
            this.hapticsSupported                = DeviceCapabilities.isVersionSupported;
        }

        private HapticPatterns.PresetType GetHapticPatternsPresetType(VibrationPresetType vibrationPresetType)
        {
            return vibrationPresetType switch
            {
                VibrationPresetType.Selection    => HapticPatterns.PresetType.Selection,
                VibrationPresetType.Success      => HapticPatterns.PresetType.Success,
                VibrationPresetType.Warning      => HapticPatterns.PresetType.Warning,
                VibrationPresetType.Failure      => HapticPatterns.PresetType.Failure,
                VibrationPresetType.LightImpact  => HapticPatterns.PresetType.LightImpact,
                VibrationPresetType.MediumImpact => HapticPatterns.PresetType.MediumImpact,
                VibrationPresetType.HeavyImpact  => HapticPatterns.PresetType.HeavyImpact,
                VibrationPresetType.RigidImpact  => HapticPatterns.PresetType.RigidImpact,
                VibrationPresetType.SoftImpact   => HapticPatterns.PresetType.SoftImpact,
                VibrationPresetType.None         => HapticPatterns.PresetType.None,
                _                                => throw new ArgumentOutOfRangeException(nameof(vibrationPresetType), vibrationPresetType, null),
            };
        }

        public void PlayPresetType(VibrationPresetType vibrationPresetType)
        {
            if (!this.UnityTemplateSettingDataController.IsVibrationOn) return;
            HapticPatterns.PlayPreset(this.GetHapticPatternsPresetType(vibrationPresetType));
        }

        public void PlayEmphasis(float amplitude, float frequency)
        {
            if (!this.UnityTemplateSettingDataController.IsVibrationOn) return;
            HapticPatterns.PlayEmphasis(amplitude, frequency);
        }

        public void PlayConstant(float amplitude, float frequency, float duration)
        {
            if (!this.UnityTemplateSettingDataController.IsVibrationOn) return;
            HapticPatterns.PlayConstant(amplitude, frequency, duration);
        }
    }
}