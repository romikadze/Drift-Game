using Source.Scripts.Data;
using Zenject;

namespace Source.Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<User>().AsSingle();
            Container.BindInterfacesAndSelfTo<AdManager>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
        }
    }
}