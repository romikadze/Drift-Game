using System;
using Source.Scripts.Camera;
using Source.Scripts.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Source.Scripts.Ui.Menu
{
    public class GarageScreen : UiScreen
    {
        public event Action<Color> OnColorSelected;
        public event Action OnUpgradeCustomizationSelected;
        public event Action OnBackButton;
        public event Action OnPurchaseCar;
        public event Action OnPurchaseUpgrades;
        public event Action OnNextCar;
        public event Action OnPreviousCar;

        [SerializeField] private MainMenuUi _mainMenuUi;
        [SerializeField] private GarageShop _garageShop;


        [Header("UI Elements")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextCarButton;
        [SerializeField] private Button _previousCarButton;
        [SerializeField] private Button _buyUpgradesButton;
        [SerializeField] private Button _buyCarButton;
        [SerializeField] private Button _upgradeCustomizationTierButton;
        [SerializeField] private GameObject _upgradesPanel;
        [SerializeField] private Button[] _colorButtons;
        [SerializeField] private TMP_Text _carCostText;
        [SerializeField] private TMP_Text _upgradeCostText;
        [SerializeField] private TMP_Text _moneyText;

        private static readonly Color[] CarColors = {Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white, Color.grey, Color.magenta};
        private UnityAction[] _onColorSelectedActions = new UnityAction[CarColors.Length];
        private MenuCamera _menuCamera;
        
        [Inject]
        private void Construct(MenuCamera menuCamera)
        {
            _menuCamera = menuCamera;
        }
        
        private void BackButtonClicked()
        {
            _mainMenuUi.ShowMenu();
        }

        private void UpgradeCustomizationTierButtonClicked()
        {
            OnUpgradeCustomizationSelected?.Invoke();
        }

        private void BuyCarButtonClicked()
        {
            OnPurchaseCar?.Invoke();
        }

        private void BuyUpgradesButtonClicked()
        {
            OnPurchaseUpgrades?.Invoke();
        }

        private async void NextCarButtonClicked()
        {
            _previousCarButton.interactable = false;
            _nextCarButton.interactable = false;
            await _menuCamera.RotateAroundOut();
            OnNextCar?.Invoke();
            await _menuCamera.RotateAroundIn();
            _previousCarButton.interactable = true;
            _nextCarButton.interactable = true;
        }

        private void UpdateMoneyText(int value)
        {
            _moneyText.text = value.ToString();
        }

        private async void PreviousCarButtonClicked()
        {
            _previousCarButton.interactable = false;
            _nextCarButton.interactable = false;
            await _menuCamera.RotateAroundOut(true);
            OnPreviousCar?.Invoke();
            await _menuCamera.RotateAroundIn(true);
            _previousCarButton.interactable = true;
            _nextCarButton.interactable = true;
        }

        private void UpdateUpgradeUpgradePrice(int value)
        {
            if (value == 0)
            {
                _buyUpgradesButton.gameObject.SetActive(false);
                return;
            }
                
            _buyUpgradesButton.gameObject.SetActive(true);
            _upgradeCostText.text = value.ToString();
        }

        private void UpdateCarCost(int value)
        {
            if (value == 0)
            {
                _buyCarButton.gameObject.SetActive(false);
                _upgradesPanel.SetActive(true);
                return;
            }
                
            _buyUpgradesButton.gameObject.SetActive(false);
            _upgradesPanel.SetActive(false);
            _buyCarButton.gameObject.SetActive(true);
            _carCostText.text = value.ToString();
        }

        private void Awake()
        {
            _backButton.onClick.AddListener(BackButtonClicked);
            _nextCarButton.onClick.AddListener(NextCarButtonClicked);
            _previousCarButton.onClick.AddListener(PreviousCarButtonClicked);
            _buyUpgradesButton.onClick.AddListener(BuyUpgradesButtonClicked);
            _buyCarButton.onClick.AddListener(BuyCarButtonClicked);
            _upgradeCustomizationTierButton.onClick.AddListener(UpgradeCustomizationTierButtonClicked);
            
            _garageShop.OnUpgradePriceChanged += UpdateUpgradeUpgradePrice;
            _garageShop.OnCarChanged += UpdateCarCost;
            _garageShop.OnMoneyChanged += UpdateMoneyText;
            
            for (int i = 0; i < CarColors.Length; i++)
            {
                var colorIndex = i;
                _onColorSelectedActions[i] = () => OnColorSelected?.Invoke(CarColors[colorIndex]);
                _colorButtons[i].onClick.AddListener(_onColorSelectedActions[i]);
            }
        }

        private void Start()
        {
            for (int i = 0; i < CarColors.Length; i++)
            {
                _colorButtons[i].GetComponent<Image>().color = CarColors[i];
            }
        }
        
        private void OnEnable()
        {
            UpdateMoneyText(_garageShop.GetMoney());
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(BackButtonClicked);
            _nextCarButton.onClick.RemoveListener(NextCarButtonClicked);
            _previousCarButton.onClick.RemoveListener(PreviousCarButtonClicked);
            _buyUpgradesButton.onClick.RemoveListener(BuyUpgradesButtonClicked);
            _buyCarButton.onClick.RemoveListener(BuyCarButtonClicked);
            _upgradeCustomizationTierButton.onClick.RemoveListener(UpgradeCustomizationTierButtonClicked);

            for (int i = 0; i < CarColors.Length; i++)
            {
                _colorButtons[i].onClick.RemoveListener(_onColorSelectedActions[i]);
            }
        }
    }
}