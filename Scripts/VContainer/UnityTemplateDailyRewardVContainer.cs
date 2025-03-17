#if GDK_VCONTAINER
#nullable enable
namespace HyperGames.UnityTemplate
{
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Item;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Pack;
    using VContainer;

    public static class UnityTemplateDailyRewardVContainer
    {
        public static void RegisterDailyReward(this IContainerBuilder builder)
        {
            builder.RegisterFromDerivedType<UnityTemplateDailyRewardItemViewHelper>();
            builder.RegisterFromDerivedType<UnityTemplateDailyRewardPackViewHelper>();
            builder.RegisterFromDerivedType<DailyRewardAnimationHelper>();
        }
    }
}
#endif