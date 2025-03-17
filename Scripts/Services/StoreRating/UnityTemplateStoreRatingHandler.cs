namespace HyperGames.UnityTemplate.UnityTemplate.Services.StoreRating
{
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateStoreRatingHandler
    {
        #region MyRegion

        private readonly IStoreRatingService storeRatingService;

        #endregion

        private const string StoreRatingLocalDataKey = "LD_StoreRating";

        [Preserve]
        public UnityTemplateStoreRatingHandler(IStoreRatingService storeRatingService)
        {
            this.storeRatingService = storeRatingService;
        }

        public void LaunchStoreRating()
        {
            this.storeRatingService.LaunchStoreRating();
            PlayerPrefs.SetString(StoreRatingLocalDataKey, "TRUE");
        }

        public bool IsRated => PlayerPrefs.HasKey(StoreRatingLocalDataKey);
    }
}