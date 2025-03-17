namespace HyperGames.UnityTemplate.UnityTemplate.Creative.Cheat
{
    using GameFoundation.DI;
    using GameFoundation.Scripts.AssetLibrary;
    using UnityEngine.Scripting;

    public class HyperGamesCheatGenerate : IInitializable
    {
        private readonly IGameAssets gameAssets;

        [Preserve]
        public HyperGamesCheatGenerate(IGameAssets gameAssets)
        {
            this.gameAssets = gameAssets;
        }

        public void Initialize()
        {
            this.gameAssets.InstantiateAsync(nameof(HyperGamesCheatView), default, default);
        }
    }
}