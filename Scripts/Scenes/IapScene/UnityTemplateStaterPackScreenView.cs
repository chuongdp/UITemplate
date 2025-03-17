namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.IapScene
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.Utilities.LoadImage;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services.RewardHandle.AllRewards;
    using ServiceImplementation.IAPServices;
    using HyperGames.UnityTemplate.UnityTemplate.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateStaterPackModel
    {
        public Action<string> OnComplete { get; set; }
    }

    public class UnityTemplateStaterPackScreenView : BaseView
    {
        public Button                      btnClose;
        public Button                      btnBuy;
        public Button                      btnRestore;
        public Button                      btnPolicy;
        public Button                      btnTerms;
        public Image                       imgGift;
        public UnityTemplateStaterPackAdapter adapter;
        public TextMeshProUGUI             txtPrice;
    }

    [ScreenInfo(nameof(UnityTemplateStaterPackScreenView))]
    public class UnityTemplateStartPackScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateStaterPackScreenView, UnityTemplateStaterPackModel>
    {
        private readonly UnityTemplateAdServiceWrapper   adService;
        private readonly UnityTemplateShopPackBlueprint  unityTemplateShopPackBlueprint;
        private readonly UnityTemplateIapServices        UnityTemplateIapServices;
        private readonly UnityTemplateMiscParamBlueprint unityTemplateMiscParamBlueprint;
        private readonly LoadImageHelper              loadImageHelper;
        private readonly IIapServices                 iapServices;

        [Preserve]
        public UnityTemplateStartPackScreenPresenter(
            SignalBus                    signalBus,
            ILogService                  logger,
            UnityTemplateAdServiceWrapper   adService,
            UnityTemplateShopPackBlueprint  unityTemplateShopPackBlueprint,
            UnityTemplateIapServices        UnityTemplateIapServices,
            UnityTemplateMiscParamBlueprint unityTemplateMiscParamBlueprint,
            LoadImageHelper              loadImageHelper,
            IIapServices                 iapServices
        ) : base(signalBus, logger)
        {
            this.adService                    = adService;
            this.unityTemplateShopPackBlueprint  = unityTemplateShopPackBlueprint;
            this.UnityTemplateIapServices        = UnityTemplateIapServices;
            this.unityTemplateMiscParamBlueprint = unityTemplateMiscParamBlueprint;
            this.loadImageHelper              = loadImageHelper;
            this.iapServices                  = iapServices;
        }

        private string iapPack = "";

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.btnBuy.onClick.AddListener(this.OnBuyClick);
            this.View.btnRestore.onClick.AddListener(this.OnRestore);
            this.View.btnClose.onClick.AddListener(this.CloseView);
            this.View.btnTerms.onClick.AddListener(this.OnOpenTerm);
            this.View.btnPolicy.onClick.AddListener(this.OnOpenPolicy);
            this.View.btnRestore.gameObject.SetActive(false);
            #if UNITY_IOS
            this.View.btnRestore.gameObject.SetActive(true);
            #endif
        }

        private void OnOpenPolicy()
        {
            if (!string.IsNullOrEmpty(this.unityTemplateMiscParamBlueprint.PolicyAddress)) Application.OpenURL(this.unityTemplateMiscParamBlueprint.PolicyAddress);
        }

        private void OnOpenTerm()
        {
            if (!string.IsNullOrEmpty(this.unityTemplateMiscParamBlueprint.TermsAddress)) Application.OpenURL(this.unityTemplateMiscParamBlueprint.TermsAddress);
        }

        private void OnRestore()
        {
            this.UnityTemplateIapServices.RestorePurchase(this.CloseView);
        }

        private void OnBuyClick()
        {
            this.UnityTemplateIapServices.BuyProduct(this.View.btnBuy.gameObject,
                this.iapPack,
                (x) =>
                {
                    this.CloseView();
                    this.Model.OnComplete?.Invoke(x);
                });
        }

        public override async UniTask BindData(UnityTemplateStaterPackModel screenModel)
        {
            var starterPacks = this.unityTemplateShopPackBlueprint.GetPack().Where(x => x.RewardIdToRewardDatas.Count > 1).ToList();
            this.iapPack = starterPacks.First(packRecord => packRecord.RewardIdToRewardDatas.ContainsKey(UnityTemplateRemoveAdRewardExecutorBase.REWARD_ID) != this.adService.IsRemovedAds).Id;

            this.View.txtPrice.text = $"Special Offer: Only {this.iapServices.GetPriceById(this.iapPack, this.unityTemplateShopPackBlueprint.GetDataById(this.iapPack).DefaultPrice)}";

            if (this.unityTemplateShopPackBlueprint.TryGetValue(this.iapPack, out var shopPackRecord))
            {
                if (!shopPackRecord.ImageAddress.IsNullOrEmpty()) this.View.imgGift.sprite = await this.loadImageHelper.LoadLocalSprite(shopPackRecord.ImageAddress);

                var model = new List<UnityTemplateStartPackItemModel>();

                foreach (var rewardBlueprintData in shopPackRecord.RewardIdToRewardDatas)
                {
                    model.Add(new()
                    {
                        IconAddress = rewardBlueprintData.Value.RewardIcon,
                        Value       = rewardBlueprintData.Value.RewardContent,
                    });
                }

                this.View.adapter.InitItemAdapter(model).Forget();
            }
        }
    }
}