#if GDK_VCONTAINER
#nullable enable
namespace GameTemplate
{
    using HyperGames.StoreRating;
    using UnityEngine;
    using VContainer;

    public static class UnityTemplateVContainer
    {
        public static void RegisterUnityTemplate(this IContainerBuilder builder, Transform rootTransform)
        {
            Application.targetFrameRate = 60;

            builder.RegisterUnityTemplateLocalData();
            builder.RegisterUnityTemplateServices(rootTransform);
            builder.RegisterStoreRatingService();
        }
    }
}
#endif