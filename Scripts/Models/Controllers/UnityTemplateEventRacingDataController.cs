namespace HyperGames.HyperCasual.GamePlay.Models
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.DI;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.Configs.GameEvents;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Services.CountryFlags.CountryFlags.Scripts;
    using ServiceImplementation.FireBaseRemoteConfig;
    using HyperGames.UnityTemplate.UnityTemplate.Extension;
    using UnityEngine;
    using UnityEngine.Scripting;
    using Utilities.Utils;
    using Random = UnityEngine.Random;

    public class UnityTemplateEventRacingDataController : IUnityTemplateControllerData, IInitializable
    {
        private const string RacingEventMaxScoreKey = "racing_event_max_score";

        public const int TotalRacingPlayer = 5;

        private const string CountryFlagsPrefab = "CountryFlags";

        #region inject

        private readonly UnityTemplateEventRacingData         UnityTemplateEventRacingData;
        private readonly IGameAssets                       gameAssets;
        private readonly UnityTemplateInventoryDataController UnityTemplateInventoryDataController;
        private readonly SignalBus                         signalBus;
        private readonly IRemoteConfig                     remoteConfig;
        private readonly GameFeaturesSetting               gameFeaturesSetting;

        #endregion

        private CountryFlags CountryFlags
        {
            get
            {
                if (this.countryFlags == null)
                    this.countryFlags = this.gameAssets.LoadAssetAsync<GameObject>(CountryFlagsPrefab).WaitForCompletion()
                        .Spawn().GetComponent<CountryFlags>();

                return this.countryFlags;
            }
            set => this.countryFlags = value;
        }

        private CountryFlags countryFlags;
        public  int          RacingScoreMax { get; set; }

        [Preserve]
        public UnityTemplateEventRacingDataController(
            UnityTemplateEventRacingData         UnityTemplateEventRacingData,
            IGameAssets                       gameAssets,
            UnityTemplateInventoryDataController UnityTemplateInventoryDataController,
            SignalBus                         signalBus,
            IRemoteConfig                     remoteConfig,
            GameFeaturesSetting               gameFeaturesSetting
        )
        {
            this.UnityTemplateEventRacingData         = UnityTemplateEventRacingData;
            this.gameAssets                        = gameAssets;
            this.UnityTemplateInventoryDataController = UnityTemplateInventoryDataController;
            this.signalBus                         = signalBus;
            this.remoteConfig                      = remoteConfig;
            this.gameFeaturesSetting               = gameFeaturesSetting;
        }

        public int      YourOldShowScore => this.UnityTemplateEventRacingData.YourOldShowScore;
        public int      YourNewScore     => this.UnityTemplateInventoryDataController.GetCurrencyValue(this.gameFeaturesSetting.RacingConfig.RacingCurrency);
        public int      YourIndex        => this.UnityTemplateEventRacingData.yourIndex;
        public long     RemainSecond     => (long)(this.UnityTemplateEventRacingData.endDate - DateTime.Now).TotalSeconds;
        public DateTime StartDate        => this.UnityTemplateEventRacingData.startDate;

        public DateTime EndDate
        {
            get
            {
                if (this.UnityTemplateEventRacingData.endDate == DateTime.MinValue) this.UnityTemplateEventRacingData.endDate = this.UnityTemplateEventRacingData.startDate.AddDays(this.gameFeaturesSetting.RacingConfig.RacingDay - Random.Range(0, 1.5f));

                return this.UnityTemplateEventRacingData.endDate;
            }
        }

        public float RacingMaxProgression => this.gameFeaturesSetting.racingConfig.RacingMaxProgressionPercent;

        public void UpdateUserOldShowScore()
        {
            this.UnityTemplateEventRacingData.YourOldShowScore =
                this.UnityTemplateInventoryDataController.GetCurrencyValue(this.gameFeaturesSetting.RacingConfig.RacingCurrency);
        }

        public void Initialize()
        {
            this.signalBus.Subscribe<RemoteConfigFetchedSucceededSignal>(this.OnFetchSucceedHandler);
        }

        private void OnFetchSucceedHandler(RemoteConfigFetchedSucceededSignal obj)
        {
            this.RacingScoreMax = this.remoteConfig.GetRemoteConfigIntValue(RacingEventMaxScoreKey, this.gameFeaturesSetting.racingConfig.RacingScoreMax);
        }

        public void AddPlayScore(int addedScore)
        {
            var yourIndex = this.UnityTemplateEventRacingData.yourIndex;
            this.UnityTemplateEventRacingData.playerIndexToData[yourIndex].Score += addedScore;
        }

        private void AddScore(int playIndex, int addedScore)
        {
            this.UnityTemplateEventRacingData.playerIndexToData[playIndex].Score += addedScore;
        }

        public Sprite GetCountryFlagSprite(string countryCode)
        {
            return this.CountryFlags.GetFlag(countryCode);
        }

        public UnityTemplateRacingPlayerData GetPlayerData(int playIndex)
        {
            var isYou = this.IsPlayer(playIndex);
            var racingPlayerData = this.UnityTemplateEventRacingData.playerIndexToData.GetOrAdd(playIndex,
                () =>
                    new()
                    {
                        Name  = isYou ? "You" : NVJOBNameGen.GiveAName(Random.Range(1, 8)),
                        Score = 0,
                        CountryCode =
                            isYou ? RegionHelper.GetCountryCodeByDeviceLang() : this.CountryFlags.RandomCountryCode(),
                        IconAddressable = this.gameFeaturesSetting.RacingConfig.IconAddressableSet.PickRandom(),
                    });

            if (racingPlayerData.IconAddressable.IsNullOrEmpty()) racingPlayerData.IconAddressable = this.gameFeaturesSetting.RacingConfig.IconAddressableSet.PickRandom();

            if (isYou) racingPlayerData.Score = this.UnityTemplateInventoryDataController.GetCurrencyValue(this.gameFeaturesSetting.RacingConfig.RacingCurrency);

            return racingPlayerData;
        }

        //Simulate score for all player except yourIndex and add to playerIndexToScore with to make them reach GameEventRacingValue.RacingMaxProgressionPercent of total score as max
        //The score of players will depend the time from lastRandomTime to now the gameEventRacingData.startDate (starting time of event) and gameEventRacingData.endDate (ending time of event
        public Dictionary<int, (int, int)> SimulatePlayerScore()
        {
            var maxScore              = this.RacingScoreMax * this.RacingMaxProgression;
            var playIndexToAddedScore = new Dictionary<int, (int, int)>();

            var currentTime = DateTime.Now;

            foreach (var (playerIndex, racingPlayerData) in this.UnityTemplateEventRacingData.playerIndexToData)
            {
                if (playerIndex == this.UnityTemplateEventRacingData.yourIndex) continue;

                //calculate input data
                var totalSecondsFromLastSimulation =
                    (currentTime - this.UnityTemplateEventRacingData.lastRandomTime).TotalSeconds;
                var totalSecondsUntilEndEventFromLastSimulation =
                    (this.EndDate - currentTime).TotalSeconds;
                var maxRandomScore = maxScore - racingPlayerData.Score;

                //calculate random score
                var randomAddingScore = (int)(totalSecondsFromLastSimulation / totalSecondsUntilEndEventFromLastSimulation * maxRandomScore);
                randomAddingScore = (int)(randomAddingScore * Random.Range(0.3f, 1.1f));
                playIndexToAddedScore.Add(playerIndex,
                    (racingPlayerData.Score, racingPlayerData.Score + randomAddingScore));
                this.AddScore(playerIndex, randomAddingScore);
            }

            this.UnityTemplateEventRacingData.lastRandomTime = currentTime;
            return playIndexToAddedScore;
        }

        public bool RacingEventComplete()
        {
            return this.YourNewScore >= this.RacingScoreMax;
        }

        public bool IsPlayer(int playerIndex)
        {
            return playerIndex == this.UnityTemplateEventRacingData.yourIndex;
        }

        public bool ChangeStatusClaimItem(int idPlayer, bool status = true)
        {
            return this.UnityTemplateEventRacingData.playerIndexToData[idPlayer].IsClaimItem = status;
        }

        public bool GetStatusClaimItem(int idPlayer = -1)
        {
            idPlayer = idPlayer == -1 ? this.YourIndex : idPlayer;

            if (this.UnityTemplateEventRacingData.playerIndexToData.Count == 0) return false;
            return this.UnityTemplateEventRacingData.playerIndexToData[idPlayer].IsClaimItem;
        }
    }
}