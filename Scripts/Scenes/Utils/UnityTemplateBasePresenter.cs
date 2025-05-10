namespace GameTemplate.Scripts.Scenes.Utils
{
    using System.Collections.Generic;
    using System.Reflection;
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnityTemplateBaseScreenUtils
    {
        private string KeySoundClick => "click_button";

        private readonly IAudioService                soundServices;
        private readonly Dictionary<GameObject, bool> oldActiveStates = new();

        public UnityTemplateBaseScreenUtils()
        {
            var container = this.GetCurrentContainer();
            this.soundServices = container.Resolve<IAudioService>();
        }

        public static UnityTemplateBaseScreenUtils Instance { get; set; }

        public static void Init() { Instance ??= new UnityTemplateBaseScreenUtils(); }

        public static void ReInit() { Instance = new UnityTemplateBaseScreenUtils(); }

        private void OnClickButton(string screenName, Button button)
        {
            Init();
            this.soundServices.PlaySound(this.KeySoundClick);
        }

        public void BindOnClickButton(string screenName, Button[] buttons)
        {
            foreach (var button in buttons) button.onClick.AddListener(() => this.OnClickButton($"{SceneDirector.CurrentSceneName}/{screenName}", button));
        }

        private class FieldInfoComparer : IEqualityComparer<FieldInfo>
        {
            public bool Equals(FieldInfo x, FieldInfo y)
            {
                if (x is null || y is null) return false;

                return x.Name == y.Name && x.DeclaringType == y.DeclaringType;
            }

            public int GetHashCode(FieldInfo obj) { return obj.Name.GetHashCode() ^ (obj.DeclaringType?.GetHashCode() ?? 0); }
        }
    }

    public abstract class UnityTemplateBaseScreenPresenter<TView> : BaseScreenPresenter<TView> where TView : IScreenView
    {
        protected UnityTemplateBaseScreenPresenter(SignalBus signalBus, ILogService logger) : base(signalBus, logger) { UnityTemplateBaseScreenUtils.Init(); }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            UnityTemplateBaseScreenUtils.Instance.BindOnClickButton(this.View.GetType().Name, this.View.RectTransform.GetComponentsInChildren<Button>());
        }
    }
}