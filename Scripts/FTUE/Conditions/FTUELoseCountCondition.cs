namespace HyperGames.UnityTemplate.UnityTemplate.FTUE.Conditions
{
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;

    public class FTUELoseCountModel
    {
        public int Count;
    }

    public class FTUELoseCountCondition : FtueCondition<FTUELoseCountModel>
    {
        #region inject

        private readonly UnityTemplateLevelDataController UnityTemplateLevelDataController;

        #endregion

        public override string Id => "lose_count";

        public FTUELoseCountCondition(UnityTemplateLevelDataController UnityTemplateLevelDataController)
        {
            this.UnityTemplateLevelDataController = UnityTemplateLevelDataController;
        }

        protected override bool IsPassedCondition(FTUELoseCountModel data)
        {
            return this.UnityTemplateLevelDataController.TotalLose == data.Count;
        }
    }
}