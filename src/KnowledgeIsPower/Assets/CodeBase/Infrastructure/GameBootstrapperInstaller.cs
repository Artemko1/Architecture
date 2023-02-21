using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapperInstaller : MonoInstaller<GameBootstrapperInstaller>
    {
        public override void InstallBindings() =>
            Container.Bind<IInitializable>().To<GameBootstrapper>().AsSingle();
    }
}