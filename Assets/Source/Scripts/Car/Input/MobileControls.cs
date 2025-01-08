using System;
using Source.Scripts.Ui.Game;
using UnityEngine;

namespace Source.Scripts.Car.Input
{
     public class MobileControls : MonoBehaviour
    {
        public event Action<float> OnSteer;
        public event Action<float> OnThrottle;
        public event Action OnHandbrake;

        public event Action OnSteerCanceled;
        public event Action OnThrottleCanceled;
        public event Action OnHandbrakeCanceled;

        [SerializeField] private UIButtonHandler _leftButton;
        [SerializeField] private UIButtonHandler _rightButton;
        [SerializeField] private UIButtonHandler _gasButton;
        [SerializeField] private UIButtonHandler _reverseButton;
        [SerializeField] private UIButtonHandler _handbrakeButton;

        private void Awake()
        {
            _leftButton.OnButtonPressed += HandleLeftButtonPressed;
            _leftButton.OnButtonReleased += HandleSteerCanceled;

            _rightButton.OnButtonPressed += HandleRightButtonPressed;
            _rightButton.OnButtonReleased += HandleSteerCanceled;

            _gasButton.OnButtonPressed += HandleGasButtonPressed;
            _gasButton.OnButtonReleased += HandleThrottleCanceled;

            _reverseButton.OnButtonPressed += HandleReverseButtonPressed;
            _reverseButton.OnButtonReleased += HandleThrottleCanceled;

            _handbrakeButton.OnButtonPressed += HandleHandbrakeButtonPressed;
            _handbrakeButton.OnButtonReleased += HandleHandbrakeCanceled;
        }

        private void OnDestroy()
        {
            _leftButton.OnButtonPressed -= HandleLeftButtonPressed;
            _leftButton.OnButtonReleased -= HandleSteerCanceled;

            _rightButton.OnButtonPressed -= HandleRightButtonPressed;
            _rightButton.OnButtonReleased -= HandleSteerCanceled;

            _gasButton.OnButtonPressed -= HandleGasButtonPressed;
            _gasButton.OnButtonReleased -= HandleThrottleCanceled;

            _reverseButton.OnButtonPressed -= HandleReverseButtonPressed;
            _reverseButton.OnButtonReleased -= HandleThrottleCanceled;

            _handbrakeButton.OnButtonPressed -= HandleHandbrakeButtonPressed;
            _handbrakeButton.OnButtonReleased -= HandleHandbrakeCanceled;
        }

        private void HandleLeftButtonPressed()
        {
            OnSteer?.Invoke(-1);
        }

        private void HandleRightButtonPressed()
        {
            OnSteer?.Invoke(1);
        }

        private void HandleGasButtonPressed()
        {
            OnThrottle?.Invoke(1);
        }

        private void HandleReverseButtonPressed()
        {
            OnThrottle?.Invoke(-1);
        }

        private void HandleHandbrakeButtonPressed()
        {
            OnHandbrake?.Invoke();
        }

        private void HandleSteerCanceled()
        {
            OnSteerCanceled?.Invoke();
        }

        private void HandleThrottleCanceled()
        {
            OnThrottleCanceled?.Invoke();
        }

        private void HandleHandbrakeCanceled()
        {
            OnHandbrakeCanceled?.Invoke();
        }
    }
}