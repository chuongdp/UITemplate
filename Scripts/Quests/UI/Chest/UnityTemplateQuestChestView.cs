namespace HyperGames.UnityTemplate.Quests.UI
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using HyperGames.UnityTemplate.Quests.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnityTemplateQuestChestModel
    {
        public ReadOnlyCollection<UnityTemplateQuestController> Quests { get; }

        public UnityTemplateQuestChestModel(IEnumerable<UnityTemplateQuestController> quests)
        {
            this.Quests = quests.ToReadOnlyCollection();
        }
    }

    public class UnityTemplateQuestChestView : MonoBehaviour
    {
        [SerializeField] private Transform                    itemViewContainer;
        [SerializeField] private UnityTemplateQuestChestItemView itemViewPrefab;
        [SerializeField] private Slider                       sld;

        private ObjectPoolManager objectPoolManager;

        private void Awake()
        {
            var container = this.GetCurrentContainer();
            this.objectPoolManager = container.Resolve<ObjectPoolManager>();
        }

        public UnityTemplateQuestChestModel Model { get; set; }

        public void BindData()
        {
            if (this.Model.Quests.Count == 0)
            {
                this.gameObject.SetActive(false);
                return;
            }
            this.gameObject.SetActive(true);
            this.Model.Quests.ForEach(quest =>
            {
                var itemView = this.objectPoolManager.Spawn(this.itemViewPrefab, this.itemViewContainer);
                itemView.Model = new(quest);
                itemView.BindData();
            });
            if (this.Model.Quests.All(quest => quest.Progress.Status.HasFlag(QuestStatus.Completed)))
            {
                this.sld.value = 1;
            }
            else
            {
                var progressHandler = this.Model.Quests.Last().GetCompleteProgressHandlers().Last();
                this.sld.value = progressHandler.CurrentProgress / progressHandler.MaxProgress;
            }
        }

        public void Dispose()
        {
            this.objectPoolManager.RecycleAll(this.itemViewPrefab);
        }
    }
}