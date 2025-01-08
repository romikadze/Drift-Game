using System;
using Cysharp.Threading.Tasks;
using Source.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Scripts.Ui.Game
{
    public class EndRaceScreen : UiScreen
    {
        public event Action OnExitClicked;
        
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private Button _doubleMoneyButton;
        [SerializeField] private Button _exitButton;
        
        private AdManager _adManager;
        private RaceData _raceData;
        
        private const string MONEY_TEXT = "Money collected: ";
        
        [Inject]
        private void Construct(AdManager adManager, RaceData raceData)
        {
            _adManager = adManager;
            _raceData = raceData;
        }
        
        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateMoneyText(int money)
        {
            _moneyText.text = MONEY_TEXT + money;
        }

        private void OnDoubleMoneyButtonClicked()
        {
            _adManager.ShowRewardedVideoAsync().Forget();
        }

        private void DisableButton(bool obj)
        {
            _doubleMoneyButton.interactable = false;
        }

        private void ExitClicked()
        {
            OnExitClicked?.Invoke();
        }

        private void Awake()
        {
            _doubleMoneyButton.onClick.AddListener(OnDoubleMoneyButtonClicked);
            _raceData.OnMoneyChanged += UpdateMoneyText;
            _adManager.OnRewarded += DisableButton;
            _exitButton.onClick.AddListener(ExitClicked);
        }

        private void OnEnable()
        {
            _doubleMoneyButton.interactable = _adManager.IsRewardedVideoReady();
            UpdateMoneyText(_raceData.Money);
        }

        private void OnDestroy()
        {
            _doubleMoneyButton.onClick.RemoveListener(OnDoubleMoneyButtonClicked);
            _raceData.OnMoneyChanged -= UpdateMoneyText;
            _adManager.OnRewarded += DisableButton;
            _exitButton.onClick.RemoveListener(ExitClicked);
        }
    }
}