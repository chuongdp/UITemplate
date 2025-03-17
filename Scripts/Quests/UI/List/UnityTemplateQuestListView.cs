namespace HyperGames.UnityTemplate.Quests.UI
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using HyperGames.UnityTemplate.Quests.Data;
    using UnityEngine;

    public class UnityTemplateQuestListModel
    {
        public ReadOnlyCollection<UnityTemplateQuestController> Quests { get; }

        public UnityTemplateQuestListModel(IEnumerable<UnityTemplateQuestController> quests)
        {
            this.Quests = quests.ToReadOnlyCollection();
        }
    }

    public class UnityTemplateQuestListView : MonoBehaviour
    {
        [SerializeField] private UnityTemplateQuestListItemAdapter listItemAdapter;

        public UnityTemplateQuestPopupPresenter Parent { get; set; }

        public UnityTemplateQuestListModel Model { get; set; }

        public void BindData()
        {
            var models = this.Model.Quests
                .Where(quest => quest.Progress.Status.HasFlag(QuestStatus.Shown))
                .OrderByDescending(quest => quest.Progress.Status is QuestStatus.NotCollected)
                .ThenByDescending(quest => quest.Progress.Status is QuestStatus.NotCompleted)
                .Select(quest => new UnityTemplateQuestListItemModel(this.Parent, quest))
                .ToList();
            this.listItemAdapter.InitItemAdapter(models).Forget();
        }
    }
}