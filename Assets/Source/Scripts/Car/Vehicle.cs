using System;
using Photon.Pun;
using Source.Scripts.Car.Input;
using Source.Scripts.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Car
{
    public class Vehicle : MonoBehaviourPun, IStartRace, IEndRace, IPunObservable
    {  
        public event Action OnDriftStart;
        public event Action OnDriftEnd;
        
        [Space(15)][Header("General")]
        [SerializeField] private Wheel[] _wheels;
        [SerializeField] private Vector3 _centerOfMass;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MobileControls _mobileControls;
        
        [Space(15)][Header("Vehicle Settings")]
        [SerializeField] private float _motorForce = 500;
        [SerializeField] private float _brakeForce = 250;
        [SerializeField] private float _handBrakeForce = 600;
        
        [Space(15)][Header("Drift Settings")]
        [SerializeField] private float _sidewaysFrictionMultiplierFront = 0.8f;
        [SerializeField] private float _sidewaysFrictionMultiplierBack = 0.5f;
        [SerializeField] private float _forwardFrictionMultiplier = 0.65f;

        [Range(0,0.5f)][SerializeField] private float _autoSteer = 0.2f;

        private const float DRIFT_ANGLE_MULTIPLIER = 50;
        private float _steerAngle;
        private float _throttle;
        private float _brake;
        private float _handBrake;
        private float _driftAngle;
        private float[] _wheelForwardFrictions;
        private float[] _wheelSidewaysFrictions;
        private InputSystem_Actions _inputActions;
        private PhotonView _photonView;
        private bool _isDrifting;
        
        private Vector3 _networkPosition;
        private Quaternion _networkRotation;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                _networkPosition = (Vector3)stream.ReceiveNext();
                _networkRotation = (Quaternion)stream.ReceiveNext();
            }
        }

        public void StartRace()
        {
            enabled = true;
        }

        public void EndRace()
        {
            enabled = false;

            foreach (var wheel in _wheels)
            {
                wheel.SetThrottle(0);
                wheel.SetBrake(1);
                wheel.SetHandBrake(1);
            }
        }

        private bool ToggleDrifting(bool newValue)
        {
            if(_isDrifting == newValue) return false;
            
            _isDrifting = newValue;
            if (_isDrifting)
            {
                OnDriftStart?.Invoke();
            }
            else
            {
                OnDriftEnd?.Invoke();
            }
            return true;
        }

        private void ApplyInput()
        {
            foreach (var wheel in _wheels)
            {
                wheel.SetSteer(Mathf.Clamp(_steerAngle + _driftAngle, -1f, 1f));
                wheel.SetThrottle(_throttle);
                wheel.SetBrake(_brake);
                wheel.SetHandBrake(_handBrake);
            }
        }

        private void ApplyDrift()
        {
            if (_isDrifting)
            {
                ApplyAutoSteer();
            }
            
            if (!ToggleDrifting(IsDrifting()))
                return;
            
            for (int i = 0; i < _wheels.Length; i++)
            {
                float sidewaysMultiplier = i < 2 ? _sidewaysFrictionMultiplierFront : _sidewaysFrictionMultiplierBack;
                
                if (_isDrifting)
                {
                    _wheels[i].SetFriction(_wheelForwardFrictions[i] * _forwardFrictionMultiplier, _wheelSidewaysFrictions[i] * sidewaysMultiplier);
                }
                else
                {
                    _wheels[i].SetFriction(_wheelForwardFrictions[i], _wheelSidewaysFrictions[i]);
                }
            }
        }

        private void ApplyAutoSteer()
        {
            Vector3 velocity = _rigidbody.velocity;
            _driftAngle = 0;
            if(velocity.magnitude < 10) return;
            
            Vector3 forwardDirection = transform.forward;
            Vector3 velocityDirection = velocity.normalized;
            
            _driftAngle = Vector3.SignedAngle(forwardDirection, velocityDirection, Vector3.up) / DRIFT_ANGLE_MULTIPLIER;
            
            _driftAngle = Mathf.Clamp(_driftAngle, -_autoSteer, _autoSteer);
        }

        private bool IsDrifting()
        {
            int wheelsSlipping = 0;

            foreach (var wheel in _wheels)
            {
                if(wheel.IsSlipping())
                {
                    wheelsSlipping++;
                }
            }
            
            return wheelsSlipping >= 2;
        }

        private void OnSteer(InputAction.CallbackContext context)
        {
            _steerAngle = context.ReadValue<float>();
        }

        private void OnThrottle(InputAction.CallbackContext context)
        {
            _throttle = context.ReadValue<float>() * _motorForce;
        }

        private void OnBrake(InputAction.CallbackContext context)
        {
            _brake = context.ReadValue<float>() * _brakeForce;
        }

        private void OnHandbrake(InputAction.CallbackContext obj)
        {
            _handBrake = obj.ReadValue<float>() * _handBrakeForce;
        }

        private void OnHandbrakeCanceled(InputAction.CallbackContext obj)
        {
            _handBrake = 0;
        }

        private void OnBrakeCanceled(InputAction.CallbackContext obj)
        {
            _brake = 0;
        }

        private void OnThrottleCanceled(InputAction.CallbackContext obj)
        {
            _throttle = 0;
        }

        private void OnSteerCanceled(InputAction.CallbackContext obj)
        {
            _steerAngle = 0;
        }
        
        private void OnSteer(float value)
        {
            _steerAngle = value;
        }

        private void OnThrottle(float value)
        {
            _throttle = value * _motorForce;
        }

        private void OnHandbrake()
        {
            _handBrake = _handBrakeForce;
        }

        private void OnHandbrakeCanceled()
        {
            _handBrake = 0;
        }

        private void OnThrottleCanceled()
        {
            _throttle = 0;
        }

        private void OnSteerCanceled()
        {
            _steerAngle = 0;
        }

        private void Awake()
        {
            _inputActions = new InputSystem_Actions();
            _photonView = GetComponent<PhotonView>();
            
            _inputActions.Enable();
            _inputActions.Car.Steer.performed += OnSteer;
            _inputActions.Car.Steer.canceled += OnSteerCanceled;
            _inputActions.Car.Throttle.performed += OnThrottle;
            _inputActions.Car.Throttle.canceled += OnThrottleCanceled;
            _inputActions.Car.Brake.performed += OnBrake;
            _inputActions.Car.Brake.canceled += OnBrakeCanceled;
            _inputActions.Car.HandBrake.performed += OnHandbrake;
            _inputActions.Car.HandBrake.canceled += OnHandbrakeCanceled;
            
            if(_mobileControls == null) return;
            _mobileControls.OnSteer += OnSteer;
            _mobileControls.OnSteerCanceled += OnSteerCanceled;
            _mobileControls.OnThrottle += OnThrottle;
            _mobileControls.OnThrottleCanceled += OnThrottleCanceled;
            _mobileControls.OnHandbrake += OnHandbrake;
            _mobileControls.OnHandbrakeCanceled += OnHandbrakeCanceled;
        }

        private void Start()
        {
            _rigidbody.centerOfMass = _centerOfMass;
            _wheelForwardFrictions = new float[_wheels.Length];
            _wheelSidewaysFrictions = new float[_wheels.Length];
            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheelForwardFrictions[i] = _wheels[i].GetForwardFriction();
                _wheelSidewaysFrictions[i] = _wheels[i].GetSidewaysFriction();
            }
        }

        private void Update()
        {
            if (_photonView.IsMine)
            {
                ApplyInput();
                ApplyDrift();
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, _networkPosition, Time.deltaTime * 10);
                transform.rotation = Quaternion.Lerp(transform.rotation, _networkRotation, Time.deltaTime * 10);
            }
        }

        private void OnDestroy()
        {
            _inputActions.Car.Steer.performed -= OnSteer;
            _inputActions.Car.Steer.canceled -= OnSteerCanceled;
            _inputActions.Car.Throttle.performed -= OnThrottle;
            _inputActions.Car.Throttle.canceled -= OnThrottleCanceled;
            _inputActions.Car.Brake.performed -= OnBrake;
            _inputActions.Car.Brake.canceled -= OnBrakeCanceled;
            _inputActions.Car.HandBrake.performed -= OnHandbrake;
            _inputActions.Car.HandBrake.canceled -= OnHandbrakeCanceled;
            _inputActions.Disable();
            
            if(_mobileControls == null) return;
            _mobileControls.OnSteer -= OnSteer;
            _mobileControls.OnSteerCanceled -= OnSteerCanceled;
            _mobileControls.OnThrottle -= OnThrottle;
            _mobileControls.OnThrottleCanceled -= OnThrottleCanceled;
            _mobileControls.OnHandbrake -= OnHandbrake;
            _mobileControls.OnHandbrakeCanceled -= OnHandbrakeCanceled;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_rigidbody == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + _centerOfMass, 0.1f);
        }
#endif
    }
}
