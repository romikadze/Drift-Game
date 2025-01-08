using System;
using Source.Scripts.Ui.Game;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Game
{
    public class Race : MonoBehaviour
    {
        public event Action<float> OnRaceTimeChanged;
        
        [SerializeField] private float _raceTime = 120f;
        
        private IStartRace[] _startRaceObjects;
        private IEndRace[] _endRaceObjects;
        private RaceMoneyManager _raceMoneyManager;
        private EndRaceScreen _endRaceScreen;
        private SceneLoader _sceneLoader;
        private float _timeLeft;
        
        [Inject]
        private void Construct(IStartRace[] startRaceObjects, IEndRace[] endRaceObjects,
            RaceMoneyManager raceMoneyManager, EndRaceScreen endRaceScreen, SceneLoader sceneLoader)
        {
            _startRaceObjects = startRaceObjects;   
            _endRaceObjects = endRaceObjects;
            _raceMoneyManager = raceMoneyManager;
            _endRaceScreen = endRaceScreen;
            _sceneLoader = sceneLoader;
        }

        private void ExitToMenu()
        {
            _raceMoneyManager.AddMoney();
            _sceneLoader.LoadMainMenu();
        }
        
        private void StartRace()
        {
            foreach (var startRaceObject in _startRaceObjects)
            {
                startRaceObject.StartRace();
            }
            Invoke(nameof(EndRace), _raceTime);
        }

        private void EndRace()
        {
            foreach (var endRaceObject in _endRaceObjects)
            {
                endRaceObject.EndRace();
            }
        }

        private void Awake()
        {
            _endRaceScreen.OnExitClicked += ExitToMenu;
        }

        private void Start()
        {
            StartRace();
            _timeLeft = _raceTime;
        }
        
        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            OnRaceTimeChanged?.Invoke(_timeLeft);
        }
    }
}