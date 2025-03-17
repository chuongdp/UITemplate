namespace HyperGames.UnityTemplate.Scripts.Models.Core.Element
{
    using System;
    using System.Collections.Generic;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using Newtonsoft.Json;

    public class UnityTemplateItemData
    {
        public static readonly DefaultComparer DefaultComparerInstance = new();

        public readonly string Id;
        public          Status CurrentStatus;
        public          int    RemainingAdsProgress;

        [JsonIgnore] public UnityTemplateShopRecord ShopBlueprintRecord { get; internal set; }

        [JsonIgnore] public UnityTemplateItemRecord ItemBlueprintRecord { get; internal set; }

        public UnityTemplateItemData(
            string               id,
            UnityTemplateShopRecord shopBlueprintRecord,
            UnityTemplateItemRecord itemBlueprintRecord,
            Status               currentStatus = Status.Locked
        )
        {
            this.Id                  = id;
            this.CurrentStatus       = currentStatus;
            this.ShopBlueprintRecord = shopBlueprintRecord;
            this.ItemBlueprintRecord = itemBlueprintRecord;

            if (this.ShopBlueprintRecord is { } && (this.ShopBlueprintRecord.UnlockType & UnlockType.Ads) != 0) this.RemainingAdsProgress = this.ShopBlueprintRecord?.Price ?? 0;
        }

        public enum Status
        {
            Owned      = 0,
            Unlocked   = 1,
            InProgress = 2,
            Locked     = 3,
        }

        [Flags]
        public enum UnlockType
        {
            None         = 0,
            IAP          = 1 << 1,
            SoftCurrency = 1 << 2,
            Ads          = 1 << 3,
            Progression  = 1 << 4,
            Gift         = 1 << 5,
            DailyReward  = 1 << 6,
            LuckySpin    = 1 << 7,
            StartedPack  = 1 << 8,
            Shard        = 1 << 9,
            All          = -1,
            Default      = UnlockType.IAP | UnlockType.SoftCurrency | UnlockType.Ads | UnlockType.Progression | UnlockType.Gift | UnlockType.StartedPack,
        }

        public class DefaultComparer : IComparer<UnityTemplateItemData>
        {
            public int Compare(UnityTemplateItemData x, UnityTemplateItemData y)
            {
                //Check ref and null first
                if (object.ReferenceEquals(x, y)) return 0;
                if (object.ReferenceEquals(null, y)) return 1;
                if (object.ReferenceEquals(null, x)) return -1;

                //Check status first
                var currentStatusComparison = x.CurrentStatus.CompareTo(y.CurrentStatus);

                if (currentStatusComparison != 0) return currentStatusComparison;

                //If status is equal, then check progress
                var progressComparison = x.RemainingAdsProgress.CompareTo(y.RemainingAdsProgress);

                if (progressComparison != 0) return progressComparison;

                //if progress is equal, then check id
                return string.Compare(x.Id, y.Id, StringComparison.Ordinal);
            }
        }
    }
}