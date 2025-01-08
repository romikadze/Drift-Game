using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Ui.Menu
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        public event Action<string> OnClicked; 
        
        [SerializeField] private Button _button;
        [SerializeField] private string _levelName;

        private void ButtonClicked()
        {
            OnClicked?.Invoke(_levelName);
        }

        private void Awake()
        {
            _button.onClick.AddListener(ButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ButtonClicked);
        }
    }
}