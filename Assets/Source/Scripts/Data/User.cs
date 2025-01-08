using System;
using Source.Scripts.Paths;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Data
{
    public class User : IInitializable, IDisposable
    {
        public event Action<int> OnMoneyChanged;
        
        public PlayerData Data { get; private set; }
        
        public CarData SelectedCar { get; private set; }
        
        public void AddMoney(int money)
        {
            if (money < 0)
            {
                throw new ArgumentException("Money cannot be negative value");
            }
            
            Data.Money += money;
            OnMoneyChanged?.Invoke(Data.Money);
        }
        
        public void SelectCar(CarData car)
        {
            SelectedCar = car;
        }
        
        public void RemoveMoney(int money)
        {
            if (money < 0)
            {
                throw new ArgumentException("Money cannot be negative value");
            }
            
            if (Data.Money - money < 0)
            {
                throw new ArgumentException("Not enough money");
            }
            
            Data.Money -= money;
            OnMoneyChanged?.Invoke(Data.Money);
        }

        public void Initialize()
        {
            LoadData();
        }

        public void Dispose()
        {
            SaveOwnedCars();
        }

        private void LoadData()
        {
            JsonDataSaver dataSaver = new JsonDataSaver();
            Data = dataSaver.Load<PlayerData>(SavePath.PLAYER_DATA_PATH) ?? PlayerData.GetDefaultPlayerData();
            SelectedCar = Data.Cars[0];
        }
        
        private void SaveOwnedCars()
        {
            JsonDataSaver dataSaver = new JsonDataSaver();
            dataSaver.Save(Data, SavePath.PLAYER_DATA_PATH);
        }
    }

    [Serializable]
    public class PlayerData
    {
        public int Money;

        public string Nickname;
        
        public CarData[] Cars;
        
        public static PlayerData GetDefaultPlayerData()
        {
            return new PlayerData
            {
                Money = 0,
                Nickname = "Player",
                Cars = CarData.GetDefaultCarData()
            };
        }
    }
}