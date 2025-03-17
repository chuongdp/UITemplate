namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.RemoveAdsBottomBar
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.ThirdPartyServices;
    using HyperGames.UnityTemplate.UnityTemplate.Signals;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnityTemplateRemoveAdsBottomBarView : MonoBehaviour
    {
        [SerializeField] private GameObject removeAdsObj;
        [SerializeField] private Button     btnRemoveAds;

        #region inject

        protected SignalBus                  signalBus;
        protected IScreenManager             screenManager;
        protected UnityTemplateAdServiceWrapper adServiceWrapper;

        private void Awake()
        {
            var container = this.GetCurrentContainer();
            this.signalBus        = container.Resolve<SignalBus>();
            this.screenManager    = container.Resolve<IScreenManager>();
            this.adServiceWrapper = container.Resolve<UnityTemplateAdServiceWrapper>();

            this.signalBus.Subscribe<OnRemoveAdsSucceedSignal>(this.OnRemoveAdsSucceedHandler);
            this.signalBus.Subscribe<UnityTemplateOnUpdateBannerStateSignal>(this.OnUpdateBannerStateSignal);
            this.btnRemoveAds.onClick.AddListener(this.OnClickRemoveAdsButton);
        }

        private void OnUpdateBannerStateSignal(UnityTemplateOnUpdateBannerStateSignal obj)
        {
            this.removeAdsObj.SetActive(obj.IsActive);
        }

        #endregion

        private void OnEnable()
        {
            #if HYPERGAMES_IAP && !CREATIVE
            this.removeAdsObj.SetActive(!this.adServiceWrapper.IsRemovedAds);
            #else
            this.removeAdsObj.SetActive(false);
            #endif
        }

        protected virtual void OnClickRemoveAdsButton()
        {
            this.screenManager.OpenScreen<UnityTemplateRemoveAdPopupPresenter>().Forget();
        }

        private void OnRemoveAdsSucceedHandler()
        {
            this.removeAdsObj.SetActive(false);
        }
    }
}