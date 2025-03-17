namespace HyperGames.UnityTemplate.Scripts.Models.LocalDatas
{
    using System;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using Sirenix.Serialization;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateAdsData : ILocalData, IUnityTemplateLocalData
    {
        [OdinSerialize] public int WatchedInterstitialAds { get; set; }

        [OdinSerialize] public int WatchedRewardedAds { get; set; }

        public void Init()
        {
        }

        public Type ControllerType => typeof(UnityTemplateAdsController);
    }
}