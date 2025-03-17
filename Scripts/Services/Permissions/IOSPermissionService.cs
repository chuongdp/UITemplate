namespace HyperGames.UnityTemplate.UnityTemplate.Services.Permissions
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    #if HYPERGAMES_NOTIFICATION && UNITY_IOS
    using Unity.Notifications.iOS;
    #endif
    using UnityEngine;
    using UnityEngine.Scripting;

    public class IOSPermissionService : BaseUnityPermissionService
    {
        [Preserve]
        public IOSPermissionService(ILogService logService, SignalBus signalBus) : base(logService, signalBus)
        {
        }

        protected override async UniTask<bool> InternalRequestPermission(PermissionRequest request)
        {
            if (Enum.TryParse(request.ToString(), out UserAuthorization authorization) && !Application.HasUserAuthorization(authorization)) await Application.RequestUserAuthorization(authorization);

            return Application.HasUserAuthorization(authorization);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async UniTask<bool> InternalRequestNotificationPermission()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            #if HYPERGAMES_NOTIFICATION && UNITY_IOS
            var iOSNotificationSettings = iOSNotificationCenter.GetNotificationSettings();
            if (iOSNotificationSettings.AuthorizationStatus != AuthorizationStatus.NotDetermined) return iOSNotificationSettings.AuthorizationStatus != AuthorizationStatus.Denied;
            using var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true);
            await UniTask.WaitUntil(() => req.IsFinished);
            return iOSNotificationSettings.AuthorizationStatus != AuthorizationStatus.Denied;
            #endif
            return false;
        }
    }
}