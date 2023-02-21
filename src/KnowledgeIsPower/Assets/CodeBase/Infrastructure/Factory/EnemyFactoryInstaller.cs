using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class EnemyFactoryInstaller : MonoInstaller<EnemyFactoryInstaller>
    {
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
    }
}