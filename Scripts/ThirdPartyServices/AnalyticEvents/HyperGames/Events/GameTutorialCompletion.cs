namespace HyperGames.UnityTemplate.Scripts.ThirdPartyServices.AnalyticEvents.HyperGames
{
    using Core.AnalyticServices.Data;

    public class GameTutorialCompletion : IEvent
    {
        public bool   Success;
        public string TutorialId;

        public GameTutorialCompletion(bool success, string tutorialId)
        {
            this.Success    = success;
            this.TutorialId = tutorialId;
        }
    }
}