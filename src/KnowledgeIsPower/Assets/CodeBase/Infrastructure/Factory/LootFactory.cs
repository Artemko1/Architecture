using System.Threading.Tasks;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class LootFactory
    {
        private readonly AssetProviderService _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly PersistentProgressService _progressService;

        private bool _isWarmedUp;

        private GameObject _lootPrefab;

        [Inject]
        public LootFactory(AssetProviderService assetProviderService, PersistentProgressService progressService, IInstantiator instantiator)
        {
            _assetProvider = assetProviderService;
            _progressService = progressService;
            _instantiator = instantiator;
        }

        public async Task Warmup()
        {
            Assert.IsFalse(_isWarmedUp, "Factory is already warmed up. It should be cleanedUp before next warmup");

            _isWarmedUp = true;

            _lootPrefab = await _assetProvider.LoadAsync<GameObject>(AssetAddress.Loot, false);
        }

        public void Cleanup()
        {
            if (!_isWarmedUp) return;

            _isWarmedUp = false;

            Addressables.Release(_lootPrefab); // Работает отлично
            _lootPrefab = null;
        }


        public LootPiece CreateLoot(Vector3 at)
        {
            var lootPiece = Object.Instantiate(_lootPrefab, at, Quaternion.identity)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.PlayerState.LootData);

            return lootPiece;
        }
    }
}