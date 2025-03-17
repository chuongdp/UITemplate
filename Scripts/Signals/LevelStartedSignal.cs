namespace HyperGames.UnityTemplate.UnityTemplate.Signals
{
    public class LevelStartedSignal
    {
        public int Level;

        public LevelStartedSignal(int level)
        {
            this.Level = level;
        }
    }
}