using Source.Scripts.Camera;
using Source.Scripts.Car;
using Source.Scripts.Menu;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private CarBodyChanger _carBodyChanger;
        [SerializeField] private MenuCamera _menuCamera;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SettingsManager>().AsSingle();
            Container.Bind<CarBodyChanger>().FromInstance(_carBodyChanger).AsSingle();
            Container.Bind<MenuCamera>().FromInstance(_menuCamera).AsSingle();
        }
    }
}