using UnityEngine;

namespace Source.Scripts.Car
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private WheelCollider _wheelCollider;
        [SerializeField] private Transform _wheelMesh;
        
        [Range(0, 20)][SerializeField] private float _steerSpeed = 5;
        [Range(0,50)][SerializeField] private float _steerAngle = 45;
        [SerializeField] private bool _useBrake;
        [Range(0,1)][SerializeField] private float _brakeMultiplier;
        [SerializeField] private bool _useHandBrake;
        [SerializeField] private bool _useThrottle;
        [Range(0,1)][SerializeField] private float _throttleMultiplier;

        [Header("Drift Settings")] 
        [SerializeField] private float _forwardSlip = 0.7f;
        [SerializeField] private float _sidewaysSlip = 0.4f;
        
        public void SetThrottle(float throttle)
        {
            if (_useThrottle)
            {
                _wheelCollider.motorTorque = throttle * _throttleMultiplier;
            }
        }
        
        public void SetBrake(float brake)
        {
            if (_useBrake)
            {
                _wheelCollider.brakeTorque = brake * _brakeMultiplier;
            }
        }
        
        public void SetHandBrake(float handBrake)
        {
            if (_useHandBrake)
            {
                _wheelCollider.brakeTorque = handBrake;
            }
        }
        
        public void SetSteer(float steer)
        {
            _wheelCollider.steerAngle = Mathf.Lerp(_wheelCollider.steerAngle, steer * _steerAngle, Time.deltaTime * _steerSpeed);
        }
        
        public void SetFriction(float forwardFriction, float sidewaysFriction)
        {
            WheelFrictionCurve forwardCurve = _wheelCollider.forwardFriction;
            WheelFrictionCurve sidewaysCurve = _wheelCollider.sidewaysFriction;

            forwardCurve.stiffness = forwardFriction;
            sidewaysCurve.stiffness = sidewaysFriction;

            _wheelCollider.forwardFriction = forwardCurve;
            _wheelCollider.sidewaysFriction = sidewaysCurve;
        }
        
        public float GetForwardFriction()
        {
            return _wheelCollider.forwardFriction.stiffness;
        }
        
        public float GetSidewaysFriction()
        {
            return _wheelCollider.sidewaysFriction.stiffness;
        }
        
        public bool IsSlipping()
        {
            if (_wheelCollider.GetGroundHit(out WheelHit hit))
            {
                float forwardSlip = Mathf.Abs(hit.forwardSlip);
                float sidewaysSlip = Mathf.Abs(hit.sidewaysSlip);
                
                if (forwardSlip > _forwardSlip || sidewaysSlip > _sidewaysSlip)
                {
                    return true;
                }
            }
            return false;
        }
        
        private void Update()
        {
            UpdateWheelVisualRotation();
        }

        private void UpdateWheelVisualRotation()
        {
            _wheelCollider.GetWorldPose(out var position, out var rotation);
            _wheelMesh.rotation = rotation;
        }
    }
}