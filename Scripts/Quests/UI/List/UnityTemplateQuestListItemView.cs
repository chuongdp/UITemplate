namespace HyperGames.UnityTemplate.Quests.UI
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Signals;
    using HyperGames.UnityTemplate.Quests.Data;
    using HyperGames.UnityTemplate.UnityTemplate.Configs.GameEvents;
    using HyperGames.UnityTemplate.UnityTemplate.Quests.Signals;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateQuestListItemModel
    {
        public UnityTemplateQuestPopupPresenter Parent { get; }
        public UnityTemplateQuestController     Quest  { get; }

        public UnityTemplateQuestListItemModel(UnityTemplateQuestPopupPresenter parent, UnityTemplateQuestController quest)
        {
            this.Parent = parent;
            this.Quest  = quest;
        }
    }

    public class UnityTemplateQuestListItemView : MonoBehaviour, IUIView
    {
        // Left
        [field: SerializeField] public Image    ImgReward { get; private set; }
        [field: SerializeField] public TMP_Text TxtReward { get; private set; }
        [field: SerializeField] public TMP_Text TxtName   { get; private set; }
        [field: SerializeField] public TMP_Text TxtDesc   { get; private set; }

        // Right
        [field: SerializeField] public TMP_Text TxtProgress { get; private set; }
        [field: SerializeField] public Slider   SldProgress { get; private set; }
        [field: SerializeField] public Button   BtnGo       { get; private set; }
        [field: SerializeField] public Button   BtnClaim    { get; private set; }

        [field: SerializeField] public GameObject[] NormalObjects    { get; private set; }
        [field: SerializeField] public GameObject[] CompletedObjects { get; private set; }
        [field: SerializeField] public GameObject[] CollectedObjects { get; private set; }
    }

    public class UnityTemplateQuestListItemPresenter : BaseUIItemPresenter<UnityTemplateQuestListItemView, UnityTemplateQuestListItemModel>
    {
        private readonly IGameAssets         gameAssets;
        private readonly IAudioService       audioService;
        private readonly GameFeaturesSetting gameFeaturesSetting;
        private readonly SignalBus           signalBus;

        [Preserve]
        public UnityTemplateQuestListItemPresenter(
            IGameAssets         gameAssets,
            IAudioService       audioService,
            GameFeaturesSetting gameFeaturesSetting,
            SignalBus           signalBus
        ) : base(gameAssets)
        {
            this.gameAssets          = gameAssets;
            this.audioService        = audioService;
            this.gameFeaturesSetting = gameFeaturesSetting;
            this.signalBus           = signalBus;
        }

        private UnityTemplateQuestListItemModel Model { get; set; }

        public override void BindData(UnityTemplateQuestListItemModel model)
        {
            this.Model = model;

            this.InitView();

            this.signalBus.Subscribe<ClaimAllQuestSignal>(this.ClaimQuestItem);
            this.View.BtnGo.onClick.AddListener(this.OnClickGo);
            this.View.BtnClaim.onClick.AddListener(this.OnClickClaim);
        }

        private void InitView()
        {
            // Left
            var record = this.Model.Quest.Record;
            var reward = record.Rewards.Single();
            this.View.ImgReward.sprite = this.gameAssets.LoadAssetAsync<Sprite>(reward.Image).WaitForCompletion();
            this.View.TxtReward.text   = reward.Value.ToString();
            this.View.TxtName.text     = record.Name;
            this.View.TxtDesc.text     = record.Description;

            // Right
            var status = this.Model.Quest.Progress.Status;
            this.SetItemStatus(status);
        }

        private void SetItemStatus(QuestStatus status)
        {
            if (status.HasFlag(QuestStatus.Completed))
            {
                this.View.TxtProgress.text  = "Completed";
                this.View.SldProgress.value = 1;
            }
            else
            {
                var progressHandler = this.Model.Quest.GetCompleteProgressHandlers().Single();
                this.View.TxtProgress.text  = $"{progressHandler.CurrentProgress}/{progressHandler.MaxProgress}";
                this.View.SldProgress.value = progressHandler.CurrentProgress / progressHandler.MaxProgress;
            }

            this.View.NormalObjects.ForEach(obj => obj.SetActive(status is QuestStatus.NotCompleted));
            this.View.CompletedObjects.ForEach(obj => obj.SetActive(status is QuestStatus.NotCollected));
            this.View.CollectedObjects.ForEach(obj => obj.SetActive(status.HasFlag(QuestStatus.Collected)));
        }

        private void OnClickGo()
        {
            this.Model.Parent.CloseViewAsync().Forget();
        }

        private void OnClickClaim()
        {
            if (!string.IsNullOrEmpty(this.gameFeaturesSetting.QuestSystemConfig.questClaimSoundKey)) this.audioService.PlaySound(this.gameFeaturesSetting.QuestSystemConfig.questClaimSoundKey);
            this.ClaimQuestItem();
        }

        private async void ClaimQuestItem()
        {
            var newStatus = this.Model.Quest.Progress.Status | QuestStatus.Collected;
            this.SetItemStatus(newStatus);
            await this.Model.Quest.CollectReward(this.View.ImgReward.rectTransform)
                .ContinueWith(() => this.Model.Parent.Refresh());
        }

        public override void Dispose()
        {
            this.signalBus.Unsubscribe<ClaimAllQuestSignal>(this.ClaimQuestItem);
            this.View.BtnGo.onClick.RemoveListener(this.OnClickGo);
            this.View.BtnClaim.onClick.RemoveListener(this.OnClickClaim);
        }
    }
}