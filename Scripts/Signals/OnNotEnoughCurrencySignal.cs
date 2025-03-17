namespace HyperGames.UnityTemplate.UnityTemplate.Signals
{
    public class OnNotEnoughCurrencySignal
    {
        public OnNotEnoughCurrencySignal(string id)
        {
            this.Id = id;
        }

        public string Id { get; set; }
    }
}