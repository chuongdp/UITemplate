namespace HyperGames.UnityTemplate.UnityTemplate.Models
{
    using Sirenix.Serialization;

    public class UnityTemplateCurrencyData
    {
        public                 string Id          { get; private set; }
        [OdinSerialize] public int    Value       { get; internal set; }
        [OdinSerialize] public int    TotalEarned { get; internal set; }
        [OdinSerialize] public int    MaxValue    { get; set; }

        public UnityTemplateCurrencyData(string id, int value, int maxValue, int totalEarned = 0)
        {
            this.Id          = id;
            this.Value       = value;
            this.TotalEarned = totalEarned;
            this.MaxValue    = maxValue;
        }
    }
}