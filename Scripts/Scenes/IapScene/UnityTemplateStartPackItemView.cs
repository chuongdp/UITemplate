namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.IapScene
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using GameFoundation.Scripts.UIModule.Utilities.LoadImage;
    using HyperGames.UnityTemplate.UnityTemplate.Extension;
    using TMPro;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class UnityTemplateStartPackItemModel
    {
        public string IconAddress { get; set; }
        public string Value       { get; set; }
    }

    public class UnityTemplateStartPackItemView : TViewMono
    {
        public Image           imgIcon;
        public TextMeshProUGUI txtValue;
    }

    public class UnityTemplateStartPackItemPresenter : BaseUIItemPresenter<UnityTemplateStartPackItemView, UnityTemplateStartPackItemModel>
    {
        private readonly LoadImageHelper loadImageHelper;

        [Preserve]
        public UnityTemplateStartPackItemPresenter(IGameAssets gameAssets, LoadImageHelper loadImageHelper) : base(gameAssets)
        {
            this.loadImageHelper = loadImageHelper;
        }

        public override async void BindData(UnityTemplateStartPackItemModel param)
        {
            if (!param.IconAddress.IsNullOrEmpty()) this.View.imgIcon.sprite = await this.loadImageHelper.LoadLocalSprite(param.IconAddress);

            this.View.txtValue.text = $"{param.Value}";
        }
    }
}