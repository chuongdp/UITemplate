#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate.UnityTemplate.FTUE
{
    using System.Linq;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.Conditions;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.FTUEListen;
    using VContainer;

    public static class UnityTemplateFTUEVContainer
    {
        public static void RegisterFTUESystem(this IContainerBuilder builder)
        {
            builder.Register<UnityTemplateFTUEController>(Lifetime.Singleton);
            builder.Register<UnityTemplateFTUEHelper>(Lifetime.Singleton);
            typeof(FTUEBaseListen).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces());

            builder.Register<UnityTemplateFTUESystem>(Lifetime.Singleton)
                .WithParameter(container => typeof(IFtueCondition).GetDerivedTypes().Select(type => (IFtueCondition)container.Instantiate(type)).ToList())
                .AsInterfacesAndSelf();
        }
    }
}
#endif