namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Loading
{
    using System;
    using System.Diagnostics;
    using BlueprintFlow.BlueprintControlFlow;
    using BlueprintFlow.Signals;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Signals;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.UnityTemplate.UserData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using UnityEngine.Scripting;
    using UnityEngine.UI;
    using Debug = UnityEngine.Debug;
    using Object = UnityEngine.Object;

    public class UnityTemplateLoadingScreenView : BaseView
    {
        [SerializeField] private Slider          LoadingSlider;
        [SerializeField] private TextMeshProUGUI loadingProgressTxt;

        public float  Progress    { get; set; }
        public string LoadingText { get; set; }

        private float visibleProgress;

        private void Update()
        {
            this.visibleProgress = Mathf.Lerp(this.visibleProgress, this.Progress, Time.unscaledDeltaTime * 5f);
            if (this.LoadingSlider is not null) this.LoadingSlider.value = this.visibleProgress;

            if (this.loadingProgressTxt is not null && this.LoadingText is not null) this.loadingProgressTxt.text = string.Format(this.LoadingText, Mathf.RoundToInt(this.visibleProgress * 100));
        }

        public UniTask CompleteLoading()
        {
            this.Progress = 1f;

            return UniTask.WaitUntil(() => this.visibleProgress >= .999f);
        }
    }

    [ScreenInfo(nameof(UnityTemplateLoadingScreenView))]
    public class UnityTemplateLoadingScreenPresenter : UnityTemplateBaseScreenPresenter<UnityTemplateLoadingScreenView>
    {
        #region Inject

        protected readonly BlueprintReaderManager blueprintManager;
        protected readonly UserDataManager        userDataManager;
        protected readonly IGameAssets            gameAssets;
        private readonly   ObjectPoolManager      objectPoolManager;

        [Preserve]
        protected UnityTemplateLoadingScreenPresenter(
            SignalBus              signalBus,
            ILogService            logger,
            BlueprintReaderManager blueprintManager,
            UserDataManager        userDataManager,
            IGameAssets            gameAssets,
            ObjectPoolManager      objectPoolManager
        ) : base(signalBus, logger)
        {
            this.blueprintManager  = blueprintManager;
            this.userDataManager   = userDataManager;
            this.gameAssets        = gameAssets;
            this.objectPoolManager = objectPoolManager;
        }

        #endregion

        protected virtual string NextSceneName => "1.MainScene";

        /// <summary>
        /// Please fill loading text with format "Text {0}" where {0} is the value position."
        /// </summary>
        /// <param name="text"></param>
        protected virtual string GetLoadingText() { return "Loading {0}%"; }

        private bool IsClosedFirstOpen { get; set; }

        private float _loadingProgress;
        private int   loadingSteps;

        private float LoadingProgress
        {
            get => this._loadingProgress;
            set
            {
                this._loadingProgress = value;

                if (value                  / this.loadingSteps <= this.View.Progress) return;
                this.View.Progress = value / this.loadingSteps;
            }
        }

        private GameObject objectPoolContainer;

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.LoadingText = this.GetLoadingText();
            this.OpenViewAsync().Forget();
        }

        public override UniTask BindData()
        {
            this.ShowFirstBannerAd();

            this.objectPoolContainer = new GameObject(nameof(this.objectPoolContainer));
            Object.DontDestroyOnLoad(this.objectPoolContainer);

            this.LoadingProgress = 0f;
            this.loadingSteps    = 1;

            var stopWatch = Stopwatch.StartNew();
            UniTask.WhenAll(
                this.CreateObjectPool(AudioService.AudioSourceKey, 3),
                this.Preload(),
#if ADMOB_IMPLEMENT || MAX_IMPLEMENT
                this.ShowAoa(),
#endif
                UniTask.WhenAll(
                    this.LoadBlueprint().ContinueWith(this.OnBlueprintLoaded),
                    this.LoadUserData().ContinueWith(this.OnUserDataLoaded)
                ).ContinueWith(this.OnBlueprintAndUserDataLoaded)
            ).ContinueWith(this.OnLoadingCompleted).ContinueWith(this.LoadNextScene).Forget();
            stopWatch.Stop();
            Debug.Log("Game Loading Time: " + stopWatch.ElapsedMilliseconds + "ms");

            return UniTask.CompletedTask;
        }

        protected virtual async UniTask LoadNextScene()
        {
            SceneDirector.CurrentSceneName = this.NextSceneName;

            var stopWatch = Stopwatch.StartNew();

            this.SignalBus.Fire<StartLoadingNewSceneSignal>();
            var nextScene = await this.TrackProgress(this.LoadSceneAsync());
            await this.View.CompleteLoading();
            await nextScene.ActivateAsync();
            this.SignalBus.Fire<FinishLoadingNewSceneSignal>();

            stopWatch.Stop();
            Debug.Log("Loading Main Scene Time: " + stopWatch.ElapsedMilliseconds + "ms");

            Resources.UnloadUnusedAssets().ToUniTask().Forget();
            this.ShowFirstBannerAd();
            this.OnAfterLoading();
        }

        protected virtual void ShowFirstBannerAd() { }

        protected virtual void OnAfterLoading() { }

        protected virtual AsyncOperationHandle<SceneInstance> LoadSceneAsync() { return this.gameAssets.LoadSceneAsync(this.NextSceneName, LoadSceneMode.Single, false); }

        private UniTask LoadBlueprint()
        {
            this.TrackProgress<LoadBlueprintDataProgressSignal>();
            this.TrackProgress<ReadBlueprintProgressSignal>();

            return this.blueprintManager.LoadBlueprint();
        }

        private UniTask LoadUserData() { return this.TrackProgress(this.userDataManager.LoadUserData()); }

        protected virtual UniTask OnBlueprintLoaded() { return UniTask.CompletedTask; }

        protected virtual UniTask OnUserDataLoaded() { return UniTask.CompletedTask; }

        protected virtual UniTask OnBlueprintAndUserDataLoaded() { return UniTask.CompletedTask; }

        protected virtual UniTask OnLoadingCompleted() { return UniTask.CompletedTask; }

        protected virtual UniTask Preload() { return UniTask.CompletedTask; }

        protected virtual UniTask ShowAoa() { return UniTask.CompletedTask; }

        protected UniTask PreloadAssets<T>(params object[] keys)
        {
            return UniTask.WhenAll(this.gameAssets.PreloadAsync<T>(this.NextSceneName, keys)
                .Select(this.TrackProgress));
        }

        protected UniTask CreateObjectPool(string prefabName, int initialPoolSize = 1)
        {
            return this.TrackProgress(
                this.objectPoolManager.CreatePool(prefabName, initialPoolSize, this.objectPoolContainer));
        }

        protected UniTask TrackProgress(UniTask task)
        {
            ++this.loadingSteps;

            return task.ContinueWith(() => ++this.LoadingProgress);
        }

        protected UniTask<T> TrackProgress<T>(AsyncOperationHandle<T> aoh)
        {
            ++this.loadingSteps;
            var localLoadingProgress = 0f;

            void UpdateProgress(float progress)
            {
                this.LoadingProgress += progress - localLoadingProgress;
                localLoadingProgress =  progress;
            }

            return aoh.ToUniTask(Progress.CreateOnlyValueChanged<float>(UpdateProgress))
                .ContinueWith(result =>
                {
                    UpdateProgress(1f);

                    return result;
                });
        }

        protected void TrackProgress<T>() where T : IProgressPercent
        {
            ++this.loadingSteps;
            var localLoadingProgress = 0f;

            this.SignalBus.Subscribe<T>(UpdateProgress);

            void UpdateProgress(T progress)
            {
                this.LoadingProgress += progress.Percent - localLoadingProgress;
                localLoadingProgress =  progress.Percent;
                if (progress.Percent >= 1f) this.SignalBus.Unsubscribe<T>(UpdateProgress);
            }
        }
    }
}