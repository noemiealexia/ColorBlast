using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    public interface ISaveService : IService
    {
        event Action SaveLoaded;

        void Save();
        SaveData Load();
        void DeleteSave();
        bool IsReady();
        SaveData GetSaveData();
        bool IsLevelUnlocked(int levelIndex);

        void MarkLevelCompleted(int levelIndex);
    }
}
