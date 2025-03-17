namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class AdInterFail : IEvent
    {
        public string errormsg;

        public AdInterFail(string pErrormsg)
        {
            this.errormsg = pErrormsg;
        }
    }
}