using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class EnemyFactoryInstaller : MonoInstaller<EnemyFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log($"Binding EnemyFactory");
            Container.Bind<EnemyFactory>().AsSingle();
        }
    }
}