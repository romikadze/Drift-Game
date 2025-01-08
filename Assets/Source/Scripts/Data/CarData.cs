using System;
using UnityEngine;

namespace Source.Scripts.Data
{
    [Serializable]
    public class CarData
    {
        public string Name;
        public Color Color;
        public int UpgradeTier;
        public int Price;
        public int UpgradePrice;
        public bool IsPurchased;
        public const float MAX_UPGRADE_TIER = 3;
        
        public static CarData[] GetDefaultCarData()
        {
            CarData[] carData = new CarData[2];
            carData[0] = new CarData
            {
                Name = "RacingCar",
                Color = Color.green,
                UpgradeTier = 0,
                Price = 0,
                UpgradePrice = 150,
                IsPurchased = true
            };
            
            carData[1] = new CarData
            {
                Name = "PoliceCar",
                Color = Color.grey,
                UpgradeTier = 0,
                Price = 500,
                UpgradePrice = 200,
                IsPurchased = false
            };
            
            return carData;
        }
    }
}