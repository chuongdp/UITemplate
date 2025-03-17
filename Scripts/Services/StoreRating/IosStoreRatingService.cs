#if UNITY_IOS && HYPERGAMES_STORE_RATING
namespace HyperGames.UnityTemplate.UnityTemplate.Services.StoreRating
{
    using Cysharp.Threading.Tasks;
    using UnityEngine.iOS;
    using UnityEngine.Scripting;

    [Preserve]
    public class IosStoreRatingService : IStoreRatingService
    {
        public UniTask LaunchStoreRating()
        {
            Device.RequestStoreReview();
            return UniTask.CompletedTask;
        }
    }
}
#endif