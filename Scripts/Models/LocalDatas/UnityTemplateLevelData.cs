namespace HyperGames.UnityTemplate.Scripts.Models.LocalDatas
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using Sirenix.Serialization;
    using UnityEngine.Scripting;

    [Preserve]
    public class UnityTemplateUserLevelData : ILocalData, IUnityTemplateLocalData
    {
        [OdinSerialize] public UnityTemplateItemData.UnlockType UnlockedFeature { get; set; } = UnityTemplateItemData.UnlockType.Default;

        [OdinSerialize] public int CurrentLevel { get; set; } = 1;

        [OdinSerialize] public Dictionary<int, LevelData> LevelToLevelData { get; set; } = new();

        [OdinSerialize] public int LastUnlockRewardLevel;

        public void SetLevelStatusByLevel(int level, LevelData.Status status)
        {
            this.LevelToLevelData[level].LevelStatus = status;
        }

        public void Init()
        {
            #if CREATIVE
            foreach (var levelData in this.LevelToLevelData.Values.ToList())
            {
                levelData.LevelStatus = LevelData.Status.Passed;
            }
            #endif
        }

        public Type ControllerType => typeof(UnityTemplateLevelDataController);
    }

    public class LevelData
    {
        public int    Level;
        public Status LevelStatus;
        public int    StarCount;
        public int    LoseCount;
        public int    WinCount;

        public LevelData(int level, Status levelStatus, int loseCount = 0, int winCount = 0, int starCount = 0)
        {
            this.Level       = level;
            this.LevelStatus = levelStatus;
            this.LoseCount   = loseCount;
            this.WinCount    = winCount;
            this.StarCount   = starCount;
        }

        public enum Status
        {
            Locked,
            Passed,
            Skipped,
        }
    }
}