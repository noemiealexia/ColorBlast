using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorBlast
{
    public class MainUI : MonoBehaviour, IWindow
    {
        [Header("Other Windows")]
        [SerializeField] private LevelUI LevelUI;
        [SerializeField] private InGameUI InGameUI;

        [Header("Buttons")]
        [SerializeField] private Button HomeButton;
        [SerializeField] private Button MuteButton;
        [SerializeField] private Button PlayButton;
        [SerializeField] private Button ReplayButton;

        [Header("Sprite Assets")]
        [SerializeField] private Image ReplayButtonImg;
        [SerializeField] private Sprite SfxIsOnSprite;
        [SerializeField] private Sprite SfxIsOffSprite;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI levelFailedText;
        [SerializeField] private TextMeshProUGUI successText;

        private ILevelService mLevelService;
        private IGameService mGameService;
        private ISettingsService mSettingsService;

        void Awake()
        {
            HomeButton.onClick.AddListener(OnHomeClicked);
            MuteButton.onClick.AddListener(OnMuteClicked);
            PlayButton.onClick.AddListener(OnPlayClicked);
            ReplayButton.onClick.AddListener(OnReplayClicked);

            mLevelService    = ServiceManager.Instance.Get<ILevelService>();
            mGameService     = ServiceManager.Instance.Get<IGameService>();
            mSettingsService = ServiceManager.Instance.Get<ISettingsService>();
        }

        void Start() 
        {
            UpdateSfxSetting();
        }

        void OnEnable() 
        {
            mLevelService.LevelLoaded        += OnLevelLoaded;
            mLevelService.LevelCompleted     += OnLevelCompleted;
            mGameService.GameInited          += OnGameInited;
            mSettingsService.SettingsUpdated += OnSettingsUpdated;
        }

        void OnDisable() 
        {
            mLevelService.LevelLoaded        -= OnLevelLoaded;
            mLevelService.LevelCompleted     -= OnLevelCompleted;
            mGameService.GameInited          -= OnGameInited;
            mSettingsService.SettingsUpdated -= OnSettingsUpdated;
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(true);
        }

        private void OnLevelLoaded(Level obj)
        {
            // Enable play button
            PlayButton.gameObject.SetActive(true);
        }

        private void OnLevelCompleted()
        {
            DOVirtual.DelayedCall(0.2f, () =>
            {
                // Level completed enable replay button with small delay
                ReplayButton.gameObject.SetActive(true);
                successText.gameObject.SetActive(true);

                LevelUI.RefreshLevelList();
            });

        }

        public void ShowReplayOnFailure()
        {
            ReplayButton.gameObject.SetActive(true);
        }

        private void OnGameInited()
        {
            LevelUI.Open();
        }

        private void OnSettingsUpdated() 
        {
            UpdateSfxSetting();
        }

        private void UpdateSfxSetting() 
        {
            var audioSetting = mSettingsService.GetAudioSettings();
            ReplayButtonImg.sprite = audioSetting.SfxIsOn ? SfxIsOnSprite : SfxIsOffSprite;
        }

        public void AnimateLevelFailed()
        {
            levelFailedText.gameObject.SetActive(true);
            levelFailedText.alpha = 0f;
            levelFailedText.transform.localScale = Vector3.one * 0.8f;

            Sequence seq = DOTween.Sequence();
            seq.Append(levelFailedText.DOFade(1f, 0.4f));
            seq.Join(levelFailedText.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack));
        }

        #region Button Callbacks

        private void OnHomeClicked() 
        {
            LevelUI.Open();
            InGameUI.Close();

            successText.gameObject.SetActive(false);
            levelFailedText.gameObject.SetActive(false);
            ServiceManager.Instance.Get<IGameService>().EndSession();
            PlayButton.gameObject.SetActive(false);
            ReplayButton.gameObject.SetActive(false);
        }

        private void OnMuteClicked() 
        {
            var audioSetting = mSettingsService.GetAudioSettings();
            audioSetting.SfxIsOn = !audioSetting.SfxIsOn; // Toggle setting

            mSettingsService.SetAudioSettings(audioSetting);
        }

        private void OnPlayClicked()
        {
            LevelUI.Close();
            InGameUI.Open();
            PlayButton.gameObject.SetActive(false);
         
            ServiceManager.Instance.Get<IGameService>().StartSession();
        }

        private void OnReplayClicked() 
        {
            successText.gameObject.SetActive(false);
            levelFailedText.gameObject.SetActive(false);
            // Reload current level
            var lastIndex = mLevelService.GetLastLoadedLevelIndex();
            mLevelService.LoadLevel(lastIndex);
            ReplayButton.gameObject.SetActive(false);
        }

        #endregion
    }
}
