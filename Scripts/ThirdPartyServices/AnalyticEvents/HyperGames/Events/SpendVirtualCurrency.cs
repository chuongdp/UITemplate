namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class SpendVirtualCurrency : IEvent
    {
        public string VirtualCurrencyName;
        public long   Value;
        public string ItemName;

        public SpendVirtualCurrency(string virtualCurrencyName, long value, string itemName)
        {
            this.VirtualCurrencyName = virtualCurrencyName;
            this.Value               = value;
            this.ItemName            = itemName;
        }
    }
}