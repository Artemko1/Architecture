using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class FactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log($"Start install bindings. Current scene is {SceneManager.GetActiveScene().name}");
            Container.Bind<GameFactory>().AsSingle();
        }
    }
}