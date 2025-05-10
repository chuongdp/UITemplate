namespace GameTemplate.UnityTemplate.Others.StateMachine.Interface
{
    using System;
    using HyperGames.HyperCasual.Others.StateMachine.Interface;

    public interface IStateMachine
    {
        IState CurrentState { get; }

        void TransitionTo(Type stateType);

        void TransitionTo<T>() where T : class, IState;

        public void TransitionTo<TState, TModel>(TModel model) where TState : class, IState<TModel>;
    }
}