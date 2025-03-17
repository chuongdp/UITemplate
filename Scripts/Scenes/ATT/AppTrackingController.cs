namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.ATT
{
    using ServiceImplementation.AdsServices.ConsentInformation;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class AppTrackingController : MonoBehaviour
    {
        [SerializeField] private GameObject attView;
        [SerializeField] private Button     btnRequestTracking;

        public virtual void Awake()
        {
            this.CheckRequestTracking();
            this.btnRequestTracking.onClick.AddListener(this.OnClickRequestTracking);
        }

        private void CheckRequestTracking()
        {
            if (AttHelper.IsRequestTrackingComplete())
            {
                this.attView.gameObject.SetActive(false);
                LoadLoadingScene();
            }
            else
            {
                this.attView.gameObject.SetActive(true);
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void OnClickRequestTracking()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            this.btnRequestTracking.interactable = false;
            if (!AttHelper.IsRequestTrackingComplete())
            {
                #if UNITY_IOS
                Unity.Advertisement.IosSupport.ATTrackingStatusBinding.RequestAuthorizationTracking();
                await Cysharp.Threading.Tasks.UniTask.WaitUntil(AttHelper.IsRequestTrackingComplete);
                #endif
            }

            LoadLoadingScene();
        }

        public virtual void LoadLoadingScene()
        {
            SceneManager.LoadScene("0.LoadingScene");
        }
    }
}