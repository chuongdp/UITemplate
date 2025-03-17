namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdInterClick : IEvent
    {
        public string Placement;

        public AdInterClick(string placement)
        {
            this.Placement = placement;
        }
    }
}