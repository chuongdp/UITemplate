#if HYPERGAMES_DAILY_QUEUE_REWARD
namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyOffer
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateDailyQueueOfferPopupView : BaseView
    {
        [SerializeField] private UnityTemplateCurrencyView           currencyView;
        [SerializeField] private Button                           closeButton;
        [SerializeField] private UnityTemplateDailyQueueOfferAdapter rewardedAdsAdapter;

        public UnityTemplateCurrencyView           CurrencyView       => this.currencyView;
        public Button                           CloseButton        => this.closeButton;
        public UnityTemplateDailyQueueOfferAdapter RewardedAdsAdapter => this.rewardedAdsAdapter;
    }

    [PopupInfo(nameof(UnityTemplateDailyQueueOfferPopupView), true, false, true)]
    public class UnityTemplateDailyQueueOfferPopupPresenter : UnityTemplateBasePopupPresenter<UnityTemplateDailyQueueOfferPopupView>
    {
        #region Inject

        private readonly UnityTemplateDailyQueueOfferDataController dailyOfferDataController;

        #endregion

        private List<UnityTemplateDailyQueueOfferItemModel> listModel;

        [Preserve]
        public UnityTemplateDailyQueueOfferPopupPresenter(
            SignalBus                               signalBus,
            ILogService                             logger,
            UnityTemplateDailyQueueOfferDataController dailyOfferDataController
        ) : base(signalBus, logger)
        {
            this.dailyOfferDataController = dailyOfferDataController;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CloseButton.onClick.AddListener(this.CloseView);
        }

        public override UniTask BindData()
        {
            this.dailyOfferDataController.CheckOfferStatus();

            this.InitOrRefreshRewardAdapter().Forget();

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.View.RewardedAdsAdapter.GetPresenters().ForEach(presenter => presenter.Dispose());
        }

        private UniTask InitOrRefreshRewardAdapter()
        {
            this.listModel ??= this.dailyOfferDataController.GetCurrentDailyQueueOfferRecord()
                .OfferItems.Values
                .Select(item => new UnityTemplateDailyQueueOfferItemModel(item.OfferId))
                .ToList();

            return this.View.RewardedAdsAdapter.InitItemAdapter(this.listModel);
        }
    }
}
#endif