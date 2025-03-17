namespace HyperGames.UnityTemplate.UnityTemplate.Services.Permissions
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using UnityEngine.Scripting;

    public class DummyPermissionService : BaseUnityPermissionService
    {
        [Preserve]
        public DummyPermissionService(ILogService logService, SignalBus signalBus) : base(logService, signalBus)
        {
        }

        protected override async UniTask<bool> InternalRequestPermission(PermissionRequest request)
        {
            this.LOGService.Log($"mirailog: DummyPermissionService InternalRequestPermission: {request}");
            await UniTask.CompletedTask;
            return false;
        }

        protected override async UniTask<bool> InternalRequestNotificationPermission()
        {
            this.LOGService.Log($"mirailog: DummyPermissionService InternalRequestNotificationPermission");
            await UniTask.CompletedTask;
            return false;
        }
    }
}