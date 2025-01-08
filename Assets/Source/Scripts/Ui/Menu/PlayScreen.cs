using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.Scripts.Camera;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.Scripts.Ui.Menu
{
    public class PlayScreen : MonoBehaviour
    {
        public event Action<string> OnLevelSelected;
        
        [SerializeField] private MainMenuUi _mainMenuUi;

        [Header("UI Elements")]
        [SerializeField] private Button _backButton;
        [SerializeField] private LevelButton[] _levelButtons;
        [SerializeField] private Transform _content;
        
        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        private void BackButtonClicked()
        {
            _mainMenuUi.ShowMenu();
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(BackButtonClicked);
            foreach (var levelButton in _levelButtons)
            {
                levelButton.OnClicked += OnLevelSelected;
            }
        }
        
        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(BackButtonClicked);
            foreach (var levelButton in _levelButtons)
            {
                levelButton.OnClicked -= OnLevelSelected;
            }
        }
    }
}