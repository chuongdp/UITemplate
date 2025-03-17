namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdsRewardOffer : IEvent
    {
        public string Placement;

        public AdsRewardOffer(string placement)
        {
            this.Placement = placement;
        }
    }
}