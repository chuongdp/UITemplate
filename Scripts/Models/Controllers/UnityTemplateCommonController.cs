namespace HyperGames.UnityTemplate.UnityTemplate.Models.Controllers
{
    using HyperGames.UnityTemplate.UnityTemplate.Models.LocalDatas;
    using UnityEngine.Scripting;

    public class UnityTemplateCommonController : IUnityTemplateControllerData
    {
        private readonly UnityTemplateCommonData UnityTemplateCommonData;

        [Preserve]
        public UnityTemplateCommonController(UnityTemplateCommonData UnityTemplateCommonData)
        {
            this.UnityTemplateCommonData = UnityTemplateCommonData;
        }

        public bool IsFirstTimeOpenGame => this.UnityTemplateCommonData.IsFirstTimeOpenGame;

        public void ChangeGameIsAlreadyOpened()
        {
            this.UnityTemplateCommonData.IsFirstTimeOpenGame = false;
        }
    }
}