namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdsIntersEligible : IEvent
    {
        public readonly string placement;

        public AdsIntersEligible(string placement)
        {
            this.placement = placement;
        }
    }
}