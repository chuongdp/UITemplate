namespace HyperGames.UnityTemplate.UnityTemplate.Signals
{
    public class OnFinishCurrencyAnimationSignal
    {
        public string Id;
        public int    Amount;
        public int    FinalValue;

        public OnFinishCurrencyAnimationSignal(string id, int amount, int finalValue)
        {
            this.Id         = id;
            this.Amount     = amount;
            this.FinalValue = finalValue;
        }
    }
}