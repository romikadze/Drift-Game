using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Source.Scripts.Camera
{
    public class MenuCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _rotateAroundSpeed = 3f;
        [SerializeField] private float _rotateUpDownSpeed = 2f;

        private bool _isRotating = true;
        private Vector3 _originalRotation;
        
        public void MoveAround()
        {
            _isRotating = true;
        }
        
        public async UniTask MoveUp()
        {
            _isRotating = false;
            _originalRotation = transform.rotation.eulerAngles;
            await transform.DORotate(_originalRotation + new Vector3(-90, 0, 0), _rotateUpDownSpeed)
                .SetEase(Ease.InQuad)
                .ToUniTask();
        }
        
        public async UniTask MoveDown()
        {
            _isRotating = false;
            await transform.DORotate(_originalRotation, _rotateUpDownSpeed)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _isRotating = true;
                })
                .ToUniTask();
        }
        
        public async UniTask RotateAroundOut(bool reverse = false)
        {
            _isRotating = false;
            _originalRotation = transform.rotation.eulerAngles;

            if(reverse)
                await transform.DORotate(_originalRotation + new Vector3(0, -90, 0), _rotateAroundSpeed/2)
                    .SetEase(Ease.InQuad)
                    .ToUniTask();
            else
                await transform.DORotate(_originalRotation + new Vector3(0, 90, 0), _rotateAroundSpeed/2)
                    .SetEase(Ease.InQuad)
                    .ToUniTask();
        }
        
        public async UniTask RotateAroundIn(bool reverse = false)
        {
            if (reverse)
            {
                await transform.DORotate(_originalRotation + new Vector3(0, 90, 0), 0).ToUniTask();
                await transform.DORotate(_originalRotation, _rotateAroundSpeed/2)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() => _isRotating = true).ToUniTask();

            }
            else
            {
                await transform.DORotate(_originalRotation + new Vector3(0, -90, 0), 0).ToUniTask();
                await transform.DORotate(_originalRotation, _rotateAroundSpeed/2)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => _isRotating = true).ToUniTask();
            }
        }
        
        private void Update()
        {
            if (_target == null || !_isRotating) return;
            
            transform.RotateAround(_target.position, Vector3.up, _rotationSpeed * Time.deltaTime);
            
            transform.DOLookAt(_target.position, 0.2f);
        }
    }
}