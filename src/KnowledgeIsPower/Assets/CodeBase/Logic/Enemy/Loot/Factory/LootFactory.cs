using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Logic.Enemy.Loot.Factory
{
    public class LootFactory : IWarmupable, IDisposable
    {
        private readonly AssetProviderService _assetProvider;
        private readonly PersistentProgressService _progressService;

        private bool _isWarmedUp;

        private GameObject _lootPrefab;

        [Inject]
        public LootFactory(AssetProviderService assetProviderService, PersistentProgressService progressService)
        {
            _assetProvider = assetProviderService;
            _progressService = progressService;
        }

        public void Dispose() =>
            Cleanup();

        public async Task Warmup()
        {
            Assert.IsFalse(_isWarmedUp, "Factory is already warmed up. It should be cleanedUp before next warmup");

            _isWarmedUp = true;

            _lootPrefab = await _assetProvider.LoadAsync<GameObject>(AssetAddress.Loot, false);
        }

        private void Cleanup()
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