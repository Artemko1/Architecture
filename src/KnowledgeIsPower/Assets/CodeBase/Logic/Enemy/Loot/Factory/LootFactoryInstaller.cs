using Zenject;

namespace CodeBase.Logic.Enemy.Loot.Factory
{
    public class LootFactoryInstaller : MonoInstaller<LootFactoryInstaller>
    {
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<LootFactory>().AsSingle();
    }
}