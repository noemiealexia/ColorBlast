using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    [Serializable]
    public class TileInfo 
    {
        public TileType TType;
        public Color TColor;
        public Color TColorBlind;
        public bool SpecialTile;
    }

    /// <summary>
    /// Color, sprite info of the Tiles are stored here
    /// </summary>
    [CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 1)]
    public class TileData : ScriptableObject
    {
        public List<TileInfo> Tiles;

        public Color GetTileColor(TileType tileType)
        {
            var tile = Tiles.Find(x => x.TType == tileType);

            if (tile != null)
            {
                var settings = ServiceManager.Instance.Get<ISettingsService>();
                var isColorBlind = settings.GetAudioSettings().ColorBlindMode;

                return isColorBlind ? tile.TColorBlind : tile.TColor;
            }
            else
            {
                return Color.magenta;
            }
        }

        public bool IsSpecialTile(TileType tileType) 
        {
            var tile = Tiles.Find(x => x.TType == tileType);
            bool result = false;

            if (tile != null)
            {
                result = tile.SpecialTile;
            }

            return result;
        }
    }
}
