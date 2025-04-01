using System.Collections.Generic;
using System;
using UnityEngine;
using static ColorBlast.SaveService;

namespace ColorBlast
{

    [Serializable]

    public class SaveData
    {
        public SettingsData Settings;
        public LevelProgress LevelProgress;

        public SaveData()
        {

        }

        public void ResetToDefaults(int levelCount)
        {
            Settings = new SettingsData();

            LevelProgress = new LevelProgress
            {
                LevelCompleted = new bool[levelCount]
            };
        }
    }

    #region DataDefinitions

    [Serializable]
    public class SettingsData
    {
        public bool SfxIsOn;
        public bool ColorBlindMode;

        public SettingsData()
        {
            SfxIsOn = true;
            ColorBlindMode = false;
        }
    }

    #endregion
}
