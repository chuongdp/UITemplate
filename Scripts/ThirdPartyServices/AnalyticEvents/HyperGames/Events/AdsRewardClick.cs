namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdsRewardClick : IEvent
    {
        public string Placement;

        public AdsRewardClick(string placement)
        {
            this.Placement = placement;
        }
    }
}