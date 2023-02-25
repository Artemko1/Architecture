using Zenject;

namespace CodeBase.Logic.Enemy.Factory
{
    public class EnemyFactoryInstaller : MonoInstaller<EnemyFactoryInstaller>
    {
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
    }
}