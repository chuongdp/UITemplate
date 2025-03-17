namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Utils
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class CreativeAttribute : Attribute
    {
        public bool HideOnCreative { get; set; } = true;
    }
}