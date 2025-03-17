namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Scenes.Utils;
    using HyperGames.UnityTemplate.Scripts.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Services;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateConnectErrorPopupView : BaseView
    {
        public Button   Reconnect;
        public TMP_Text Message;
        public TMP_Text ButtonText;
    }

    [PopupInfo(nameof(UnityTemplateConnectErrorPopupView), true, false)]
    public class UnityTemplateConnectErrorPresenter : UnityTemplateBasePopupPresenter<UnityTemplateConnectErrorPopupView>
    {
        protected virtual double CheckTimeout        => 5;
        protected virtual string ConnectingMessage   => "Trying to reconnect...\nPlease wait...";
        protected virtual string ConnectErrorMessage => "Your connection has been lost!\nCheck your internet connection and try again";

        protected virtual string ReconnectButtonMessage    => "Reconnect";
        protected virtual string ReconnectingButtonMessage => "Reconnecting";

        [Preserve]
        public UnityTemplateConnectErrorPresenter(
            SignalBus        signalBus,
            ILogService      logger,
            IScreenManager   screenManager,
            IInternetService internetService
        ) : base(signalBus, logger)
        {
            this.screenManager   = screenManager;
            this.internetService = internetService;
        }

        public override UniTask BindData()
        {
            this.UpdateContent(false);
            return UniTask.CompletedTask;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.Reconnect.onClick.AddListener(this.OnClickReconnect);
        }

        protected virtual void OnConnectSuccess()
        {
            this.CloseView();
        }

        private void UpdateContent(bool isConnecting)
        {
            //this.View.Message.text           = isConnecting ? this.ConnectingMessage : this.ConnectErrorMessage;
            //this.View.ButtonText.text        = isConnecting ? this.ReconnectingButtonMessage : this.ReconnectButtonMessage;
            this.View.Reconnect.interactable = !isConnecting;
            //this.UpdateImage(isConnecting);
        }

        private async void OnClickReconnect()
        {
            var time                      = Time.realtimeSinceStartup;
            var timeSinceLastConnectCheck = time - 0.1;
            this.UpdateContent(true);
            var isConnected = false;
            await UniTask.WaitUntil(() =>
            {
                var intervalTime = Time.realtimeSinceStartup - timeSinceLastConnectCheck;
                if (intervalTime >= 1)
                {
                    isConnected               = this.internetService.IsInternetAvailable;
                    timeSinceLastConnectCheck = Time.realtimeSinceStartup;
                }

                return isConnected || Time.realtimeSinceStartup - time > this.CheckTimeout;
            });

            if (isConnected)
            {
                this.OnConnectSuccess();
                return;
            }

            this.UpdateContent(false);
        }

        #region MyRegion

        private readonly IScreenManager   screenManager;
        private readonly IInternetService internetService;

        #endregion
    }
}