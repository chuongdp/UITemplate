namespace HyperGames.UnityTemplate.UnityTemplate.Others.StateMachine.Signals
{
    using HyperGames.HyperCasual.Others.StateMachine.Interface;

    public class OnStateEnterSignal
    {
        public IState State { get; }

        public OnStateEnterSignal(IState state)
        {
            this.State = state;
        }
    }
}