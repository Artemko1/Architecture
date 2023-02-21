using System.Threading.Tasks;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class HudFactory
    {
        private readonly AssetProviderService _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly PersistentProgressService _progressService;
        private readonly WindowService _windowService;

        [Inject]
        public HudFactory(AssetProviderService assetProviderService, PersistentProgressService progressService,
            WindowService windowService, IInstantiator instantiator)
        {
            _assetProvider = assetProviderService;
            _progressService = progressService;
            _windowService = windowService;
            _instantiator = instantiator;
        }

        public async Task<GameObject> CreateHud()
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(AssetAddress.HudPath);
            GameObject hud = Object.Instantiate(prefab);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.PlayerState.LootData);

            foreach (OpenWindowButton windowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                windowButton.Construct(_windowService);
            }

            return hud;
        }
    }
}