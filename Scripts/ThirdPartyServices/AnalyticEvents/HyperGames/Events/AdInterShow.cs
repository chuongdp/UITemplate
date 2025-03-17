namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdInterShow : IEvent
    {
        public string Placement;

        public AdInterShow(string placement)
        {
            this.Placement = placement;
        }
    }
}