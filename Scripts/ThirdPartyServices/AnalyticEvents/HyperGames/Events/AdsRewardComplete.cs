namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdsRewardComplete : IEvent
    {
        public string Placement;

        public AdsRewardComplete(string placement)
        {
            this.Placement = placement;
        }
    }
}