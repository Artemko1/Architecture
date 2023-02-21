using System.Threading.Tasks;
using CodeBase.Services.AssetProvider;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class HeroFactory
    {
        private readonly AssetProviderService _assetProvider;
        private readonly IInstantiator _instantiator;

        [Inject]
        public HeroFactory(AssetProviderService assetProviderService, IInstantiator instantiator)
        {
            _assetProvider = assetProviderService;
            _instantiator = instantiator;
        }

        public async Task<GameObject> CreateHero(Vector3 initialHeroPosition, Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(AssetAddress.HeroPath);

            return _instantiator.InstantiatePrefab(prefab, initialHeroPosition, Quaternion.identity, parent);
        }
    }
}