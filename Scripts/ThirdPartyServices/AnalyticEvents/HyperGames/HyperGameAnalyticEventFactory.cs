namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices;
    using Core.AnalyticServices.CommonEvents;
    using Core.AnalyticServices.Data;
    using GameFoundation.Signals;
    using global::HyperGames.UnityTemplate.Scripts.Services;
    using global::HyperGames.UnityTemplate.Scripts.Models.LocalDatas;
    using global::HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using UnityEngine.Scripting;

    public class HyperAnalyticEventFactory : BaseAnalyticEventFactory
    {
        #region inject

        private readonly IInternetService                 internetService;
        private readonly UnityTemplateLevelDataController unityTemplateLevelDataController;

        [Preserve]
        public HyperAnalyticEventFactory(SignalBus signalBus, IAnalyticServices analyticServices, IInternetService internetService, UnityTemplateLevelDataController unityTemplateLevelDataController) : base(
            signalBus, analyticServices)
        {
            this.internetService               = internetService;
            this.unityTemplateLevelDataController = unityTemplateLevelDataController;
        }

        #endregion

        public override IEvent InterstitialShow(int level, string place) { return new ShowInterstitialAds(this.internetService.IsInternetAvailable, place); }

        public override IEvent InterstitialShowCompleted(int level, string place) { return new InterstitialAdsSuccess(place); }

        public override IEvent RewardedVideoEligible(string place) => new AdsRewardEligible(place);

        public override IEvent RewardedVideoOffer(string place) { return new AdsRewardOffer(place); }

        public override IEvent RewardedVideoDownloaded(string place, long loadingMilis) { return new AdsRewardedDownloaded(place, loadingMilis); }

        public override IEvent RewardedVideoCalled(string place) { return new AdsRewardedCalled(); }

        public override IEvent RewardedVideoShow(int level, string place) { return new ShowRewardedAds(this.internetService.IsInternetAvailable, place); }

        public override IEvent RewardedVideoShowCompleted(int level, string place, bool isRewarded) { return new RewardedAdsSuccess(place, isRewarded ? "success" : "skip"); }

        public override IEvent LevelLose(int level, int timeSpent, int loseCount) { return new LevelFailed(level, timeSpent); }

        public override IEvent LevelStart(int level, int gold) { return new LevelStart(level, this.unityTemplateLevelDataController.GetLevelData(level).LevelStatus == LevelData.Status.Passed); }

        public override IEvent LevelWin(int level, int timeSpent, int winCount) { return new LevelPassed(level, timeSpent); }

        public override IEvent LevelSkipped(int level, int timeSpent) { return new LevelSkipped(level, timeSpent); }

        public override void ForceUpdateAllProperties() { }

        public override string LevelMaxProperty             => "level_max";
        public override string LastLevelProperty            => "last_level";
        public override string LastAdsPlacementProperty     => "last_placement";
        public override string TotalInterstitialAdsProperty => "total_interstitial_ads";
        public override string TotalRewardedAdsProperty     => "total_rewarded_ads";

        public override AnalyticsEventCustomizationConfig AppsFlyerAnalyticsEventCustomizationConfig { get; set; } = new()
        {
            IgnoreEvents = new()
            {
                typeof(GameStarted),
                typeof(AdInterClick),
                typeof(AdInterFail),
                typeof(AdsRewardFail),
                typeof(AdsRewardOffer),
            },
            CustomEventKeys = new()
            {
                { nameof(BannerShown), "af_banner_shown" },
                { nameof(LevelComplete), "af_level_achieved" },
                { nameof(AdInterLoad), "af_inters_api_called" },
                { nameof(AdInterShow), "af_inters_displayed" },
                { nameof(AdInterDownloaded), "af_inters_ad_eligible" },
                { nameof(AdsRewardClick), "af_rewarded_ad_eligible" },
                { nameof(AdsRewardedDownloaded), "af_rewarded_api_called" },
                { nameof(AdsRewardShow), "af_rewarded_displayed" },
                { nameof(AdsRewardComplete), "af_rewarded_ad_completed" },
            }
        };

        public override AnalyticsEventCustomizationConfig FireBaseAnalyticsEventCustomizationConfig { get; set; } = new()
        {
            CustomEventKeys = new()
            {
                { nameof(BaseAnalyticEventFactory.AppOpenFullScreenContentOpened), "aoa_show_success" },
                { nameof(ShowInterstitialAds), "inter_show_success" },
                { nameof(RewardedAdsSuccess), "rewarded_show_success" },
            }
        };
    }
}