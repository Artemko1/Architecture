using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Elements;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Hud
{
    public class HudFactory
    {
        private readonly AssetProviderService _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly PersistentProgressService _progressService;

        [Inject]
        public HudFactory(AssetProviderService assetProviderService, PersistentProgressService progressService, IInstantiator instantiator)
        {
            _assetProvider = assetProviderService;
            _progressService = progressService;
            _instantiator = instantiator;
        }

        public async Task<GameObject> CreateHud()
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(AssetAddress.HudPath);
            GameObject hud = _instantiator.InstantiatePrefab(prefab);
            hud
                .GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.PlayerState.LootData);
            hud
                .GetComponent<AddressableReleaser>()
                .Construct(prefab);
            return hud;
        }
    }
}