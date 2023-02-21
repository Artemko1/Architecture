using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class LootFactoryInstaller : MonoInstaller<LootFactoryInstaller>
    {
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<LootFactory>().AsSingle();
    }
}