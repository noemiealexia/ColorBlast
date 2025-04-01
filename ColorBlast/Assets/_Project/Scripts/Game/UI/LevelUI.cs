using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ColorBlast
{
    public class LevelUI : MonoBehaviour, IWindow
    {
        [SerializeField] private LevelItemUI LevelItemUIPrefab;
        [SerializeField] private Transform ItemContainerTransform;

        private ILevelService mLevelService;
        private ISaveService mSaveService;

        private bool mListPopulated = false;

        void Awake() 
        {
            mLevelService = ServiceManager.Instance.Get<ILevelService>();
            mSaveService = ServiceManager.Instance.Get<ISaveService>();
        }

        void OnEnable() 
        {
            mLevelService.LevelLoaded += OnLevelLoaded;
        }

        void OnDisable() 
        {
            mLevelService.LevelLoaded -= OnLevelLoaded;
        }
        
        public void Open()
        {
            gameObject.SetActive(true);

            // If the list is populated before, skip
            if (!mListPopulated) 
            {
                PopulateLevelList();
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnLevelLoaded(Level obj)
        {
            // Close level window if a level is loaded.
            Close();
        }

        private void PopulateLevelList() 
        {
            var saveService = ServiceManager.Instance.Get<ISaveService>();
            var levels = mLevelService.GetLevelList();

            for(int i = 0; i < levels.Count; i++) 
            {
                bool isUnlocked = saveService.IsLevelUnlocked(i);
                Debug.Log($"Level {i} isUnlocked: {isUnlocked}");

                var levelItemUI = Instantiate(LevelItemUIPrefab, ItemContainerTransform);
                levelItemUI.Fill(i + 1, levels[i], isUnlocked);
            }

            mListPopulated = true;
        }

        public void RefreshLevelList()
        {

            foreach (Transform child in ItemContainerTransform)
            {
                Destroy(child.gameObject);
            }

            mListPopulated = false;
            PopulateLevelList();
        }
    }
}
