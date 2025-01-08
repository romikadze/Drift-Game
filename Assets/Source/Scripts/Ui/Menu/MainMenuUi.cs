using System;
using UnityEngine;

namespace Source.Scripts.Ui.Menu
{
    public class MainMenuUi : MonoBehaviour
    {
        [Header("Screens")]
        [SerializeField] private UiScreen _lobbyScreen;
        [SerializeField] private UiScreen _garageScreen;
        [SerializeField] private UiScreen _settingsScreen;
        [SerializeField] private UiScreen _menuScreen;

        public void ShowMenu()
        {
            _menuScreen.Show();
            _settingsScreen.Hide();
            _garageScreen.Hide();
            _lobbyScreen.Hide();
        }
        
        public void ShowPlay()
        {
            _menuScreen.Hide();
            _settingsScreen.Hide();
            _garageScreen.Hide();
            _lobbyScreen.Show();
        }
        
        public void ShowGarage()
        {
            _menuScreen.Hide();
            _settingsScreen.Hide();
            _garageScreen.Show();
            _lobbyScreen.Hide();
        }
        
        public void ShowSettings()
        {
            _menuScreen.Hide();
            _settingsScreen.Show();
            _garageScreen.Hide();
            _lobbyScreen.Hide();
        }

        private void Awake()
        {
            ShowMenu();
        }
    }
}