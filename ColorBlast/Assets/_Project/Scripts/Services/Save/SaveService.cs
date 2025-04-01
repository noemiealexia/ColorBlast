using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ColorBlast
{
    public class SaveService : ISaveService
    {
        public const string SaveFileName = "PlayerData.dat";
        private SaveData mSaveData;

        public event Action SaveLoaded;

        public void Init()
        {
            Logman.Log("SaveService - Init");
        }

        public void Release()
        {
            Logman.Log("SaveService - Release");
        }

        public bool IsReady()
        {
            return mSaveData != null;
        }

        public SaveData Load()
        {
            if (IsReady())
            {
                SaveLoaded?.Invoke();
                return mSaveData;
            }

            LoadInternal();

            if (mSaveData == null)
            {
                CreateDefaultSaveData();
            }

            SaveLoaded?.Invoke();
            return mSaveData;
        }

        public void Save()
        {
            if (IsReady())
            {
                SaveInternal();
            }
        }

        public void DeleteSave()
        {
            if (IsReady())
            {
                int levelCount = ServiceManager.Instance.Get<ILevelService>().GetTotalLevelCount();
                mSaveData.ResetToDefaults(levelCount);
                Save();
            }
        }

        public SaveData GetSaveData() 
        {
            return mSaveData;
        }

        private void CreateDefaultSaveData()
        {
            int levelCount = ServiceManager.Instance.Get<ILevelService>().GetTotalLevelCount();

            mSaveData = new SaveData();
            mSaveData.ResetToDefaults(levelCount); // ??
            SaveInternal();
        }

        private void SaveInternal()
        {
            string json = JsonUtility.ToJson(mSaveData);

            SaveFileManager.WriteToFile(SaveFileName, json);
        }

        private void LoadInternal()
        {
            if (SaveFileManager.LoadFromFile(SaveFileName, out string jsonStr))
            {
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    try
                    {
                        mSaveData = JsonUtility.FromJson<SaveData>(jsonStr);

                        if (mSaveData.LevelProgress?.LevelCompleted == null || mSaveData.LevelProgress.LevelCompleted.Length == 0)
                        {
                            CreateDefaultSaveData();
                        }
                    }
                    catch
                    {
                        CreateDefaultSaveData();
                    }
                }
            }
            else
            {
                CreateDefaultSaveData();
            }
        }

        public bool IsLevelUnlocked(int levelIndex)
        {
            bool result = false;

            if (mSaveData?.LevelProgress?.LevelCompleted == null)
            {
                result = (levelIndex == 0);
            }
            else if (levelIndex == 0)
            {
                result = true;
            }
            else
            {
                result = mSaveData.LevelProgress.LevelCompleted.Length > levelIndex - 1 &&
                         mSaveData.LevelProgress.LevelCompleted[levelIndex - 1];
            }

            Debug.Log($"IsLevelUnlocked({levelIndex}) = {result}");
            return result;
        }

        public void MarkLevelCompleted(int levelIndex)
        {

            if (mSaveData?.LevelProgress?.LevelCompleted == null)
            {
                return;
            }

            if (levelIndex >= 0 && levelIndex < mSaveData.LevelProgress.LevelCompleted.Length)
            {
                mSaveData.LevelProgress.LevelCompleted[levelIndex] = true;
                Save();
            }
        }
    }

    [System.Serializable]
    public class LevelProgress
    {
        public bool[] LevelCompleted;
    }
}
