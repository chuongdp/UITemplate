namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.FeaturesConfig
{
    using GameFoundation.DI;
    using ServiceImplementation.FireBaseRemoteConfig;
    using UnityEngine.Scripting;

    public class UnityTemplateFeatureConfig : IInitializable
    {
        private const string IsDailyRewardEnableKey = "IsDailyRewardEnable";
        private const string IsLeaderboardEnableKey = "IsLeaderboardEnable";
        private const string IsLuckySpinEnableKey   = "IsLuckySpinEnable";
        private const string IsChestRoomEnableKey   = "IsChestRoomEnable";
        private const string IsIAPEnableKey         = "IsIAPEnable";
        private const string IsRemoveAdsEnableKey   = "IsRemoveAdsEnable";
        private const string IsSuggestionEnableKey  = "IsSuggestionEnable";
        private const string IsBuildingEnableKey    = "IsBuildingEnable";
        private const string IsVFXEnableKey         = "IsVFXEnable";
        private const string IsSFXEnableKey         = "IsSFXEnable";

        private readonly IRemoteConfig UnityTemplateRemoteConfig;

        [Preserve]
        public UnityTemplateFeatureConfig(IRemoteConfig UnityTemplateRemoteConfig)
        {
            this.UnityTemplateRemoteConfig = UnityTemplateRemoteConfig;
        }

        public bool IsDailyRewardEnable { get; set; } = true;
        public bool IsLeaderboardEnable { get; set; } = true;
        public bool IsLuckySpinEnable   { get; set; } = true;
        public bool IsChestRoomEnable   { get; set; } = true;
        public bool IsIAPEnable         { get; set; } = true;
        public bool IsRemoveAdsEnable   { get; set; } = true;
        public bool IsSuggestionEnable  { get; set; } = true;
        public bool IsBuildingEnable    { get; set; } = true;
        public bool IsVFXEnable         { get; set; } = true;
        public bool IsSFXEnable         { get; set; } = true;

        public void Initialize()
        {
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsDailyRewardEnableKey, remoteValue => this.IsDailyRewardEnable = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsLeaderboardEnableKey, remoteValue => this.IsLeaderboardEnable = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsLuckySpinEnableKey, remoteValue => this.IsLuckySpinEnable     = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsChestRoomEnableKey, remoteValue => this.IsChestRoomEnable     = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsIAPEnableKey, remoteValue => this.IsIAPEnable                 = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsRemoveAdsEnableKey, remoteValue => this.IsRemoveAdsEnable     = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsSuggestionEnableKey, remoteValue => this.IsSuggestionEnable   = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsBuildingEnableKey, remoteValue => this.IsBuildingEnable       = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsVFXEnableKey, remoteValue => this.IsVFXEnable                 = remoteValue, true);
            this.UnityTemplateRemoteConfig.GetRemoteConfigBoolValueAsync(IsSFXEnableKey, remoteValue => this.IsSFXEnable                 = remoteValue, true);
        }
    }
}