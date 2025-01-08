using System;
using System.Linq;
using Source.Scripts.Car;
using Source.Scripts.Data;
using Source.Scripts.Ui.Menu;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Menu
{
    public class GarageShop : MonoBehaviour
    {
        public event Action<int> OnMoneyChanged; 
        public event Action<int> OnUpgradePriceChanged;
        public event Action<int> OnCarChanged;
        
        [SerializeField] private GarageScreen _garageScreen;
        [SerializeField] private CarBodyChanger _carBodyChanger;

        private User _user;
        private const int COLOR_PRICE = 100;
        private Color _currentColor;
        private bool _purchaseCarColor;
        private bool _purchaseCarCustomization;
        private int _currentPrice;
        
        [Inject]
        private void Construct(User user)
        {
            _user = user;
        }

        public int GetMoney()
        {
            return _user.Data.Money;
        }

        private void PurchaseUpgrades()
        {
            if (_user.SelectedCar == null)
                return;
            
            if(_user.Data.Money < _currentPrice)
                return;
            
            _user.RemoveMoney(_currentPrice);
            
            if(_purchaseCarCustomization)
                _user.SelectedCar.UpgradeTier++;
            if (_purchaseCarColor)
                _user.SelectedCar.Color = _currentColor;
            
            Reset();
        }

        private void PurchaseCar()
        { 
            var carName = _carBodyChanger.GetCarName();
            var carData = _user.Data.Cars.FirstOrDefault(data => data.Name == carName);
            if (carData == null)
                return;
            
            if(_user.Data.Money < carData.Price)
                return;
            
            _user.RemoveMoney(carData.Price);
            
            carData.IsPurchased = true;
            
            UpdateCarConfig();
        }

        private void Reset()
        {
            _purchaseCarColor = false;
            _purchaseCarCustomization = false;
            UpdatePrice();
            OnMoneyChanged?.Invoke(_user.Data.Money);
        }

        private void NextCar()
        {
            _carBodyChanger.NextBody();
            UpdateCarConfig();
        }

        private void PreviousCar()
        {
            _carBodyChanger.PreviousBody();
            UpdateCarConfig();
        }

        private void UpdateCarConfig()
        {
            var carName = _carBodyChanger.GetCarName();
            var carData = _user.Data.Cars.FirstOrDefault(data => data.Name == carName);
            if (carData == null)
                return;
            
            _carBodyChanger.ChangeUpgrade(carData.UpgradeTier);
            _carBodyChanger.ChangeColor(carData.Color);
            
            if (carData.IsPurchased)
            {
                SelectCar(carData);
            }
            
            Reset();

            OnCarChanged?.Invoke(carData.IsPurchased ? 0 : carData.Price);
        }

        private void UpgradeCustomization()
        {
            if(_user.SelectedCar.UpgradeTier >= CarData.MAX_UPGRADE_TIER)
                return;
            int upgradeValue = _purchaseCarCustomization? 0 : 1;
            _carBodyChanger.ChangeUpgrade(_user.SelectedCar.UpgradeTier + upgradeValue);
            _purchaseCarCustomization = !_purchaseCarCustomization;
            UpdatePrice();
        }

        private void SelectCar(CarData carData)
        {
            if (carData == null)
                return;
            
            _user.SelectCar(carData);
        }

        private void SelectColor(Color color)
        {
            if(_user.SelectedCar.Color == color)
            {
                _purchaseCarColor = false;
            }
            else
            {
                _purchaseCarColor = true;
            }
            
            _currentColor = color;
            _carBodyChanger.ChangeColor(color);
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            _currentPrice = 0;
            if (_purchaseCarColor)
            {
                _currentPrice += COLOR_PRICE;
            }

            if (_purchaseCarCustomization)
            {
                _currentPrice += _user.SelectedCar.UpgradePrice;
            }
            
            OnUpgradePriceChanged?.Invoke(_currentPrice);
        }

        private void Awake()
        {
            _garageScreen.OnColorSelected += SelectColor;
            _garageScreen.OnUpgradeCustomizationSelected += UpgradeCustomization;
            _garageScreen.OnNextCar += NextCar;
            _garageScreen.OnPreviousCar += PreviousCar;
            _garageScreen.OnPurchaseCar += PurchaseCar;
            _garageScreen.OnPurchaseUpgrades += PurchaseUpgrades;
            _garageScreen.OnBackButton += UpdateCarConfig;
            
            _user.OnMoneyChanged += OnMoneyChanged;
        }

        private void Start()
        {
            UpdateCarConfig();
        }

        private void OnDestroy()
        {
            _garageScreen.OnColorSelected += SelectColor;
            _garageScreen.OnUpgradeCustomizationSelected += UpgradeCustomization;
            _garageScreen.OnNextCar += NextCar;
            _garageScreen.OnPreviousCar += PreviousCar;
            _garageScreen.OnPurchaseCar += PurchaseCar;
            _garageScreen.OnPurchaseUpgrades += PurchaseUpgrades;
            _garageScreen.OnBackButton += UpdateCarConfig;
            
            _user.OnMoneyChanged += OnMoneyChanged;
        }
    }
}