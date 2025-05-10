namespace GameTemplate.UnityTemplate.Interfaces
{
    using GameTemplate.UnityTemplate.Services.Vibration;

    public interface IVibrationService
    {
        void PlayPresetType(VibrationPresetType vibrationPresetType);
        void PlayEmphasis(float                 amplitude, float frequency);
        void PlayConstant(float                 amplitude, float frequency, float duration);
    }
}