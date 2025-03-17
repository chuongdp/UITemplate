#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.FTUE
{
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.Conditions;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.FTUEListen;
    using Zenject;

    public class UnityTemplateFTUEInstaller : Installer<UnityTemplateFTUEInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.Bind<IFtueCondition>()
                .To(convention => convention.AllNonAbstractClasses().DerivingFrom<IFtueCondition>())
                .AsSingle()
                .WhenInjectedInto<UnityTemplateFTUESystem>();

            this.Container.BindInterfacesAndSelfTo<UnityTemplateFTUESystem>().AsCached();
            this.Container.Bind<UnityTemplateFTUEController>().AsCached();
            this.Container.Bind<UnityTemplateFTUEHelper>().AsCached();
            this.Container.BindInterfacesAndSelfToAllTypeDriveFrom<FTUEBaseListen>();
        }
    }
}
#endif