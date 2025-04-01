using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorBlast
{
    public class GoalItemUI : MonoBehaviour
    {
        [SerializeField] private Image BackgroundImage;
        [SerializeField] private TextMeshProUGUI CountText;
        [SerializeField] private TileData TileDat;
        private TileType mTileType;

        public void SetGoal(LevelGoal levelGoal)
        {
            mTileType = levelGoal.TargetTile;
            SetBackgroundColor(mTileType);
            UpdateCount(levelGoal.Count);
        }

        public void UpdateCount(int count) 
        {
            CountText.SetText("x" + count);
        }

        private void SetBackgroundColor(TileType tileType)
        {
            Color color = TileDat.GetTileColor(tileType);
            color.a = 1f;
            BackgroundImage.color = color;
        }

        public void RefreshColor()
        {
            SetBackgroundColor(mTileType);
        }
    }
}
