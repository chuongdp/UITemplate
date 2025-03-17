namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class InterstitialAdsSuccess : IEvent
    {
        public string placement;

        public InterstitialAdsSuccess(string placement)
        {
            this.placement = placement;
        }
    }
}