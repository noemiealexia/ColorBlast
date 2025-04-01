using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorBlast
{
    public class LevelItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI LevelNumber;
        [SerializeField] private TextMeshProUGUI LevelName;
        [SerializeField] private Button OpenLevelButton;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject lockIcon;


        private int mLevelIndex = 0;
        private ILevelService mLevelService;

        private void Awake()
        {
            OpenLevelButton.onClick.AddListener(OnOpenLevelClicked);
            mLevelService = ServiceManager.Instance.Get<ILevelService>();
        }

        public void Fill(int levelNumber, Level level, bool isUnlocked)
        {
            LevelName.SetText(level.Name);
            LevelNumber.SetText("Level: " + levelNumber.ToString());

            mLevelIndex = levelNumber - 1;

            OpenLevelButton.interactable = isUnlocked;
            if (canvasGroup != null)
                canvasGroup.alpha = isUnlocked ? 1f : 0.5f;

            if (lockIcon != null)
                lockIcon.SetActive(!isUnlocked);
        }

        private void OnOpenLevelClicked() 
        {
            mLevelService.LoadLevel(mLevelIndex);
        }
    }
}
