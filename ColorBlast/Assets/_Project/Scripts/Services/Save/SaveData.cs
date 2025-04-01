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

            //LevelProgress.LevelCompleted[0] = true;
        }
    }

    #region DataDefinitions

    [Serializable]
    public class SettingsData
    {
        public bool SfxIsOn;

        public SettingsData()
        {
            SfxIsOn = true;
        }

        public void SetSfxIsOn(bool isOn)
        {
            SfxIsOn = isOn;
        }
    }

    #endregion
}
