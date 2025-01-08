using Source.Scripts.Game;
using UnityEngine;

namespace Source.Scripts.Camera
{
    public class CameraMovement : MonoBehaviour, IStartRace, IEndRace
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _initialOffset;
        [SerializeField] private float _smoothSpeed = 1f;
        [SerializeField] private float _rotationSpeed = 5.0f;
        [SerializeField] private float _followDelay = 0.5f;
        [SerializeField] private float _maxDistance = 5f;

        private Vector3 _currentVelocity;

        private Vector3 _offset;

        public void StartRace()
        {
            enabled = true;
        }

        public void EndRace()
        {
            enabled = false;
        }
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void FollowTarget()
        {
            _offset = _target.forward * _initialOffset.z + _target.right * _initialOffset.x + _target.up * _initialOffset.y;

            Vector3 desiredPosition = _target.position + _offset;
            float currentSmoothSpeed = _offset.magnitude > _maxDistance ? _offset.magnitude/_maxDistance * _smoothSpeed : _smoothSpeed;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, 1/currentSmoothSpeed);
            transform.position = smoothedPosition;
        }

        private void RotateCamera()
        {
            Vector3 direction = _target.forward;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime * _followDelay);
        }

        private void Start()
        {
            _offset = _initialOffset;
        }

        private void Update()
        {
            FollowTarget();
            RotateCamera();
        }
    }
}