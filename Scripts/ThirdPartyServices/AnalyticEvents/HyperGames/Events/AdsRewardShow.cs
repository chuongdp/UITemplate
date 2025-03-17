namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdsRewardShow : IEvent
    {
        public string Placement;

        public AdsRewardShow(string placement)
        {
            this.Placement = placement;
        }
    }
}