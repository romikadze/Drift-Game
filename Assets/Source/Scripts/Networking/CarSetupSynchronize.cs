using Photon.Pun;
using Source.Scripts.Car;
using Source.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Networking
{
    public class CarSetupSynchronize : MonoBehaviourPunCallbacks
    {
        [SerializeField] private CarBodyChanger[] _carBodyChangers;
        private User _user;
        
        [Inject] private void Construct(User user)
        {
            _user = user;
        }
        
        public void UpdateCarSetup()
        {
            var players = PhotonNetwork.PlayerList;
            var carPhotonView = GetComponent<PhotonView>();
            
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == PhotonNetwork.LocalPlayer)
                {
                    carPhotonView.RPC(nameof(RPC_UpdateCarCustomization), RpcTarget.All, i, _user.SelectedCar.Color.ToColorString(), _user.SelectedCar.Name, _user.SelectedCar.UpgradeTier);
                }
            }
        }
        
        [PunRPC]
        private void RPC_UpdateCarCustomization(int index, string color, string body, int upgradeTier)
        {
            _carBodyChangers[index].ChangeBody(body);
            _carBodyChangers[index].ChangeColor(ColorExtensions.FromColorString(color));
            _carBodyChangers[index].ChangeUpgrade(upgradeTier);
        }
    }
    
    public static class ColorExtensions
    {
        public static string ToColorString(this Color color)
        {
            return $"{color.r},{color.g},{color.b},{color.a}";
        }

        public static Color FromColorString(string colorString)
        {
            var values = colorString.Split(',');
            if (values.Length != 4) throw new System.FormatException("Invalid color string format.");
            return new Color(
                float.Parse(values[0]),
                float.Parse(values[1]),
                float.Parse(values[2]),
                float.Parse(values[3])
            );
        }
    }
}