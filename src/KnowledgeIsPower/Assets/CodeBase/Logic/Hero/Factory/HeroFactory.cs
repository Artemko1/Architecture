using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Services.AssetProvider;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Hero.Factory
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

        public async Task<GameObject> CreateHero(Vector3 initialHeroPosition, Transform parent = null)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(Constants.AssetAddress.HeroPath);

            GameObject hero = _instantiator.InstantiatePrefab(prefab, initialHeroPosition, Quaternion.identity, parent);

            hero
                .GetComponent<AddressableReleaser>()
                .Construct(_assetProvider, prefab);

            return hero;
        }
    }
}