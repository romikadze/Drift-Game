using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Scripts.Ui.Menu
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private MainMenuUi _mainMenuUi;
        
        [Header("UI Elements")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _garageButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;


        private void PlayButtonClicked()
        {
            _mainMenuUi.ShowPlay();
        }

        private void GarageButtonClicked()
        {
            _mainMenuUi.ShowGarage();
        }

        private void SettingsButtonClicked()
        {
            _mainMenuUi.ShowSettings();
        }

        private void ExitButtonClicked()
        {
            Application.Quit();
        }

        private void Awake()
        {
            _playButton.onClick.AddListener(PlayButtonClicked);
            _garageButton.onClick.AddListener(GarageButtonClicked);
            _settingsButton.onClick.AddListener(SettingsButtonClicked);
            _exitButton.onClick.AddListener(ExitButtonClicked);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(PlayButtonClicked);
            _garageButton.onClick.RemoveListener(GarageButtonClicked);
            _settingsButton.onClick.RemoveListener(SettingsButtonClicked);
            _exitButton.onClick.RemoveListener(ExitButtonClicked);
        }
    }
}