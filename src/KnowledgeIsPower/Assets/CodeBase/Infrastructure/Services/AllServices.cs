using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class AllServices
    {
        private static AllServices _instance;
        public static AllServices Container => _instance ??= new AllServices();

        public void RegisterSingle<TService>(TService implementation) where TService : IService
        {
            Implementation<TService>.ServiceInstance = implementation;
        }

        public TService Single<TService>() where TService : IService
        {
            TService serviceInstance = Implementation<TService>.ServiceInstance;
            if (serviceInstance == null)
            {
                Debug.LogWarning($"Requested service of type {typeof(TService)} is not registered");
            }

            return serviceInstance;
        }

        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}