namespace HyperGames.UnityTemplate.UnityTemplate.Scripts.Services
{
    using System;
    using GameFoundation.Scripts.Utilities.LogService;

    public interface IUnityTemplateScreenShow
    {
        Type ScreenPresenter { get; }
        void OnProcessScreenShow();
    }

    public abstract class UnityTemplateBaseScreenShow : IUnityTemplateScreenShow
    {
        private readonly ILogService logger;
        public abstract  Type        ScreenPresenter { get; }

        protected UnityTemplateBaseScreenShow(ILogService logger)
        {
            this.logger = logger;
        }

        public abstract void OnProcessScreenShow();
    }
}