namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Pack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.DailyReward.Item;
    using HyperGames.UnityTemplate.Scripts.Blueprints;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    public class UnityTemplateDailyRewardPackModel
    {
        public Action<UnityTemplateDailyRewardPackPresenter> OnClick           { get; set; }
        public RewardStatus                               RewardStatus      { get; set; }
        public UnityTemplateDailyRewardRecord                DailyRewardRecord { get; set; }
        public bool                                       IsGetWithAds      { get; set; }

        public UnityTemplateDailyRewardPackModel(UnityTemplateDailyRewardRecord dailyRewardRecord, RewardStatus rewardStatus, Action<UnityTemplateDailyRewardPackPresenter> click)
        {
            this.DailyRewardRecord = dailyRewardRecord;
            this.RewardStatus      = rewardStatus;
            this.OnClick           = click;
        }
    }

    public class UnityTemplateDailyRewardPackView : TViewMono
    {
        [BoxGroup("View")] [SerializeField] private UnityTemplateDailyRewardItemAdapter dailyRewardItemAdapter;
        [BoxGroup("View")] [SerializeField] private Button                           btnClaim;
        [BoxGroup("View")] [SerializeField] private TextMeshProUGUI                  txtDayLabel;
        [BoxGroup("View")] [SerializeField] private GameObject                       objClaimed;
        [BoxGroup("View")] [SerializeField] private GameObject                       objClaimedCheckIcon;
        [BoxGroup("View")] [SerializeField] private GameObject                       objClaimByAds;

        [BoxGroup("View/Background")] [SerializeField] private Image imgBackground;

        [BoxGroup("View/Background")] [SerializeField] private Sprite sprBgNormal;

        [BoxGroup("View/Background")] [SerializeField] private Sprite sprBgCurrentDay;

        [BoxGroup("View/PackImage")] [SerializeField] private Image packImg;

        [BoxGroup("Feature/CoverPack")] [SerializeField] private bool coverPackWhenAllItemsHidden;

        [BoxGroup("Feature/CoverPack")] [SerializeField] private Image coverImg;

        public UnityTemplateDailyRewardItemAdapter DailyRewardItemAdapter      => this.dailyRewardItemAdapter;
        public Button                           BtnClaim                    => this.btnClaim;
        public TextMeshProUGUI                  TxtDayLabel                 => this.txtDayLabel;
        public GameObject                       ObjClaimed                  => this.objClaimed;
        public GameObject                       ObjClaimedCheckIcon         => this.objClaimedCheckIcon;
        public GameObject                       ObjClaimByAds               => this.objClaimByAds;
        public Image                            ImgBackground               => this.imgBackground;
        public Sprite                           SprBgNormal                 => this.sprBgNormal;
        public Sprite                           SprBgCurrentDay             => this.sprBgCurrentDay;
        public Image                            PackImg                     => this.packImg;
        public bool                             CoverPackWhenAllItemsHidden => this.coverPackWhenAllItemsHidden;
        public Image                            CoverImg                    => this.coverImg;
        public Action                           OnClickClaimButton          { get; set; }

        private void Awake()
        {
            if (this.btnClaim != null)
                this.btnClaim.onClick.AddListener(() =>
                {
                    this.OnClickClaimButton?.Invoke();
                });
        }
    }

    public class UnityTemplateDailyRewardPackPresenter : BaseUIItemPresenter<UnityTemplateDailyRewardPackView, UnityTemplateDailyRewardPackModel>
    {
        public UnityTemplateDailyRewardPackModel Model { get; set; }

        #region inject

        private readonly UnityTemplateDailyRewardPackViewHelper dailyRewardPackViewHelper;

        #endregion

        [Preserve]
        public UnityTemplateDailyRewardPackPresenter(
            IGameAssets                         gameAssets,
            UnityTemplateDailyRewardPackViewHelper dailyRewardPackViewHelper
        ) : base(gameAssets)
        {
            this.dailyRewardPackViewHelper = dailyRewardPackViewHelper;
        }

        public override void BindData(UnityTemplateDailyRewardPackModel param)
        {
            this.Model = param;

            this.dailyRewardPackViewHelper.BindDataItem(param, this.View, this);

            if (!string.IsNullOrEmpty(this.Model.DailyRewardRecord.PackImage)) return;
            var models = param.DailyRewardRecord.Reward.Values
                .Select(item => new UnityTemplateDailyRewardItemModel
                {
                    DailyRewardRecord = this.Model.DailyRewardRecord,
                    RewardRecord      = item,
                    RewardStatus      = this.Model.RewardStatus,
                    IsGetWithAds      = this.Model.IsGetWithAds,
                })
                .ToList();
            this.View.DailyRewardItemAdapter.InitItemAdapter(models).Forget();

            this.CoverPack(models);
        }

        private void CoverPack(IEnumerable<UnityTemplateDailyRewardItemModel> itemModels)
        {
            if (!this.View.CoverPackWhenAllItemsHidden) return;

            if (this.Model.RewardStatus == RewardStatus.Claimed)
            {
                this.View.CoverImg.gameObject.SetActive(false);
                return;
            }

            var isAllItemHidden = itemModels.All(im => !im.RewardRecord.SpoilReward);
            this.View.CoverImg.gameObject.SetActive(isAllItemHidden);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.dailyRewardPackViewHelper.DisposeItem(this);
        }

        public void ClaimReward()
        {
            this.dailyRewardPackViewHelper.OnClaimReward(this);
        }
    }
}