namespace HyperGames.UnityTemplate.Quests
{
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.Quests.Data;
    using UnityEngine.Scripting;

    public class UnityTemplateQuestManager : IInitializable, ITickable
    {
        private readonly IDependencyContainer     container;
        private readonly UnityTemplateQuestBlueprint questBlueprint;
        private readonly UnityTemplateQuestProgress  questProgress;

        private readonly Dictionary<string, UnityTemplateQuestController> controllers = new();

        [Preserve]
        public UnityTemplateQuestManager(
            IDependencyContainer     container,
            UnityTemplateQuestBlueprint questBlueprint,
            UnityTemplateQuestProgress  questProgress
        )
        {
            this.container      = container;
            this.questBlueprint = questBlueprint;
            this.questProgress  = questProgress;
        }

        void IInitializable.Initialize()
        {
            this.questBlueprint.Keys.ForEach(this.InstantiateHandler);
        }

        void ITickable.Tick()
        {
            foreach (var (id, controller) in this.controllers.ToArray())
            {
                if (!controller.CanBeReset()) continue;
                controller.Dispose();
                this.controllers.Remove(id);
                this.questProgress.Storage.Remove(id);
                this.InstantiateHandler(id);
            }
            foreach (var controller in this.controllers.Values) controller.UpdateStatus();
        }

        public UnityTemplateQuestController GetController(string id)
        {
            return this.controllers[id];
        }

        public IEnumerable<UnityTemplateQuestController> GetAllControllers()
        {
            return this.controllers.Values;
        }

        private void InstantiateHandler(string id)
        {
            var record     = this.questBlueprint[id];
            var progress   = this.questProgress.Storage.GetOrAdd(record.Id, () => new(record));
            var controller = this.container.Instantiate<UnityTemplateQuestController>();
            controller.Record   = record;
            controller.Progress = progress;
            controller.Initialize();
            this.controllers.Add(id, controller);
        }
    }
}