namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using UnityEngine.UI;

    public class UnityTemplateAdsButton : Button
    {
        private UnityTemplateAdServiceWrapper adServices;
        private CancellationTokenSource    cts;

        public void OnViewReady(UnityTemplateAdServiceWrapper adService)
        {
            this.adServices = adService;
        }

        public void BindData(string place)
        {
            this.cts          = new();
            this.interactable = false;
            UniTask.WaitUntil(() => this.adServices.IsRewardedAdReady(place), cancellationToken: this.cts.Token).ContinueWith(() => this.interactable = true);
        }

        public void Dispose()
        {
            this.cts?.Dispose();
        }
    }
}