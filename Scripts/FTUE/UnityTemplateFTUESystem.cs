namespace HyperGames.UnityTemplate.UnityTemplate.FTUE
{
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.Conditions;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.Signal;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Extension;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateFTUESystem : IInitializable
    {
        #region inject

        private readonly SignalBus                    signalBus;
        private readonly UnityTemplateFTUEDataController UnityTemplateFtueDataController;
        private readonly UnityTemplateFTUEBlueprint      unityTemplateFtueBlueprint;
        private readonly UnityTemplateFTUEController     UnityTemplateFtueController;
        private readonly IScreenManager               screenManager;

        #endregion

        private Dictionary<string, IFtueCondition>      IDToFtueConditions         { get; }
        private Dictionary<string, HashSet<GameObject>> StepIdToEnableGameObjects  { get; } = new(); //Use to enable the UI follow user's FTUE
        private Dictionary<string, HashSet<GameObject>> StepIdToDisableGameObjects { get; } = new(); //Use to disable the UI follow user's FTUE

        [Preserve]
        public UnityTemplateFTUESystem(
            SignalBus                    signalBus,
            UnityTemplateFTUEDataController UnityTemplateFtueDataController,
            UnityTemplateFTUEBlueprint      unityTemplateFtueBlueprint,
            UnityTemplateFTUEController     UnityTemplateFtueController,
            IScreenManager               screenManager,
            List<IFtueCondition>         ftueConditions
        )
        {
            this.signalBus                    = signalBus;
            this.UnityTemplateFtueDataController = UnityTemplateFtueDataController;
            this.unityTemplateFtueBlueprint      = unityTemplateFtueBlueprint;
            this.UnityTemplateFtueController     = UnityTemplateFtueController;
            this.screenManager                = screenManager;
            this.IDToFtueConditions           = ftueConditions.ToDictionary(condition => condition.Id, condition => condition);
        }

        public void Initialize()
        {
            this.signalBus.Subscribe<FTUETriggerSignal>(this.OnTriggerFTUE);
            this.signalBus.Subscribe<FTUEButtonClickSignal>(this.OnFTUEStepFinishedHandler);
            this.signalBus.Subscribe<FTUEDoActionSignal>(this.OnFTUEStepFinishedHandler);
        }

        //TODO : need to refactor for contunious FTUE
        public void OnFTUEStepFinishedHandler(IHaveStepId obj)
        {
            this.UnityTemplateFtueDataController.CompleteStep(obj.StepId);
            var disableObjectSet = this.StepIdToDisableGameObjects.GetOrAdd(obj.StepId, () => new HashSet<GameObject>());
            foreach (var gameObject in disableObjectSet) gameObject.SetActive(false);
            this.UnityTemplateFtueController.DoDeactiveFTUE(obj.StepId);
            var nextStepId = this.unityTemplateFtueBlueprint[obj.StepId].NextStepId;
            if (!nextStepId.IsNullOrEmpty()) this.OnTriggerFTUE(new(nextStepId));
        }

        public void RegisterEnableObjectToStepId(GameObject gameObject, string stepId)
        {
            var objectSet = this.StepIdToEnableGameObjects.GetOrAdd(stepId, () => new HashSet<GameObject>());
            objectSet.Add(gameObject);
            //In the case the game object in the initialized screen
            gameObject.SetActive(this.UnityTemplateFtueDataController.IsFinishedStep(stepId) || this.IsFTUEActiveAble(stepId));
        }

        public void RegisterDisableObjectToStepId(GameObject gameObject, string stepId)
        {
            gameObject.SetActive(false);
            var objectSet = this.StepIdToDisableGameObjects.GetOrAdd(stepId, () => new HashSet<GameObject>());
            objectSet.Add(gameObject);
        }

        private void OnTriggerFTUE(FTUETriggerSignal obj)
        {
            var stepId = obj.StepId;

            if (stepId.IsNullOrEmpty()) return;
            if (this.UnityTemplateFtueController.ThereIsFTUEActive()) return;

            var enableObjectSet = this.StepIdToEnableGameObjects.GetOrAdd(stepId, () => new HashSet<GameObject>());
            if (!this.IsFTUEActiveAble(stepId))
            {
                foreach (var gameObject in enableObjectSet) gameObject.SetActive(this.UnityTemplateFtueDataController.IsFinishedStep(stepId));

                return;
            }

            foreach (var gameObject in enableObjectSet) gameObject.SetActive(true);

            var disableObjectSet = this.StepIdToDisableGameObjects.GetOrAdd(stepId, () => new HashSet<GameObject>());
            this.UnityTemplateFtueController.DoActiveFTUE(stepId, disableObjectSet);
        }

        public bool IsFTUEActiveAble(string stepId)
        {
            if (this.UnityTemplateFtueDataController.IsFinishedStep(stepId)) return false;

            if (this.unityTemplateFtueBlueprint.GetDataById(stepId).RequireTriggerComplete.Any(stepId => !this.UnityTemplateFtueDataController.IsFinishedStep(stepId))) return false;

            var requireConditions = this.unityTemplateFtueBlueprint.GetDataById(stepId).GetRequireCondition();

            if (requireConditions != null && !requireConditions.All(requireCondition => this.IDToFtueConditions[requireCondition.RequireId].IsPassedCondition(requireCondition.ConditionDetail)))
                return
                    false;

            return true;
        }

        public bool IsAnyFtueActive()
        {
            return this.IsAnyFtueActive(this.screenManager.CurrentActiveScreen.Value);
        }

        public bool IsAnyFtueActive(IScreenPresenter screenPresenter)
        {
            var currentScreen = screenPresenter.GetType().Name;

            foreach (var stepBlueprintRecord in this.unityTemplateFtueBlueprint.Values)
            {
                if (!currentScreen.Equals(stepBlueprintRecord.ScreenLocation)) continue;
                if (!this.IsFTUEActiveAble(stepBlueprintRecord.Id)) continue;

                return true;
            }

            return false;
        }
    }
}