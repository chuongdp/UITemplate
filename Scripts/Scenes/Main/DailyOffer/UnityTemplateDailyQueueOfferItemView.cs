#if HYPERGAMES_DAILY_QUEUE_REWARD
namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyOffer
{
    using System;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using R3;
    using HyperGames.UnityTemplate.UnityTemplate.Blueprints;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Scripting;

    public class UnityTemplateDailyQueueOfferItemView : TViewMono
    {
        [SerializeField] private TMP_Text            buttonText;
        [SerializeField] private TMP_Text            itemText;
        [SerializeField] private Image               itemImage;
        [SerializeField] private GameObject          adsIconObj;
        [SerializeField] private GameObject          disableClaimObj;
        [SerializeField] private Button              claimButton;
        [SerializeField] private UnityTemplateAdsButton adsClaimButton;

        public TMP_Text            ButtonText      => this.buttonText;
        public TMP_Text            ItemText        => this.itemText;
        public Image               ItemImage       => this.itemImage;
        public GameObject          AdsIconObj      => this.adsIconObj;
        public GameObject          DisableClaimObj => this.disableClaimObj;
        public Button              ClaimButton     => this.claimButton;
        public UnityTemplateAdsButton AdsClaimButton  => this.adsClaimButton;

        public Action ClickClaim;
        public Action ClickAdsClaim;

        private void Awake()
        {
            this.claimButton.onClick.AddListener(() => this.ClickClaim?.Invoke());
            this.adsClaimButton.onClick.AddListener(() => this.ClickAdsClaim?.Invoke());
        }
    }

    public class UnityTemplateDailyQueueOfferItemModel
    {
        public string OfferId { get; }

        public UnityTemplateDailyQueueOfferItemModel(string offerId) { this.OfferId = offerId; }
    }

    public class UnityTemplateDailyQueueOfferItemPresenter : BaseUIItemPresenter<UnityTemplateDailyQueueOfferItemView, UnityTemplateDailyQueueOfferItemModel>
    {
        #region Inject

        private readonly UnityTemplateAdServiceWrapper              adServiceWrapper;
        private readonly UnityTemplateDailyQueueOfferDataController dailyQueueOfferDataController;

        #endregion

        private RectTransform                       claimRect;
        private IDisposable                         remainTimeDisposable;
        private UnityTemplateDailyQueueOfferItemModel  model;
        private UnityTemplateDailyQueueOfferItemRecord dailyQueueOfferItemRecord;

        private const string AdsPlacement = "Daily_Offer";

        [Preserve]
        public UnityTemplateDailyQueueOfferItemPresenter
        (
            IGameAssets                             gameAssets,
            UnityTemplateAdServiceWrapper              adServiceWrapper,
            UnityTemplateDailyQueueOfferDataController dailyQueueOfferDataController
        )
            : base(gameAssets)
        {
            this.adServiceWrapper = adServiceWrapper;
            this.dailyQueueOfferDataController = dailyQueueOfferDataController;
        }

        public override void OnViewReady()
        {
            base.OnViewReady();
            this.claimRect = this.View.ItemImage.GetComponent<RectTransform>();
            this.View.AdsClaimButton.OnViewReady(this.adServiceWrapper);
        }

        public override void BindData(UnityTemplateDailyQueueOfferItemModel param)
        {
            this.View.ClickClaim = this.OnClickClaim;
            this.View.ClickAdsClaim = this.OnClickAdsClaim;

            this.model = param;
            this.dailyQueueOfferItemRecord = this.dailyQueueOfferDataController.GetCurrentDailyQueueOfferRecord().OfferItems[this.model.OfferId];
            this.View.ItemText.text = $"x{this.dailyQueueOfferItemRecord.Value}";
            this.View.ItemImage.sprite = this.GameAssets.ForceLoadAsset<Sprite>(this.dailyQueueOfferItemRecord.ImageId);
            this.OnUpdateOfferItemByStatus();
            this.BindDataToClaimButton();
            this.BindDataToAdsClaimButton();
        }

        private void BindDataToClaimButton()
        {
            if (this.remainTimeDisposable != null) return;

            this.remainTimeDisposable = Observable.EveryUpdate().Subscribe(_ => this.OnUpdateRemainTimeFreeOffer());
        }

        private void BindDataToAdsClaimButton()
        {
            this.dailyQueueOfferDataController.OnUpdateOfferItem += this.OnUpdateOfferItemByStatus;
            this.View.AdsClaimButton.BindData(AdsPlacement);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.dailyQueueOfferDataController.OnUpdateOfferItem -= this.OnUpdateOfferItemByStatus;

            if (this.remainTimeDisposable == null) return;

            this.remainTimeDisposable?.Dispose();
            this.remainTimeDisposable = null;
        }

        private void OnUpdateOfferItemByStatus()
        {
            if (!this.dailyQueueOfferDataController.TryGetOfferStatusDuringDay(this.model.OfferId, out var status))
            {
                return;
            }

            var isRewardedAds = this.dailyQueueOfferItemRecord.IsRewardedAds;
            this.View.ClaimButton.gameObject.SetActive(status == RewardStatus.Unlocked && !isRewardedAds);
            this.View.AdsClaimButton.gameObject.SetActive(status == RewardStatus.Unlocked && isRewardedAds);
            this.View.AdsIconObj.SetActive(status != RewardStatus.Claimed && isRewardedAds);
            this.View.DisableClaimObj.SetActive(status != RewardStatus.Unlocked);
        }

        private void OnUpdateRemainTimeFreeOffer()
        {
            if (!this.dailyQueueOfferDataController.TryGetOfferStatusDuringDay(this.model.OfferId, out var status))
            {
                return;
            }

            var remainTimeToNextDay = this.dailyQueueOfferDataController.GetRemainTimeToNextDay();
            var remainHours = remainTimeToNextDay.Hours;
            var remainMinutes = remainTimeToNextDay.Minutes;
            var remainSeconds = remainTimeToNextDay.Seconds;
            if (status == RewardStatus.Claimed)
            {
                var textRemainHours = remainHours > 0 ? $"{remainHours}h " : "";
                var textRemainMinutes = $"{remainMinutes}m";
                var textRemainSeconds = remainHours > 0 ? "" : $"{remainSeconds}s";
                this.View.ButtonText.text = $"{textRemainHours}{textRemainMinutes}{textRemainSeconds}";
            }
            else
            {
                this.View.ButtonText.text = "Claim";
            }
        }

        protected void OnClickClaim() { this.OnClaimOfferSucceed(); }

        protected void OnClickAdsClaim() { this.adServiceWrapper.ShowRewardedAd(AdsPlacement, this.OnClaimOfferSucceed, this.OnClaimOfferFailed); }

        protected virtual void OnClaimOfferSucceed()
        {
            var offerData = this.dailyQueueOfferDataController.GetCurrentDailyQueueOfferRecord().OfferItems[this.model.OfferId];
            this.dailyQueueOfferDataController.ClaimOfferItem(offerData, this.claimRect);
        }

        protected virtual void OnClaimOfferFailed() { }
    }
}
#endif