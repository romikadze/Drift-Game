using Source.Scripts.Game;
using UnityEngine;

namespace Source.Scripts.Ui.Game
{
    public class InGameUi : MonoBehaviour, IStartRace, IEndRace
    {
        [SerializeField] private UiScreen _raceScreen;
        [SerializeField] private UiScreen _endRaceScreen;

        public void StartRace()
        {
            _raceScreen.Show();
        }

        public void EndRace()
        {
            _raceScreen.Hide();
            _endRaceScreen.Show();
        }

        private void Awake()
        {
            _raceScreen.Hide();
            _endRaceScreen.Hide();
        }
    }
}