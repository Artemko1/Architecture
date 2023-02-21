using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class LootFactoryInstaller : MonoInstaller<LootFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log($"Binding LootFactory");
            Container.Bind<LootFactory>().AsSingle();
        }
    }
}