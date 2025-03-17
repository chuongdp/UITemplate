namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Item
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateDailyRewardItemModel
    {
        public UnityTemplateDailyRewardRecord DailyRewardRecord { get; set; }
        public UnityTemplateRewardRecord      RewardRecord      { get; set; }
        public RewardStatus                RewardStatus      { get; set; }
        public bool                        IsGetWithAds      { get; set; }
    }

    public class UnityTemplateDailyRewardItemView : TViewMono
    {
        [BoxGroup("Reward")] [SerializeField] private GameObject      objReward;
        [BoxGroup("Reward")] [SerializeField] private Image           imgReward;
        [BoxGroup("Reward")] [SerializeField] private TextMeshProUGUI txtValue;
        [BoxGroup("Reward")] [SerializeField] private GameObject      objLock;

        public GameObject      ObjReward => this.objReward;
        public Image           ImgReward => this.imgReward;
        public TextMeshProUGUI TxtValue  => this.txtValue;
        public GameObject      ObjLock   => this.objLock;

        public void UpdateIconRectTransform(Vector2? position, Vector2? size)
        {
            var rectTransform = this.objReward.GetComponent<RectTransform>();

            if (position is { }) rectTransform.anchoredPosition = position.Value;

            if (size is { }) rectTransform.sizeDelta = size.Value;
        }
    }

    public class UnityTemplateDailyRewardItemPresenter : BaseUIItemPresenter<UnityTemplateDailyRewardItemView, UnityTemplateDailyRewardItemModel>
    {
        public UnityTemplateDailyRewardItemModel Model { get; set; }

        #region inject

        private readonly UnityTemplateDailyRewardItemViewHelper dailyRewardItemViewHelper;

        #endregion

        [Preserve]
        public UnityTemplateDailyRewardItemPresenter(IGameAssets gameAssets, UnityTemplateDailyRewardItemViewHelper dailyRewardItemViewHelper) : base(gameAssets)
        {
            this.dailyRewardItemViewHelper = dailyRewardItemViewHelper;
        }

        public override void BindData(UnityTemplateDailyRewardItemModel param)
        {
            this.Model = param;
            this.dailyRewardItemViewHelper.BindDataItem(param, this.View, this);
        }

        public override void Dispose()
        {
            this.dailyRewardItemViewHelper.DisposeItem(this);
        }
    }
}