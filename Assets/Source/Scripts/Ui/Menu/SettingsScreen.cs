using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.Scripts.Camera;
using Source.Scripts.Data;
using Source.Scripts.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace Source.Scripts.Ui.Menu
{
    [Serializable]
    public class Settings
    {
        public float MusicVolume;
        public float SoundVolume;
        
        public static Settings GetDefaultSettings()
        {
            return new Settings
            {
                MusicVolume = 0.5f,
                SoundVolume = 0.5f
            };
        }
    }

    public class SettingsScreen : UiScreen
    {
        [SerializeField] private MainMenuUi _mainMenuUi;
        [SerializeField] private MenuCamera _menuCamera;
        
        [Header("UI Elements")]
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _soundVolumeSlider;
        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform _content;
        
        [Header("Parameters")]
        [SerializeField] private float _animationDuration = 2f;

        private User _user;
        private Settings _settings;
        private SettingsManager _settingsManager;

        [Inject]
        private void Construct(User user, SettingsManager settingsManager)
        {
            _user = user;
            _settingsManager = settingsManager;
        }

        public void Initialize()
        {
            
            SetMusicVolume(_settings.MusicVolume);
            SetSFXVolume(_settings.SoundVolume);
        }

        private void MusicVolumeChanged(float value)
        {
            _settings.MusicVolume = value;
            SetMusicVolume(value);
        }

        private void SoundVolumeChanged(float value)
        {
            _settings.SoundVolume = value;
            SetSFXVolume(value);
        }

        private void NicknameChanged(string value)
        {
            _user.Data.Nickname = value;
        }

        private void UpdateNicknameText()
        {
            _nicknameInputField.text = _user.Data.Nickname;
        }

        private async void BackClicked()
        {
            await DisappearAnimation();
            _mainMenuUi.ShowMenu();
        }

        private void SetMusicVolume(float sliderValue)
        {
            float dB = Mathf.Lerp(-80, 0, sliderValue);
            _audioMixer.SetFloat("MusicVolume", dB);
        }

        private void SetSFXVolume(float sliderValue)
        {
            float dB = Mathf.Lerp(-80, 0, sliderValue);
            _audioMixer.SetFloat("SFXVolume", dB);
        }

        private void AppearAnimation()
        {
            _content.DOMove(Vector2.zero + new Vector2(Screen.width/2, Screen.height/2), _animationDuration)
                .SetEase(Ease.InQuad);
            _menuCamera.MoveUp().Forget();
        }

        private async UniTask DisappearAnimation()
        {
            _menuCamera.MoveDown().Forget();
            await _content.DOMove((Vector2)_content.position + Screen.height * Vector2.up, _animationDuration)
                .SetEase(Ease.OutQuad)
                .ToUniTask();
        }
        
        private void UpdateUi()
        {
            _musicVolumeSlider.value = _settings.MusicVolume;
            _soundVolumeSlider.value = _settings.SoundVolume;
        }
        
        private void Awake()
        {
            _musicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
            _soundVolumeSlider.onValueChanged.AddListener(SoundVolumeChanged);
            _nicknameInputField.onValueChanged.AddListener(NicknameChanged);
            _backButton.onClick.AddListener(BackClicked);
            
            _content.position = (Vector2)_content.position + Screen.height * Vector2.up;
            
        }

        private void OnEnable()
        {
            _settings = _settingsManager.GetSettings();
            UpdateNicknameText();
            AppearAnimation();
            UpdateUi();
        }
        
        private void OnDestroy()
        {
            _musicVolumeSlider.onValueChanged.RemoveListener(MusicVolumeChanged);
            _soundVolumeSlider.onValueChanged.RemoveListener(SoundVolumeChanged);
            _nicknameInputField.onValueChanged.RemoveListener(NicknameChanged);
            _backButton.onClick.RemoveListener(BackClicked);
        }
    }
}