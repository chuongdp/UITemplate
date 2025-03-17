namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using UnityEngine.Scripting;

    public class UnityTemplateIAPOwnerPackControllerData : IUnityTemplateControllerData
    {
        private readonly UnityTemplateIAPOwnerPackData UnityTemplateIAPOwnerPackData;

        [Preserve]
        public UnityTemplateIAPOwnerPackControllerData(UnityTemplateIAPOwnerPackData UnityTemplateIAPOwnerPackData)
        {
            this.UnityTemplateIAPOwnerPackData = UnityTemplateIAPOwnerPackData;
        }

        public void AddPack(string packId)
        {
            if (this.UnityTemplateIAPOwnerPackData.OwnedPacks.Contains(packId)) return;
            this.UnityTemplateIAPOwnerPackData.OwnedPacks.Add(packId);
        }

        public bool IsOwnerPack(string packId)
        {
            return this.UnityTemplateIAPOwnerPackData.OwnedPacks.Contains(packId);
        }
    }
}