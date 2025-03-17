#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Installers
{
    using System;
    using GameFoundation.Scripts.Interfaces;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.LogService;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.UserData;
    using Zenject;
    using Zenject.Internal;

    public class UnityTemplateLocalDataInstaller : Installer<UnityTemplateLocalDataInstaller>
    {
        public override void InstallBindings()
        {
            this.BindLocalData();
            this.BindAllController();
        }

        private void BindLocalData()
        {
            var logger = this.Container.Resolve<ILogService>();

            ReflectionUtils.GetAllDerivedTypes<ILocalData>().ForEach(type =>
            {
                var data = Activator.CreateInstance(type);
                if (type.DerivesFrom<IUnityTemplateLocalData>())
                {
                    if ((data as IUnityTemplateLocalData)?.ControllerType is { } controllerType)
                    {
                        this.Container.Bind(type).FromInstance(data).WhenInjectedInto(controllerType);
                    }
                    else
                    {
                        logger.Error($"Waring, the local data {type.Name} has no controller, consider to create new controller");
                        this.Container.Bind(type).FromInstance(data).AsCached();
                    }
                }
                else
                {
                    this.Container.Bind(type).FromInstance(data).AsCached();
                }
            });

            this.Container.Bind<UserDataManager>().AsCached();
        }

        private void BindAllController()
        {
            var listController = ReflectionUtils.GetAllDerivedTypes<IUnityTemplateControllerData>();

            foreach (var controller in listController)
            {
                this.Container.BindInterfacesAndSelfTo(controller).AsCached();
            }
        }
    }
}
#endif