namespace HyperGames.UnityTemplate.UnityTemplate.Scenes.Main.Level
{
    using System.Collections.Generic;
    using HyperGames.UnityTemplate.Scripts.Models.Controllers;
    using HyperGames.UnityTemplate.Scripts.Models.LocalDatas;
    using HyperGames.UnityTemplate.UnityTemplate.Models;
    using HyperGames.UnityTemplate.UnityTemplate.Models.Controllers;
    using TMPro;
    using UnityEngine;

    public class UnityTemplateLevelItemWithStarView : UnityTemplateLevelItemView
    {
        public List<GameObject> StarList;

        public override void InitView(LevelData data, UnityTemplateLevelDataController userLevelData)
        {
            base.InitView(data, userLevelData);
            data.StarCount = data.LevelStatus != LevelData.Status.Passed ? 0 : data.StarCount;
            for (var i = 0; i < this.StarList.Count; i++) this.StarList[i].SetActive(i < data.StarCount);

            if (data.StarCount == 0)
                this.LevelText.alignment = TextAlignmentOptions.Center;
            else
                this.LevelText.alignment = TextAlignmentOptions.Top;
        }
    }
}