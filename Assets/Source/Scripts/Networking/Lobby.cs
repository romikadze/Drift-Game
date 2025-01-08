using Photon.Pun;
using Photon.Realtime;
using Source.Scripts.Data;
using Source.Scripts.Ui.Menu;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Source.Scripts.Networking
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayScreen _playScreen;
        [SerializeField] private LobbyUi _lobbyUi;

        private User _user;

        [Inject]
        private void Construct(User user)
        {
            _user = user;
        }
        
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Master Server.");
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name);
        }

        public override void OnJoinedRoom()
        {
            _lobbyUi.ShowRoomPanel(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.IsMasterClient);
            UpdatePlayerList();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayerList();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            UpdatePlayerList();
        }
        
        private void SetPlayerName()
        {
            _lobbyUi.SetPlayerName("Nickname: " + _user.Data.Nickname);
            PhotonNetwork.NickName = _user.Data.Nickname;
        }

        private void CreateRoom(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
                roomName = "Room" + Random.Range(1000, 9999);

            _lobbyUi.SetRoomName(roomName);
            
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 4
            };

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        private void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void UpdatePlayerList()
        {
            var players = "Players:\n";
            foreach (var player in PhotonNetwork.PlayerList)
            {
                players += player.NickName + "\n";
            }

            _lobbyUi.SetPlayerList(players);
        }

        private void StartGame(string sceneName)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(sceneName);
            }
        }

        private void CheckForLobby()
        {
            if (PhotonNetwork.InRoom)
            {
                _lobbyUi.ShowRoomPanel(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.IsMasterClient);
                UpdatePlayerList();
            }
        }

        private void Awake()
        {
            _lobbyUi.OnCreateRoomClicked += CreateRoom;
            _lobbyUi.OnJoinRoomClicked += JoinRoom;
            _lobbyUi.OnExitRoomClicked += LeaveRoom;
            _lobbyUi.OnStartGameClicked += _playScreen.Show;
            _lobbyUi.OnLobbyScreenOpened += SetPlayerName;
            _lobbyUi.OnLobbyScreenOpened += CheckForLobby;
            _playScreen.OnLevelSelected += StartGame;
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void OnDestroy()
        {
            _lobbyUi.OnCreateRoomClicked -= CreateRoom;
            _lobbyUi.OnJoinRoomClicked -= JoinRoom;
            _lobbyUi.OnExitRoomClicked -= LeaveRoom;
            _lobbyUi.OnStartGameClicked -= _playScreen.Show;
            _lobbyUi.OnLobbyScreenOpened -= SetPlayerName;
            _playScreen.OnLevelSelected -= StartGame;
        }
    }
}