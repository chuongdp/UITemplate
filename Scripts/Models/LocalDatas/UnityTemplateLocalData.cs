namespace HyperGames.UnityTemplate.Models
{
    using System;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;

    public abstract class UnityTemplateLocalData<TController> : ILocalData, IUnityTemplateLocalData
    {
        Type IUnityTemplateLocalData.ControllerType => typeof(TController);

        void ILocalData.Init()
        {
            this.Init();
        }

        protected virtual void Init()
        {
        }
    }
}