namespace HyperGames.UnityTemplate.UnityTemplate.Events.Racing
{
    using GameFoundation.DI;
    using HyperGames.HyperCasual.GamePlay.Models;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class UnityTemplateRacingRowView : MonoBehaviour
    {
        private UnityTemplateEventRacingDataController UnityTemplateEventRacingDataController;

        public TMP_Text nameText;
        public TMP_Text scoreText;
        public Slider   progressSlider;
        public Image    yourIcon;
        public Image    flagImage;
        public Button   buttonChest;

        [Header("Animation")] public Animator animatorButtonChest;

        protected bool IsPlayer;

        protected virtual void Awake()
        {
            var container = this.GetCurrentContainer();
            this.UnityTemplateEventRacingDataController = container.Resolve<UnityTemplateEventRacingDataController>();
        }

        public virtual void InitView(UnityTemplateRacingPlayerData playerData, int indexPlayer, UnityAction onOpenChest = null)
        {
            this.IsPlayer            = this.UnityTemplateEventRacingDataController.IsPlayer(indexPlayer);
            this.nameText.text       = playerData.Name;
            this.scoreText.text      = playerData.Score.ToString();
            this.flagImage.sprite    = this.UnityTemplateEventRacingDataController.GetCountryFlagSprite(playerData.CountryCode);
            this.buttonChest.enabled = this.IsPlayer;
            this.buttonChest.onClick.AddListener(onOpenChest);
        }

        public virtual void CheckStatus()
        {
            var isWin = this.UnityTemplateEventRacingDataController.RacingEventComplete();
            this.animatorButtonChest.enabled = isWin && this.IsPlayer;
        }
    }
}