using Photon.Pun;
using Photon.Realtime;
using Source.Scripts.Camera;
using Source.Scripts.Car;
using Source.Scripts.Drift;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Scripts.Networking
{
    public class NetworkAssigner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Vehicle[] _cars;
        [SerializeField] private CameraMovement _camera;
        [SerializeField] private DriftPointsCounter _driftPointsCounter;
        [SerializeField] private CarSetupSynchronize _carSetupSynchronize;
        
        
        
        private void Start()
        {
            AssignCarsToPlayers();
        }

        private void AssignCarsToPlayers()
        {
            var players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                if (PhotonNetwork.IsMasterClient && i < _cars.Length)
                {
                    PhotonView carPhotonView = _cars[i].GetComponent<PhotonView>();
                    carPhotonView.TransferOwnership(players[i]);
                }
                
                if (players[i] == PhotonNetwork.LocalPlayer)
                {
                    _camera.SetTarget(_cars[i].transform);
                    _driftPointsCounter.SetVehicle(_cars[i]);
                    _carSetupSynchronize.UpdateCarSetup();
                }
            }
            for (int i = players.Length; i < _cars.Length; i++)
            {
                _cars[i].gameObject.SetActive(false);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                AssignCarsToPlayers();
            }
        }
        
      
    }
}