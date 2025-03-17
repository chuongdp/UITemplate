namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.Collection
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Models;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.Collection.Elements;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateCollectionScreenView : BaseView
    {
        public UnityTemplateCurrencyView CoinText;
        public Button                 HomeButton;
        public UnityTemplateOnOffButton  CharactersButton;
        public UnityTemplateOnOffButton  ItemsButton;
        public Button                 WatchAdsButton;
        public ItemCollectionAdapter  ItemCollectionAdapter;
    }

    [ScreenInfo(nameof(UnityTemplateCollectionScreenView))]
    public class UnityTemplateCollectionScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateCollectionScreenView>
    {
        private const string CatCharacter = "Character";
        private const string CatItem      = "Item";

        private readonly List<ItemCollectionItemModel> itemLists = new();

        [Preserve]
        public UnityTemplateCollectionScreenPresenter(
            SignalBus                         signalBus,
            ILogService                       logger,
            IScreenManager                    screenManager,
            UnityTemplateShopBlueprint           shopBlueprint,
            UnityTemplateItemBlueprint           itemBlueprint,
            UnityTemplateInventoryDataController UnityTemplateInventoryDataController
        ) :
            base(signalBus, logger)
        {
            this.screenManager                     = screenManager;
            this.shopBlueprint                     = shopBlueprint;
            this.itemBlueprint                     = itemBlueprint;
            this.UnityTemplateInventoryDataController = UnityTemplateInventoryDataController;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.HomeButton.onClick.AddListener(this.OnClickHome);
            this.View.WatchAdsButton.onClick.AddListener(this.OnClickWatchAds);
            this.View.CharactersButton.Button.onClick.AddListener(this.OnClickCharacters);
            this.View.ItemsButton.Button.onClick.AddListener(this.OnClickItem);
        }

        public override UniTask BindData()
        {
            this.GetItemDataList(this.itemLists);
            this.SelectTabCategory(CatCharacter);
            return UniTask.CompletedTask;
        }

        private void OnNotEnoughMoney()
        {
            // show popup not enough money here
        }

        private void GetItemDataList(List<ItemCollectionItemModel> source)
        {
            source.Clear();
            for (var i = 0; i < this.shopBlueprint.Values.Count; i++)
            {
                var shopRecord = this.shopBlueprint.Values.ElementAt(i);
                var itemRecord = this.itemBlueprint.GetDataById(shopRecord.Id);

                if (!itemRecord.Category.Equals(CatItem)) continue;
                var model = new ItemCollectionItemModel
                {
                    Index                       = i,
                    UnityTemplateItemInventoryData = this.UnityTemplateInventoryDataController.GetItemData(itemRecord.Id),
                    Category                    = CatItem,
                    OnBuy                       = this.OnBuyItem,
                    OnSelected                  = this.OnSelectedItem,
                    OnNotEnoughMoney            = this.OnNotEnoughMoney,
                };
                source.Add(model);
            }
        }

        private void OnBuyItem(ItemCollectionItemModel obj)
        {
            obj.UnityTemplateItemInventoryData.CurrentStatus = UnityTemplateItemData.Status.Owned;
            // this.userData.InventoryData.CurrentSelectItemId.Value = obj.UnityTemplateItemData.BlueprintRecord.Name;
            this.UnityTemplateInventoryDataController.UpdateStatusItemData(obj.UnityTemplateItemInventoryData.ItemBlueprintRecord.Id, UnityTemplateItemData.Status.Owned);
            // update payment coin here

            this.View.ItemCollectionAdapter.Refresh();
        }

        private void OnSelectedItem(ItemCollectionItemModel obj)
        {
            // this.userData.UserPackageData.CurrentSelectItemId.Value = obj.UnityTemplateItemData.BlueprintRecord.Name;
            this.View.ItemCollectionAdapter.Refresh();
        }

        private async void SelectTabCategory(string categoryTab)
        {
            if (categoryTab.Equals(CatItem)) await this.View.ItemCollectionAdapter.InitItemAdapter(this.itemLists);

            // await this.View.CharacterCollectionAdapter.InitItemAdapter(this.characterLists, this.diContainer);
            this.View.ItemCollectionAdapter.gameObject.SetActive(categoryTab.Equals(CatItem));
            // this.View.CharacterCollectionAdapter.gameObject.SetActive(categoryTab.Equals(CatCharacter));
        }

        private void OnClickItem()
        {
            this.SelectTabCategory(CatItem);
            this.ConfigBtnStatus(false, true);
        }

        private void OnClickCharacters()
        {
            this.SelectTabCategory(CatCharacter);
            this.ConfigBtnStatus(true, false);
        }

        private void OnClickHome()
        {
            this.screenManager.OpenScreen<UnityTemplateHomeSimpleScreenPresenter>();
        }

        private void OnClickWatchAds()
        {
        }

        private void ConfigBtnStatus(bool isCharacterActive, bool isItemActive)
        {
            this.View.CharactersButton.SetOnOff(isCharacterActive);
            this.View.ItemsButton.SetOnOff(isItemActive);
        }

        #region Inject

        private readonly IScreenManager                    screenManager;
        private readonly UnityTemplateShopBlueprint           shopBlueprint;
        private readonly UnityTemplateItemBlueprint           itemBlueprint;
        private readonly UnityTemplateInventoryDataController UnityTemplateInventoryDataController;

        #endregion
    }
}