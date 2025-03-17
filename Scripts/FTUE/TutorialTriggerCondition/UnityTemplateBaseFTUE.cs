// namespace HyperGames.UnityTemplate.UnityTemplate.FTUE.TutorialTriggerCondition
// {
//     using System;
//     using GameFoundation.Scripts.Utilities.LogService;
//     using HyperGames.UnityTemplate.UnityTemplate.Blueprints;
//     using HyperGames.UnityTemplate.UnityTemplate.Extension;
//     using HyperGames.UnityTemplate.UnityTemplate.FTUE.Signal;
//     using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
//     using Zenject;
//
//     public interface IUnityTemplateFTUE
//     {
//         string StepId { get; }
//         bool   IsPassedCondition();
//         void   Execute(string stepId);
//     }
//
//     public abstract class UnityTemplateFTUEStepBase : IUnityTemplateFTUE, IInitializable, IDisposable
//     {
//         protected readonly ILogService                  Logger;
//         protected readonly SignalBus                    SignalBus;
//         protected readonly UnityTemplateFTUEBlueprint      UnityTemplateFtueBlueprint;
//         private readonly   UnityTemplateFTUEControllerData UnityTemplateFtueControllerData;
//         protected readonly UnityTemplateFTUEController     UnityTemplateFtueController;
//         public abstract    string                       StepId { get; }
//
//         protected UnityTemplateFTUEStepBase(ILogService logger, SignalBus signalBus, UnityTemplateFTUEBlueprint UnityTemplateFtueBlueprint, UnityTemplateFTUEControllerData UnityTemplateFtueControllerData,
//             UnityTemplateFTUEController UnityTemplateFtueController)
//         {
//             this.Logger                       = logger;
//             this.SignalBus                    = signalBus;
//             this.UnityTemplateFtueBlueprint      = UnityTemplateFtueBlueprint;
//             this.UnityTemplateFtueControllerData = UnityTemplateFtueControllerData;
//             this.UnityTemplateFtueController     = UnityTemplateFtueController;
//         }
//
//         public void Initialize() { this.SignalBus.Subscribe<FTUEButtonClickSignal>(this.OnFTUEButtonClick); }
//
//         public abstract bool IsPassedCondition();
//
//         public void Execute(string stepId)
//         {
//             this.TriggerStepId = stepId;
//             var canTrigger = this.IsPassedCondition();
//
//             this.UnityTemplateFtueController.SetTutorialStatus(canTrigger, stepId);
//         }
//
//         protected string TriggerStepId { get; set; }
//
//         protected void SaveCompleteStepToLocalData(string stepId) { this.UnityTemplateFtueControllerData.CompleteStep(stepId); }
//
//         protected virtual void OnFTUEButtonClick(FTUEButtonClickSignal obj)
//         {
//             this.UnityTemplateFtueController.SetTutorialStatus(false, obj.StepId);
//             this.UnityTemplateFtueControllerData.CompleteStep(obj.StepId);
//             var nextStepId = this.UnityTemplateFtueBlueprint[obj.StepId].NextStepId;
//
//             if (nextStepId.IsNullOrEmpty()) return;
//             this.SignalBus.Fire(new FTUETriggerSignal(nextStepId));
//         }
//
//         protected void AuoTriggerNextStep(string currentStepId)
//         {
//           
//         }
//
//         public void Dispose() { this.SignalBus.Unsubscribe<FTUEButtonClickSignal>(this.OnFTUEButtonClick); }
//     }
// }

