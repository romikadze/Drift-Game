using System.Linq;
using Source.Scripts.Camera;
using Source.Scripts.Data;
using Source.Scripts.Drift;
using Source.Scripts.Game;
using Source.Scripts.Ui.Game;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private DriftPointsCounter _driftPointsCounter;
        [SerializeField] private Race _race;
        [SerializeField] private EndRaceScreen _endRaceScreen;
        [SerializeField] private MonoBehaviour[] _endRaceObjects;
        [SerializeField] private MonoBehaviour[] _startRaceObjects;

        public override void InstallBindings()
        {
            Container.Bind<RaceData>().AsSingle();
            Container.BindInterfacesAndSelfTo<RaceMoneyManager>().AsSingle();
            Container.Bind<Race>().FromInstance(_race).AsSingle();
            Container.Bind<EndRaceScreen>().FromInstance(_endRaceScreen).AsSingle();
            Container.Bind<CameraMovement>().FromInstance(_cameraMovement).AsSingle();
            Container.Bind<DriftPointsCounter>().FromInstance(_driftPointsCounter).AsSingle();
            Container.BindInterfacesAndSelfTo<IEndRace[]>().FromInstance(_endRaceObjects.OfType<IEndRace>().ToArray()).AsSingle();
            Container.BindInterfacesAndSelfTo<IStartRace[]>().FromInstance(_startRaceObjects.OfType<IStartRace>().ToArray()).AsSingle();
        }
    }
}
