using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.Scripts.Camera;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Ui.Menu
{
    public class LobbyUi : UiScreen
    {
        public event Action OnStartGameClicked;
        public event Action<string> OnCreateRoomClicked;
        public event Action<string> OnJoinRoomClicked;
        public event Action OnExitRoomClicked;
        public event Action OnLobbyScreenOpened; 

        [SerializeField] private MainMenuUi _mainMenuUi;

        [Header("UI Elements")]
        [SerializeField] private Transform _lobbyPanel;
        [SerializeField] private Transform _roomPanel;
        [SerializeField] private Transform _content;
        [SerializeField] private PlayScreen _playScreen;
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_InputField _roomNameInput;
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private TMP_Text _playerListText;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private Button _joinRoomButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _leaveButton;
        [SerializeField] private MenuCamera _menuCamera;
       
        [Header("Parameters")]
        [SerializeField] private float _animationDuration = 2f;

        public void SetPlayerName(string playerName)
        {
            _playerNameText.text = playerName;
        }
        
        public void SetRoomName(string roomName)
        {
            _roomNameText.text = roomName;
        }
        
        public void SetPlayerList(string playerList)
        {
            _playerListText.text = playerList;
        }
        
        public void ShowRoomPanel(string roomName, bool isHost)
        {
            _roomNameText.text = roomName;
            _lobbyPanel.gameObject.SetActive(false);
            _roomPanel.gameObject.SetActive(true);
            _startGameButton.interactable = isHost;
        }

        private void JoinRoomClicked()
        {
            OnJoinRoomClicked?.Invoke(_roomNameInput.text);
        }

        private void CreateRoomClicked()
        {
            OnCreateRoomClicked?.Invoke(_roomNameInput.text);
        }
        
        private void StartGameClicked()
        {
            OnStartGameClicked?.Invoke();   
        }
        
        private void ExitRoomClicked()
        {
            OnExitRoomClicked?.Invoke();
            _lobbyPanel.gameObject.SetActive(true);
            _roomPanel.gameObject.SetActive(false);
        }
        
        private async void BackClicked()
        {
            await DisappearAnimation();
            _mainMenuUi.ShowMenu();
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

        private void Awake()
        {
            _createRoomButton.onClick.AddListener(CreateRoomClicked);
            _joinRoomButton.onClick.AddListener(JoinRoomClicked);
            _startGameButton.onClick.AddListener(StartGameClicked);
            _leaveButton.onClick.AddListener(ExitRoomClicked);
            _backButton.onClick.AddListener(BackClicked);
            
            _content.position = (Vector2)_content.position + Screen.height * Vector2.up;
        }
        
        private void OnEnable()
        {
            AppearAnimation();
            OnLobbyScreenOpened?.Invoke();
        }
        
        private void OnDestroy()
        {
            _createRoomButton.onClick.RemoveListener(CreateRoomClicked);
            _joinRoomButton.onClick.RemoveListener(JoinRoomClicked);
            _startGameButton.onClick.RemoveListener(StartGameClicked);
            _leaveButton.onClick.RemoveListener(ExitRoomClicked);
            _backButton.onClick.RemoveListener(BackClicked);
        }
    }
}