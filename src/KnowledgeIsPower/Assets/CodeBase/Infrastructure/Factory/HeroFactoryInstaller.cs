using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class HeroFactoryInstaller : MonoInstaller<HeroFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log($"Binding HeroFactory");
            Container.Bind<HeroFactory>().AsSingle();
        }
    }
}