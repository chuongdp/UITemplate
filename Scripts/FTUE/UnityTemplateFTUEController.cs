namespace HyperGames.UnityTemplate.UnityTemplate.FTUE
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.UnityTemplate.FTUE.Signal;
    using HyperGames.UnityTemplate.UnityTemplate.Scripts.Services.Highlight;
    using HyperGames.UnityTemplate.UnityTemplate.Extension;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class UnityTemplateFTUEController
    {
        private readonly HighlightController     highlightController;
        private readonly UnityTemplateFTUEBlueprint unityTemplateFtueBlueprint;
        private readonly SignalBus               signalBus;
        private          string                  currentActiveStepId;

        [Preserve]
        public UnityTemplateFTUEController(HighlightController highlightController, UnityTemplateFTUEBlueprint unityTemplateFtueBlueprint, SignalBus signalBus)
        {
            this.highlightController     = highlightController;
            this.unityTemplateFtueBlueprint = unityTemplateFtueBlueprint;
            this.signalBus               = signalBus;
        }

        public bool ThereIsFTUEActive()
        {
            return !string.IsNullOrEmpty(this.currentActiveStepId);
        }

        public void DoDeactiveFTUE(string stepId)
        {
            if (stepId.IsNullOrEmpty() || !stepId.Equals(this.currentActiveStepId)) return;
            this.currentActiveStepId = null;

            var record = this.unityTemplateFtueBlueprint.GetDataById(stepId);

            if (string.IsNullOrEmpty(record.HighLightPath)) return;
            this.highlightController.TurnOffHighlight();
        }

        public void DoActiveFTUE(string stepId, HashSet<GameObject> disableObjectSet)
        {
            this.currentActiveStepId = stepId;
            foreach (var disableObject in disableObjectSet) disableObject.SetActive(true);
            this.SetHighlight(stepId).Forget();
        }

        private async UniTask SetHighlight(string stepId)
        {
            var record = this.unityTemplateFtueBlueprint.GetDataById(stepId);
            await this.highlightController.SetHighlight(record.HighLightPath,
                record.ButtonCanClick,
                () =>
                {
                    this.signalBus.Fire(new FTUEButtonClickSignal(stepId));
                });
            this.highlightController.ConfigHand(TypeConfigHand.AllAppear, record.HandSizeDelta, record.Radius, record.HandAnchor, record.HandRotation);
        }
    }
}