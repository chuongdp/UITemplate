namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdInterLoad : IEvent
    {
        public string Placement;

        public AdInterLoad(string placement)
        {
            this.Placement = placement;
        }
    }
}