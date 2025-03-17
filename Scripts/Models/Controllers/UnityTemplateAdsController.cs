namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using HyperGames.UnityTemplate.Scripts.Models.LocalDatas;
    using UnityEngine.Scripting;

    public class UnityTemplateAdsController : IUnityTemplateControllerData
    {
        private readonly UnityTemplateAdsData UnityTemplateAdsData;

        [Preserve]
        public UnityTemplateAdsController(UnityTemplateAdsData UnityTemplateAdsData)
        {
            this.UnityTemplateAdsData = UnityTemplateAdsData;
        }

        public int WatchInterstitialAds => this.UnityTemplateAdsData.WatchedInterstitialAds;
        public int WatchRewardedAds     => this.UnityTemplateAdsData.WatchedRewardedAds;
        public int WatchedAdsCount      => this.UnityTemplateAdsData.WatchedInterstitialAds + this.UnityTemplateAdsData.WatchedRewardedAds;

        public void UpdateWatchedInterstitialAds()
        {
            this.UnityTemplateAdsData.WatchedInterstitialAds++;
        }

        public void UpdateWatchedRewardedAds()
        {
            this.UnityTemplateAdsData.WatchedRewardedAds++;
        }
    }
}