using System;
using UnityEngine;

namespace Source.Scripts.Car
{
    public class CarBodyChanger : MonoBehaviour
    {
        [SerializeField] private CarBody[] _carBodies;
        
        private int _currentBodyIndex;
        private int _currentUpgrradeIndex;
        
        public string GetCarName()
        {
            return _carBodies[_currentBodyIndex].Name;
        }
        
        public void ChangeBody(int index)
        {
            _currentBodyIndex = index;
            foreach (var carBody in _carBodies)
            {
                carBody.Body.SetActive(false);
            }
            _carBodies[_currentBodyIndex].Body.SetActive(true);
        }
        
        public void ChangeBody(string carName)
        {
            foreach (var carBody in _carBodies)
            {
                carBody.Body.SetActive(carBody.Name == carName);
            }
            
            _currentBodyIndex = Array.FindIndex(_carBodies, body => body.Name == carName);
        }
        
        public void NextBody()
        {
            _currentBodyIndex++;
            if (_currentBodyIndex >= _carBodies.Length)
            {
                _currentBodyIndex = 0;
            }
            ChangeBody(_currentBodyIndex);
        }
        
        public void PreviousBody()
        {
            _currentBodyIndex--;
            if (_currentBodyIndex < 0)
            {
                _currentBodyIndex = _carBodies.Length - 1;
            }
            ChangeBody(_currentBodyIndex);
        }

        public void ChangeColor(Color color)
        {
            _carBodies[_currentBodyIndex].Renderer.material.color = color;
        }
        
        public void ChangeUpgrade(int index)
        {
            var upgradesLength = _carBodies[_currentBodyIndex].Upgrades.Length;
            
            if (index < 0 || index > upgradesLength)
                return;
            
            for (int i = 0; i < upgradesLength; i++)
            {
                _carBodies[_currentBodyIndex].Upgrades[i].SetActive(i < index);
            }
        }

        private void Start()
        {
            ChangeBody(_currentBodyIndex);
        }
    }
    
    [Serializable]
    public struct CarBody
    {
        public string Name;
        public GameObject Body;
        public MeshRenderer Renderer;
        public GameObject[] Upgrades;
    }
}