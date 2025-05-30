﻿#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate
{
    using GameFoundation.DI;
    using GameFoundation.Scripts.Interfaces;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.UserData;
    using VContainer;

    public static class UnityTemplateLocalDataVContainer
    {
        public static void RegisterUnityTemplateLocalData(this IContainerBuilder builder)
        {
            typeof(ILocalData).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton));
            typeof(IUnityTemplateControllerData).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton).AsInterfacesAndSelf());
            builder.Register<UserDataManager>(Lifetime.Singleton);
        }
    }
}
#endif