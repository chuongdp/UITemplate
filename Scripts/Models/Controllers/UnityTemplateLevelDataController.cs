namespace HyperGames.UnityTemplate.Scripts.Models.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.UserData;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using HyperGames.UnityTemplate.Scripts.Models.Core.Element;
    using HyperGames.UnityTemplate.Scripts.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Signals;
    using UnityEngine.Scripting;

    public class UnityTemplateLevelDataController : IUnityTemplateControllerData
    {
        #region inject

        private readonly UnityTemplateLevelBlueprint          unityTemplateLevelBlueprint;
        private readonly UnityTemplateUserLevelData           UnityTemplateUserLevelData;
        private readonly UnityTemplateInventoryDataController UnityTemplateInventoryDataController;
        private readonly SignalBus                         signalBus;
        private readonly IHandleUserDataServices           handleUserDataServices;

        #endregion

        [Preserve]
        public UnityTemplateLevelDataController(UnityTemplateLevelBlueprint unityTemplateLevelBlueprint, UnityTemplateUserLevelData UnityTemplateUserLevelData, UnityTemplateInventoryDataController UnityTemplateInventoryDataController, SignalBus signalBus, IHandleUserDataServices handleUserDataServices)
        {
            this.unityTemplateLevelBlueprint          = unityTemplateLevelBlueprint;
            this.UnityTemplateUserLevelData           = UnityTemplateUserLevelData;
            this.UnityTemplateInventoryDataController = UnityTemplateInventoryDataController;
            this.signalBus                         = signalBus;
            this.handleUserDataServices            = handleUserDataServices;
        }

        public UnityTemplateItemData.UnlockType UnlockedFeature => this.UnityTemplateUserLevelData.UnlockedFeature;

        public bool IsFeatureUnlocked(UnityTemplateItemData.UnlockType feature)
        {
            return (this.UnityTemplateUserLevelData.UnlockedFeature & feature) != 0;
        }

        public void UnlockFeature(UnityTemplateItemData.UnlockType feature)
        {
            this.UnityTemplateUserLevelData.UnlockedFeature |= feature;
        }

        public int LastUnlockRewardLevel { get => this.UnityTemplateUserLevelData.LastUnlockRewardLevel; set => this.UnityTemplateUserLevelData.LastUnlockRewardLevel = value; }

        public List<LevelData> GetAllLevels()
        {
            return this.unityTemplateLevelBlueprint.Values.Select(levelRecord => this.GetLevelData(levelRecord.Level)).ToList();
        }

        public LevelData GetLevelData(int level)
        {
            return this.UnityTemplateUserLevelData.LevelToLevelData.GetOrAdd(level, () => new(level, LevelData.Status.Locked));
        }

        /// <summary>Have be called when level started</summary>
        public void PlayCurrentLevel()
        {
            this.signalBus.Fire(new LevelStartedSignal(this.UnityTemplateUserLevelData.CurrentLevel));
        }

        /// <summary>
        /// Called when select a level in level selection screen
        /// </summary>
        /// <param name="level">selected level</param>
        public void SelectLevel(int level)
        {
            this.UnityTemplateUserLevelData.CurrentLevel = level;

            this.handleUserDataServices.SaveAll();
        }

        /// <summary>
        /// Called when player lose current level
        /// </summary>
        /// <param name="time">Play time in seconds</param>
        public void LoseCurrentLevel(int time = 0)
        {
            this.signalBus.Fire(new LevelEndedSignal { Level = this.UnityTemplateUserLevelData.CurrentLevel, IsWin = false, Time = time, CurrentIdToValue = null });
            this.GetLevelData(this.UnityTemplateUserLevelData.CurrentLevel).LoseCount++;

            this.handleUserDataServices.SaveAll();
        }

        public int TotalLose => this.UnityTemplateUserLevelData.LevelToLevelData.Values.Sum(levelData => levelData.LoseCount);
        public int TotalWin  => this.UnityTemplateUserLevelData.LevelToLevelData.Values.Sum(levelData => levelData.WinCount);

        /// <summary>
        /// Called when player win current level
        /// </summary>
        /// <param name="time">Play time in seconds</param>
        public void PassCurrentLevel(int time = 0)
        {
            this.GetLevelData(this.UnityTemplateUserLevelData.CurrentLevel).WinCount++;
            this.UnityTemplateUserLevelData.SetLevelStatusByLevel(this.UnityTemplateUserLevelData.CurrentLevel, LevelData.Status.Passed);
            this.signalBus.Fire(new LevelEndedSignal { Level = this.UnityTemplateUserLevelData.CurrentLevel, IsWin = true, Time = time, CurrentIdToValue = null });
            if (this.GetCurrentLevelData.LevelStatus == LevelData.Status.Locked) this.UnityTemplateUserLevelData.SetLevelStatusByLevel(this.UnityTemplateUserLevelData.CurrentLevel, LevelData.Status.Passed);
            this.UnityTemplateUserLevelData.CurrentLevel++;

            this.handleUserDataServices.SaveAll();
        }

        /// <summary>
        /// Called when player skip current level
        /// </summary>
        /// <param name="time">Play time in seconds</param>
        public void SkipCurrentLevel(int time = 0)
        {
            if (this.GetCurrentLevelData.LevelStatus == LevelData.Status.Locked) this.UnityTemplateUserLevelData.SetLevelStatusByLevel(this.UnityTemplateUserLevelData.CurrentLevel, LevelData.Status.Skipped);
            this.signalBus.Fire(new LevelSkippedSignal { Level = this.UnityTemplateUserLevelData.CurrentLevel, Time = time });
            this.UnityTemplateUserLevelData.CurrentLevel++;

            this.handleUserDataServices.SaveAll();
        }

        public LevelData GetCurrentLevelData => this.GetLevelData(this.UnityTemplateUserLevelData.CurrentLevel);
        public int       CurrentLevel        => this.GetLevelData(this.UnityTemplateUserLevelData.CurrentLevel).Level;

        public int MaxLevel
        {
            get
            {
                var levelDatas = this.UnityTemplateUserLevelData.LevelToLevelData.Values.Where(levelData => levelData.LevelStatus == LevelData.Status.Passed).ToList();

                return levelDatas.Count == 0 ? 0 : levelDatas.Max(data => data.Level);
            }
        }

        public int TotalLevelSurpassed
        {
            get
            {
                var levelDatas = this.UnityTemplateUserLevelData.LevelToLevelData.Values.Where(levelData => levelData.LevelStatus != LevelData.Status.Locked).ToList();

                return levelDatas.Count == 0 ? 0 : levelDatas.Max(data => data.Level);
            }
        }

        public bool CheckLevelIsUnlockedStatus(int level)
        {
            var skippedLevel      = this.UnityTemplateUserLevelData.LevelToLevelData.Values.LastOrDefault(levelData => levelData.LevelStatus == LevelData.Status.Skipped);
            var skippedLevelIndex = this.UnityTemplateUserLevelData.LevelToLevelData.Values.ToList().IndexOf(skippedLevel);
            if (skippedLevelIndex == -1 && this.MaxLevel == 0 && level == 1) return true;

            var maxIndex = Math.Max(skippedLevelIndex, this.MaxLevel);

            return level == maxIndex + 1;
        }

        public float GetRewardProgress(int level)
        {
            var levelUnlockReward = this.GetLevelUnlockReward(level);
            if (levelUnlockReward < 0) return 0;

            //update last unlock reward level
            var temp = this.GetLastLevelUnlockReward(level);

            return (float)(level - temp) / (levelUnlockReward - temp);
        }

        public string GetRewardId(int currentLevel)
        {
            var levelUnlockReward = this.GetLevelUnlockReward(currentLevel);

            if (levelUnlockReward < 0) return null;
            var rewardList = this.unityTemplateLevelBlueprint.GetDataById(levelUnlockReward).Rewards;

            for (var i = 0; i < rewardList.Count; i++)
                if (this.UnityTemplateInventoryDataController.GetItemData(rewardList[i]).CurrentStatus != UnityTemplateItemData.Status.Owned)
                    return rewardList[i];

            return null;
        }

        public Dictionary<string, int> GetAltReward(int currentLevel)
        {
            var levelUnlockReward = this.GetLevelUnlockReward(currentLevel);

            return levelUnlockReward < 0 ? null : this.unityTemplateLevelBlueprint.GetDataById(levelUnlockReward).AltReward;
        }

        private int GetLevelUnlockReward(int level)
        {
            for (var i = level; i <= this.unityTemplateLevelBlueprint.Count; i++)
                if (this.unityTemplateLevelBlueprint.GetDataById(i).Rewards.Count > 0)
                    return i;

            return -1;
        }

        private int GetLastLevelUnlockReward(int level)
        {
            for (var i = level - 1; i > 0; i--)
                if (this.unityTemplateLevelBlueprint.GetDataById(i).Rewards.Count > 0)
                    return i;

            return 0;
        }
    }
}