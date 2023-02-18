using UnityEngine;
using Zenject;

namespace CodeBase.Services
{
    public class AllServices
    {
        private readonly DiContainer _diContainer;

        [Inject]
        public AllServices(DiContainer diContainer)
        {
            _diContainer = diContainer;
            Container = this;
        }

        public static AllServices Container { get; private set; }

        public TService Single<TService>() where TService : IService
        {
            Debug.LogWarning($"Resolving {typeof(TService)} from container...");
            return _diContainer.Resolve<TService>();
        }
    }
}