namespace HyperGames.UnityTemplate.UnityTemplate.FTUE.Conditions
{
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;

    public class FTUEPassedLevelConditionModel
    {
        public int Level;
    }

    public class FTUEPassedLevelCondition : FtueCondition<FTUEPassedLevelConditionModel>
    {
        #region inject

        private readonly UnityTemplateLevelDataController UnityTemplateLevelDataController;

        #endregion

        public FTUEPassedLevelCondition(UnityTemplateLevelDataController UnityTemplateLevelDataController)
        {
            this.UnityTemplateLevelDataController = UnityTemplateLevelDataController;
        }

        public override string Id => "passed_level";

        protected override bool IsPassedCondition(FTUEPassedLevelConditionModel data)
        {
            return this.UnityTemplateLevelDataController.CurrentLevel >= data.Level;
        }
    }
}