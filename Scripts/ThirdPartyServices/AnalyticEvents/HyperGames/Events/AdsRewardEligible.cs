namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdsRewardEligible : IEvent

    {
        private readonly string placement;

        public AdsRewardEligible(string placement)
        {
            this.placement = placement;
        }
    }
}