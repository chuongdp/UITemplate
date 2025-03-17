namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils
{
    using UnityEngine;

    public abstract class UnityTemplateFlyingAnimationView : MonoBehaviour
    {
        [SerializeField] private Transform targetFlyingAnimation;

        public          Transform TargetFlyingAnimation => this.targetFlyingAnimation ??= this.GetComponent<Transform>();
        public abstract string    CurrencyKey           { get; }
    }
}