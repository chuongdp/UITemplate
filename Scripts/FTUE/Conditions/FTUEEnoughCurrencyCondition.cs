namespace HyperGames.UnityTemplate.UnityTemplate.FTUE.Conditions
{
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;

    public class FTUEEnoughCurrencyContidionModel
    {
        public string Condition;
        public string Id;
        public int    Value;
    }

    public class FTUEEnoughCurrencyCondition : FtueCondition<FTUEEnoughCurrencyContidionModel>
    {
        #region inject

        private readonly UnityTemplateInventoryDataController UnityTemplateInventoryDataController;
        private readonly UnityTemplateFTUEHelper              UnityTemplateFtueHelper;

        #endregion

        public FTUEEnoughCurrencyCondition(UnityTemplateInventoryDataController UnityTemplateInventoryDataController, UnityTemplateFTUEHelper UnityTemplateFtueHelper)
        {
            this.UnityTemplateInventoryDataController = UnityTemplateInventoryDataController;
            this.UnityTemplateFtueHelper              = UnityTemplateFtueHelper;
        }

        public override string Id => "enough_currency";

        protected override bool IsPassedCondition(FTUEEnoughCurrencyContidionModel data)
        {
            return this.UnityTemplateFtueHelper.CompareIntWithCondition(this.UnityTemplateInventoryDataController.GetCurrencyValue(data.Id), data.Value, data.Condition);
        }
    }
}