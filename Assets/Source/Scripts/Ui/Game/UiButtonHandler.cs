using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Scripts.Ui.Game
{
    public class UIButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnButtonPressed;
        public event Action OnButtonReleased;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnButtonPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnButtonReleased?.Invoke();
        }
    }

}