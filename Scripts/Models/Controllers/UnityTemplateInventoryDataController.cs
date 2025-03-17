namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlueprintFlow.Signals;
    using Cysharp.Threading.Tasks;
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Signals;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateInventoryDataController : IUnityTemplateControllerData, IInitializable
    {
        #region Inject

        private readonly SignalBus                           signalBus;
        private readonly IScreenManager                      screenManager;
        private readonly UnityTemplateInventoryData             UnityTemplateInventoryData;
        private readonly UnityTemplateFlyingAnimationController UnityTemplateFlyingAnimationController;
        private readonly UnityTemplateCurrencyBlueprint         unityTemplateCurrencyBlueprint;
        private readonly UnityTemplateShopBlueprint             unityTemplateShopBlueprint;
        private readonly UnityTemplateItemBlueprint             unityTemplateItemBlueprint;
        private readonly IAudioService                       audioService;

        #endregion

        public const string DefaultSoftCurrencyID              = "Coin";
        public const string DefaultChestRoomKeyCurrencyID      = "ChestRoomKey";
        public const string DefaultLuckySpinFreeTurnCurrencyID = "LuckySpinFreeTurn";

        [Preserve]
        public UnityTemplateInventoryDataController(
            UnityTemplateInventoryData             UnityTemplateInventoryData,
            UnityTemplateFlyingAnimationController UnityTemplateFlyingAnimationController,
            UnityTemplateCurrencyBlueprint         unityTemplateCurrencyBlueprint,
            UnityTemplateShopBlueprint             unityTemplateShopBlueprint,
            SignalBus                           signalBus,
            UnityTemplateItemBlueprint             unityTemplateItemBlueprint,
            IScreenManager                      screenManager,
            IAudioService                       audioService
        )
        {
            this.UnityTemplateInventoryData             = UnityTemplateInventoryData;
            this.UnityTemplateFlyingAnimationController = UnityTemplateFlyingAnimationController;
            this.unityTemplateCurrencyBlueprint         = unityTemplateCurrencyBlueprint;
            this.unityTemplateShopBlueprint             = unityTemplateShopBlueprint;
            this.signalBus                           = signalBus;
            this.unityTemplateItemBlueprint             = unityTemplateItemBlueprint;
            this.screenManager                       = screenManager;
            this.audioService                        = audioService;
        }

        public List<UnityTemplateItemData> GetDefaultItemByCategory(string category)
        {
            return this.UnityTemplateInventoryData.IDToItemData.Values.Where(itemData =>
                itemData.ItemBlueprintRecord.Category == category && itemData.ItemBlueprintRecord.IsDefaultItem
            ).ToList();
        }

        public Dictionary<string, List<UnityTemplateItemData>> GetDefaultItemWithCategory()
        {
            return this.UnityTemplateInventoryData.IDToItemData.Values
                .GroupBy(itemData => itemData.ItemBlueprintRecord.Category)
                .ToDictionary(
                    group => group.Key,
                    group => group.Where(itemData => itemData.ItemBlueprintRecord.IsDefaultItem).ToList()
                );
        }

        public string GetTempCurrencyKey(string currency)
        {
            return $"Temp_{currency}";
        }

        public void ApplyTempCurrency(string currency)
        {
            this.UpdateCurrency(this.GetCurrencyValue(this.GetTempCurrencyKey(currency)), currency);
        }

        public void ResetTempCurrency(string currency)
        {
            this.UpdateCurrency(this.GetCurrencyValue(currency), this.GetTempCurrencyKey(currency));
        }

        public string GetCurrentItemSelected(string category)
        {
            return this.UnityTemplateInventoryData.CategoryToChosenItem.TryGetValue(category, out var currentId) ? currentId : null;
        }

        public void UpdateCurrentSelectedItem(string category, string id)
        {
            if (this.UnityTemplateInventoryData.CategoryToChosenItem.TryGetValue(category, out var currentId))
                this.UnityTemplateInventoryData.CategoryToChosenItem[category] = id;
            else
                this.UnityTemplateInventoryData.CategoryToChosenItem.Add(category, id);
        }

        public int GetCurrencyValue(string id = DefaultSoftCurrencyID)
        {
            return this.UnityTemplateInventoryData.IDToCurrencyData
                .GetOrAdd(id, () => new(id, 0, this.unityTemplateCurrencyBlueprint.GetDataById(id).Max)).Value;
        }

        public UnityTemplateCurrencyData GetCurrencyData(string id = DefaultSoftCurrencyID)
        {
            return this.UnityTemplateInventoryData.IDToCurrencyData.GetOrAdd(id, () => new(id, 0, this.unityTemplateCurrencyBlueprint.GetDataById(id).Max));
        }

        public bool IsCurrencyFull(string id)
        {
            return this.GetCurrencyValue(id) >= this.unityTemplateCurrencyBlueprint.GetDataById(id).Max;
        }

        public bool HasItem(string id)
        {
            return this.UnityTemplateInventoryData.IDToItemData.ContainsKey(id);
        }

        public bool TryGetItemData(string id, out UnityTemplateItemData itemData)
        {
            return this.UnityTemplateInventoryData.IDToItemData.TryGetValue(id, out itemData);
        }

        public UnityTemplateItemData GetItemData(string id, UnityTemplateItemData.Status defaultStatusWhenCreateNew = UnityTemplateItemData.Status.Locked)
        {
            var itemRecord = this.unityTemplateItemBlueprint.GetDataById(id);
            var shopRecord = this.unityTemplateShopBlueprint.GetDataById(id);
            var item       = this.UnityTemplateInventoryData.IDToItemData.GetOrAdd(id, () => new(id, shopRecord, itemRecord, defaultStatusWhenCreateNew));
            item.ShopBlueprintRecord = shopRecord;
            item.ItemBlueprintRecord = itemRecord;
            return item;
        }

        public void SetOwnedItemData(UnityTemplateItemData itemData, bool isSelected = false)
        {
            itemData.CurrentStatus = UnityTemplateItemData.Status.Owned;
            this.AddItemData(itemData);

            if (!isSelected) return;
            this.UpdateCurrentSelectedItem(itemData.ItemBlueprintRecord.Category, itemData.Id);
        }

        public void AddItemData(UnityTemplateItemData itemData)
        {
            if (this.UnityTemplateInventoryData.IDToItemData.TryGetValue(itemData.Id, out var data))
            {
                if (data != itemData)
                    this.UnityTemplateInventoryData.IDToItemData.Remove(itemData.Id);
                else
                    return;
            }

            this.UnityTemplateInventoryData.IDToItemData.Add(itemData.Id, itemData);
        }

        public void PayCurrency(Dictionary<string, int> currency, int time = 1)
        {
            foreach (var (currencyKey, currencyValue) in currency) this.AddCurrency(-currencyValue * time, currencyKey).Forget();
        }

        /// <summary>
        /// minAnimAmount and maxAnimAmount is range amount of currency object that will be animated
        /// </summary>
        public async UniTask<bool> AddCurrency(
            int           addingValue,
            string        id                         = DefaultSoftCurrencyID,
            RectTransform startAnimationRect         = null,
            string        claimSoundKey              = null,
            int           minAnimAmount              = 6,
            int           maxAnimAmount              = 10,
            float         timeAnimAnim               = 1f,
            float         flyPunchPositionAnimFactor = 0.3f
        )
        {
            var lastValue = this.GetCurrencyValue(id);

            var resultValue = lastValue + addingValue;
            if (resultValue < 0)
            {
                this.signalBus.Fire(new OnNotEnoughCurrencySignal(id));
                return false;
            }

            var currencyWithCap = this.SetCurrencyWithCap(resultValue, id);
            var amount          = currencyWithCap - lastValue;
            this.signalBus.Fire(new OnUpdateCurrencySignal(id, amount, currencyWithCap));

            if (startAnimationRect != null)
            {
                var flyingObject = this.unityTemplateCurrencyBlueprint.GetDataById(id).FlyingObject;
                var currencyView = this.screenManager.RootUICanvas.GetComponentsInChildren<UnityTemplateCurrencyView>().FirstOrDefault(viewTarget => viewTarget.CurrencyKey.Equals(id));
                if (currencyView != null)
                {
                    if (!string.IsNullOrEmpty(claimSoundKey)) this.audioService.PlaySound(claimSoundKey);
                    await this.UnityTemplateFlyingAnimationController.PlayAnimation<UnityTemplateCurrencyView>(startAnimationRect,
                        minAnimAmount,
                        maxAnimAmount,
                        timeAnimAnim,
                        currencyView.CurrencyIcon.transform as RectTransform,
                        flyingObject,
                        flyPunchPositionAnimFactor);

                    lastValue = this.GetCurrencyValue(id); // get last value after animation because it can be changed by other animation
                    this.signalBus.Fire(new OnFinishCurrencyAnimationSignal(id, amount, currencyWithCap));
                }
            }
            else
                // if there is no animation, just update the currency
            {
                this.signalBus.Fire(new OnFinishCurrencyAnimationSignal(id, amount, currencyWithCap));
            }

            return true;
        }

        public void UpdateCurrency(int finalValue, string id = DefaultSoftCurrencyID)
        {
            var lastValue = this.GetCurrencyValue(id);

            var currencyWithCap = this.SetCurrencyWithCap(finalValue, id);
            this.signalBus.Fire(new OnUpdateCurrencySignal(id, currencyWithCap - lastValue, currencyWithCap));
        }

        private int SetCurrencyWithCap(int value, string id)
        {
            var UnityTemplateCurrencyData = this.UnityTemplateInventoryData.IDToCurrencyData[id];
            UnityTemplateCurrencyData.Value = Math.Min(UnityTemplateCurrencyData.MaxValue, value);
            return UnityTemplateCurrencyData.Value;
        }

        public UnityTemplateItemData GetFirstItem(
            string                             category   = null,
            UnityTemplateItemData.UnlockType      unlockType = UnityTemplateItemData.UnlockType.All,
            IComparer<UnityTemplateItemData>      orderBy    = null,
            params UnityTemplateItemData.Status[] statuses
        )
        {
            return this.GetAllItems(category, unlockType, orderBy, statuses).FirstOrDefault();
        }

        public List<UnityTemplateItemData> GetAllItems(
            string                             category   = null,
            UnityTemplateItemData.UnlockType      unlockType = UnityTemplateItemData.UnlockType.All,
            IComparer<UnityTemplateItemData>      orderBy    = null,
            params UnityTemplateItemData.Status[] statuses
        )
        {
            var query                                                  = this.UnityTemplateInventoryData.IDToItemData.Values.ToList();
            if (category is { }) query                                 = query.Where(itemData => itemData.ItemBlueprintRecord.Category.Equals(category)).ToList();
            if (unlockType != UnityTemplateItemData.UnlockType.All) query = query.Where(itemData => (itemData.ShopBlueprintRecord.UnlockType & unlockType) != 0).ToList();
            if (statuses.Length > 0) query                             = query.Where(itemData => statuses.Contains(itemData.CurrentStatus)).ToList();
            if (orderBy is { }) query                                  = query.OrderBy(itemData => itemData, orderBy).ToList();

            return query;
        }

        public List<UnityTemplateItemData> GetAllItemWithOrder(
            string                        category   = null,
            UnityTemplateItemData.UnlockType unlockType = UnityTemplateItemData.UnlockType.All,
            IComparer<UnityTemplateItemData> comparer   = null
        )
        {
            return this.GetAllItems(category, unlockType).OrderBy(itemData => itemData, comparer ?? UnityTemplateItemData.DefaultComparerInstance).ToList();
        }

        public UnityTemplateItemData UpdateStatusItemData(string id, UnityTemplateItemData.Status status)
        {
            var itemData = this.UnityTemplateInventoryData.IDToItemData.GetOrAdd(id,
                () =>
                {
                    var shopRecord = this.unityTemplateShopBlueprint.GetDataById(id);
                    var itemRecord = this.unityTemplateItemBlueprint.GetDataById(id);

                    return new(id, shopRecord, itemRecord, status);
                });

            itemData.CurrentStatus = status;

            return itemData;
        }

        private void OnLoadBlueprintSuccess()
        {
            //remove item that don't exist in blueprint anymore
            foreach (var notExistKey in this.UnityTemplateInventoryData.IDToItemData.Keys.Where(itemKey => !this.unityTemplateItemBlueprint.ContainsKey(itemKey)).ToList()) this.UnityTemplateInventoryData.IDToItemData.Remove(notExistKey);

            foreach (var itemRecord in this.unityTemplateItemBlueprint.Values)
            {
                // Add item to inventory
                // if item exist in shop blueprint, it's status will be unlocked or owned if IsDefaultItem is true
                // else it's status will be locked

                var status = UnityTemplateItemData.Status.Locked;

                if (this.unityTemplateShopBlueprint.TryGetValue(itemRecord.Id, out var shopRecord)) status = UnityTemplateItemData.Status.Unlocked;

                if (itemRecord.IsDefaultItem) status = UnityTemplateItemData.Status.Owned;

                if (!this.UnityTemplateInventoryData.IDToItemData.TryGetValue(itemRecord.Id, out var existedItemData))
                {
                    #if CREATIVE
                    this.UnityTemplateInventoryData.IDToItemData.Add(itemRecord.Id, new UnityTemplateItemData(itemRecord.Id, shopRecord, itemRecord, UnityTemplateItemData.Status.Owned));
                    #else
                    this.UnityTemplateInventoryData.IDToItemData.Add(itemRecord.Id, new(itemRecord.Id, shopRecord, itemRecord, status));
                    #endif
                }
                else
                {
                    #if CREATIVE
                    existedItemData.CurrentStatus = UnityTemplateItemData.Status.Owned;
                    #endif
                    existedItemData.ShopBlueprintRecord = shopRecord;
                    existedItemData.ItemBlueprintRecord = itemRecord;
                }
            }

            // Set default item
            var defaultItemWithCategory = this.GetDefaultItemWithCategory();

            foreach (var (category, defaultItems) in defaultItemWithCategory)
            {
                var currentItemSelected = this.GetCurrentItemSelected(category);
                if (currentItemSelected is { } && this.unityTemplateItemBlueprint.ContainsKey(currentItemSelected)) continue;
                if (defaultItems is null or { Count: 0 }) continue;
                this.UpdateCurrentSelectedItem(category, defaultItems[0].Id);
            }
        }

        public async UniTask AddGenericReward(string rewardKey, int rewardValue, RectTransform startPosCurrency = null, string claimSoundKey = null)
        {
            if (this.unityTemplateCurrencyBlueprint.TryGetValue(rewardKey, out _))
                await this.AddCurrency(rewardValue, rewardKey, startPosCurrency, claimSoundKey);
            else if (this.unityTemplateItemBlueprint.TryGetValue(rewardKey, out _))
                this.UnityTemplateInventoryData.IDToItemData[rewardKey].CurrentStatus = UnityTemplateItemData.Status.Owned;
            else
                throw new("Need to implemented!!!");
        }

        public async UniTask AddGenericReward(Dictionary<string, int> reward, RectTransform startPosCurrency = null)
        {
            List<UniTask> rewardAnimationTasks = new();
            foreach (var (rewardKey, rewardValue) in reward) rewardAnimationTasks.Add(this.AddGenericReward(rewardKey, rewardValue, startPosCurrency));

            await UniTask.WhenAll(rewardAnimationTasks);
        }

        public bool IsAlreadyContainedItem(Dictionary<string, int> reward)
        {
            foreach (var (rewardKey, _) in reward)
            {
                if (this.unityTemplateItemBlueprint.TryGetValue(rewardKey, out _))
                    if (this.UnityTemplateInventoryData.IDToItemData[rewardKey].CurrentStatus == UnityTemplateItemData.Status.Owned)
                        return true;
            }

            return false;
        }

        public Dictionary<string, UnityTemplateItemData> GetAllItemAvailable()
        {
            return this.UnityTemplateInventoryData.IDToItemData
                .Where(itemData => itemData.Value.CurrentStatus != UnityTemplateItemData.Status.Owned && itemData.Value.CurrentStatus == UnityTemplateItemData.Status.Unlocked)
                .ToDictionary(itemData => itemData.Key, itemData => itemData.Value);
        }

        public void Initialize()
        {
            this.signalBus.Subscribe<LoadBlueprintDataSucceedSignal>(this.OnLoadBlueprintSuccess);
        }

        public bool IsAffordCurrency(Dictionary<string, int> currency, int time = 1)
        {
            foreach (var (currencyKey, currencyValue) in currency)
                if (this.GetCurrencyValue(currencyKey) < currencyValue * time)
                    return false;

            return true;
        }

        public bool IsAffordCurrency(string currencyName, int amount)
        {
            return this.GetCurrencyValue(currencyName) >= amount;
        }
    }
}