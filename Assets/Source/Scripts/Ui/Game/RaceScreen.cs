using Source.Scripts.Drift;
using Source.Scripts.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Ui.Game
{
    public class RaceScreen : UiScreen
    {
        [SerializeField] private TMP_Text _driftPointsText;
        [SerializeField] private TMP_Text _driftTimeText;
        [SerializeField] private TMP_Text _raceTimeText;
        
        private DriftPointsCounter _driftPointsCounter;
        private Race _race;
        
        private const string DRIFT_POINTS_TEXT = "Drift Points: ";
        private const string DRIFT_TIME_TEXT = "Drift Time: ";
        private const string RACE_TIME_TEXT = "Time left: ";

        [Inject]
        private void Construct(DriftPointsCounter driftPointsCounter, Race race)
        {
            _driftPointsCounter = driftPointsCounter;
            _race = race;
        }
        
        public override void Show()
        {
            gameObject.SetActive(true);
        }
        
        public override void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void UpdateDriftPoints(int value)
        {
            _driftPointsText.text = DRIFT_POINTS_TEXT + value;
        }

        private void UpdateDriftTime(float value)
        {
            _driftTimeText.text = DRIFT_TIME_TEXT + value.ToString("F2");
        }
        
        private void UpdateRaceTime(float value)
        {
            _raceTimeText.text = RACE_TIME_TEXT + value.ToString("F2");
        }

        private void Awake()
        {
            _driftPointsCounter.OnDriftTimeChanged += UpdateDriftTime;
            _driftPointsCounter.OnDriftPointsChanged += UpdateDriftPoints;
            _race.OnRaceTimeChanged += UpdateRaceTime;
        }

        private void OnDestroy()
        {
            _driftPointsCounter.OnDriftTimeChanged -= UpdateDriftTime;
            _driftPointsCounter.OnDriftPointsChanged -= UpdateDriftPoints;
            _race.OnRaceTimeChanged -= UpdateRaceTime;
        }
    }
}