namespace HyperGames.UnityTemplate.Quests.Signals
{
    public sealed class QuestStatusChangedSignal
    {
        public UnityTemplateQuestController QuestController { get; }

        internal QuestStatusChangedSignal(UnityTemplateQuestController questController)
        {
            this.QuestController = questController;
        }
    }
}