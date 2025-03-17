namespace HyperGames.UnityTemplate.UnityTemplate.Others.StateMachine.Signals
{
    using HyperGames.HyperCasual.Others.StateMachine.Interface;

    public class OnStateExitSignal
    {
        public IState State { get; }

        public OnStateExitSignal(IState state)
        {
            this.State = state;
        }
    }
}