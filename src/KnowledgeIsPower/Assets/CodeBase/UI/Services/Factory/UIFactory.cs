using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Services;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IService
    {
        private readonly AssetProviderService _assets;
        private readonly PersistentProgressService _progressService;
        private readonly IStaticDataProviderService _staticData;

        private Transform _uiRoot;

        public UIFactory(AssetProviderService assets, IStaticDataProviderService staticData, PersistentProgressService progressService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            WindowBase window = Object.Instantiate(config.Prefab, _uiRoot);
            window.Construct(_progressService);
        }

        public async Task CreateUIRoot()
        {
            var prefab = await _assets.LoadAsync<GameObject>(AssetAddress.UIRoot);
            _uiRoot = Object.Instantiate(prefab).transform;
        }
    }
}