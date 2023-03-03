using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IWarmupable
    {
        private readonly AssetProviderService _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly StaticDataProviderService _staticData;

        private Transform _uiRoot;

        [Inject]
        public UIFactory(AssetProviderService assetProvider, StaticDataProviderService staticData, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _instantiator = instantiator;
        }

        public Task Warmup() =>
            CreateUIRoot();

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            _instantiator.InstantiatePrefab(config.Prefab, _uiRoot);
        }

        private async Task CreateUIRoot()
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(Constants.AssetAddress.UIRoot);
            _uiRoot = Object.Instantiate(prefab).transform;

            _uiRoot
                .GetComponent<AddressableReleaser>()
                .Construct(_assetProvider, prefab);
        }
    }
}