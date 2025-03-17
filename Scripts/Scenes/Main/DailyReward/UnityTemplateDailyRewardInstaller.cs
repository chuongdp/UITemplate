#if GDK_ZENJECT
namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward
{
    using System.Linq;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Item;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Pack;
    using Zenject;

    public class UnityTemplateDailyRewardInstaller : Installer<UnityTemplateDailyRewardInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.Bind<UnityTemplateDailyRewardItemViewHelper>().AsCached();
            this.Container.Bind<UnityTemplateDailyRewardPackViewHelper>().AsCached();
            var concreteDailyRewardAnimationHelperType = ReflectionUtils.GetAllDerivedTypes<DailyRewardAnimationHelper>()
                                   .OrderBy(type => type == typeof(DailyRewardAnimationHelper))
                                   .First();
            this.Container.Bind<DailyRewardAnimationHelper>().To(concreteDailyRewardAnimationHelperType).AsCached();
        }
    }
}
#endif