namespace HyperGames.UnityTemplate.UnityTemplate.FTUE.Signal
{
    public class FTUEButtonClickSignal : IHaveStepId
    {
        public string StepId { get; set; }

        public FTUEButtonClickSignal(string stepId)
        {
            this.StepId = stepId;
        }
    }
}