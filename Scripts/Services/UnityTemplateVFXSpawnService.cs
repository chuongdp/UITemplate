namespace HyperGames.UnityTemplate.UnityTemplate.Services
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using GameFoundation.Signals;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateVFXSpawnService
    {
        private readonly IGameAssets gameAssets;

        [Preserve]
        public UnityTemplateVFXSpawnService(SignalBus signalBus, IGameAssets gameAssets) { this.gameAssets = gameAssets; }

        public async void SpawnVFX(Transform target,    Transform parent,               List<string> listVFXKey,
                                   bool      randomPos, bool      randomRotate = false, bool         isFloat = false)
        {
            var randomIndex = Random.Range(0, listVFXKey.Count - 1);
            var vfxKey      = listVFXKey[randomIndex];
            var vfxPrefab   = await this.gameAssets.LoadAssetAsync<GameObject>(vfxKey);
            // spawn vfx follow target's position
            var position = target.position;
            //spawn random position base on target's position
            if (randomPos)
            {
                position.x += Random.Range(-1f, 1f);
                position.y += Random.Range(0f,  2f);
                position.z =  -1;
            }

            // random vfx rotation
            var rotation = randomRotate ? Quaternion.Euler(0, 0, Random.Range(-50, 50)) : Quaternion.identity;
            // spawn vfx
            var vfxObj = parent != null ? vfxPrefab.Spawn(parent, position, rotation) : vfxPrefab.Spawn(position, rotation);
            if (isFloat) vfxObj.transform.DOMoveY(position.y + 1f, 1f);
            await UniTask.Delay(2000);
            vfxObj.Recycle();
        }

        public async UniTask<GameObject> SpawnVFX(Transform target, Transform parent, List<string> listVFXKey)
        {
            var randomIndex = Random.Range(0, listVFXKey.Count - 1);
            var vfxKey      = listVFXKey[randomIndex];
            var vfxPrefab   = await this.gameAssets.LoadAssetAsync<GameObject>(vfxKey);
            // spawn vfx follow target's position
            var position = target.position;
            // spawn vfx
            var vfxObj = parent != null ? vfxPrefab.Spawn(parent, position) : vfxPrefab.Spawn(position);

            return vfxObj;
        }

        public async UniTask<GameObject> SpawnVFX(Vector3 target, Transform parent, List<string> listVFXKey)
        {
            var randomIndex = Random.Range(0, listVFXKey.Count - 1);
            var vfxKey      = listVFXKey[randomIndex];
            var vfxPrefab   = await this.gameAssets.LoadAssetAsync<GameObject>(vfxKey);
            // spawn vfx follow target's position
            var position = target;
            // spawn vfx
            var vfxObj = parent != null ? vfxPrefab.Spawn(parent, position) : vfxPrefab.Spawn(position);

            return vfxObj;
        }
    }
}