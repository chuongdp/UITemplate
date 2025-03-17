namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using UnityEngine.Scripting;

    public class UnityTemplateFTUEDataController : IUnityTemplateControllerData
    {
        #region inject

        private readonly UnityTemplateFTUEBlueprint unityTemplateFtueBlueprint;
        private readonly UnityTemplateFTUEData      templateFtueData;

        #endregion

        [Preserve]
        public UnityTemplateFTUEDataController(UnityTemplateFTUEData templateFtueData, UnityTemplateFTUEBlueprint unityTemplateFtueBlueprint)
        {
            this.templateFtueData        = templateFtueData;
            this.unityTemplateFtueBlueprint = unityTemplateFtueBlueprint;
        }

        public bool IsFinishedStep(string stepId)
        {
            return this.templateFtueData.FinishedStep.Contains(stepId);
        }

        public void CompleteStep(string stepId)
        {
            if (this.templateFtueData.FinishedStep.Contains(stepId)) return;
            this.templateFtueData.FinishedStep.Add(stepId);
            foreach (var previousStep in this.unityTemplateFtueBlueprint.GetDataById(stepId).PreviousSteps) this.CompleteStep(previousStep);
        }
    }
}