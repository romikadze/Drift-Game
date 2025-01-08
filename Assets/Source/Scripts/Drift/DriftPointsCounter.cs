using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Source.Scripts.Car;
using Source.Scripts.Data;
using Source.Scripts.Game;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Drift
{
    public class DriftPointsCounter : MonoBehaviour, IEndRace, IStartRace
    {
        public event Action<float> OnDriftTimeChanged;
        public event Action<int> OnDriftPointsChanged;

        [SerializeField] private Vehicle _vehicle;
        
        [SerializeField] private float _driftCountdown = 1.5f;
        [SerializeField] private float _baseRewardPerSecond = 10f;
        [SerializeField] private float _driftTimeRewardPower = 1.5f;

        private CancellationTokenSource _cancellationTokenSource;
        private RaceData _raceData;
        private int _driftPoints;
        private float _driftTime;

        [Inject]
        private void Construct(RaceData raceData)
        {
            _raceData = raceData;
        }

        public void StartRace()
        {
            gameObject.SetActive(true);
        }

        public void EndRace()
        {
            _driftPoints += CalculateDriftReward(_driftTime);
            CancelTask();
            _raceData.AddMoney(_driftPoints);
            gameObject.SetActive(false);
        }
        
        public void SetVehicle(Vehicle vehicle)
        {
            if (_vehicle != null)
            {
                _vehicle.OnDriftStart -= StartDriftTimer;
                _vehicle.OnDriftEnd -= StopDriftTimer;
            }
            
            _vehicle = vehicle;
            _vehicle.OnDriftStart += StartDriftTimer;
            _vehicle.OnDriftEnd += StopDriftTimer;
        }

        private void StartDriftTimer()
        {
            CancelTask();
            _cancellationTokenSource = new CancellationTokenSource();
            DriftTimerAsync(_cancellationTokenSource.Token).Forget();
        }

        private void StopDriftTimer()
        {
            CancelTask();
            _cancellationTokenSource = new CancellationTokenSource();
            StartDriftCountdownAsync(_cancellationTokenSource.Token).Forget();
        }

        private int CalculateDriftReward(float driftTime)
        {
            float reward = _baseRewardPerSecond * Mathf.Pow(driftTime, _driftTimeRewardPower);
            return Mathf.FloorToInt(reward);
        }

        private async UniTask StartDriftCountdownAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_driftCountdown), cancellationToken: token);
            if(token.IsCancellationRequested) return;
            _driftPoints += CalculateDriftReward(_driftTime);
            _driftTime = 0;
            OnDriftTimeChanged?.Invoke(_driftTime);
            OnDriftPointsChanged?.Invoke(_driftPoints);
        }

        private async UniTask DriftTimerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _driftTime += Time.deltaTime;
                OnDriftTimeChanged?.Invoke(_driftTime);
                await UniTask.Yield();
            }
        }

        private void CancelTask()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private void Awake()
        {
            if (_vehicle == null)
                return;
            _vehicle.OnDriftStart += StartDriftTimer;
            _vehicle.OnDriftEnd += StopDriftTimer;
        }
        
        private void OnDestroy()
        {
            CancelTask();
            _vehicle.OnDriftStart -= StartDriftTimer;
            _vehicle.OnDriftEnd -= StopDriftTimer;
        }
    }
}