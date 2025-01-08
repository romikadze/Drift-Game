using System;
using Source.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Game
{
    public class RaceMoneyManager : IInitializable, IDisposable
    {
        private User _user;
        private RaceData _raceData;
        private AdManager _adManager;
        
        [Inject] 
        private void Construct(User user, RaceData raceData, AdManager adManager)
        {
            _user = user;
            _raceData = raceData;
            _adManager = adManager;
        }
            
        public void Initialize()
        {
            _adManager.OnRewarded += SetRewarded;
        }

        public void AddMoney()
        {
            _user.AddMoney(_raceData.Money);
        }

        public void Dispose()
        {
            _adManager.OnRewarded -= SetRewarded;
        }
        
        private void SetRewarded(bool isRewarded)
        {
            Debug.Log($"Rewarded {isRewarded}");
            if (isRewarded)
            {
                _raceData.AddMoney(_raceData.Money);
            }
        }
    }
}