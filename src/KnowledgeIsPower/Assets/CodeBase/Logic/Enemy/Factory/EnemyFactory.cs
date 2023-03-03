using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Logic.Enemy.Spawner;
using CodeBase.Logic.Enemy.Targets;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Monsters;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Zenject;

namespace CodeBase.Logic.Enemy.Factory
{
    public class EnemyFactory : IWarmupable, IDisposable
    {
        private readonly AssetProviderService _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataProviderService _staticData;

        private bool _isWarmedUp;

        private GameObject _spawnerPrefab;

        [Inject]
        public EnemyFactory(AssetProviderService assetProviderService, IStaticDataProviderService staticData, IInstantiator instantiator)
        {
            _assetProvider = assetProviderService;
            _staticData = staticData;
            _instantiator = instantiator;
        }

        public void Dispose() =>
            Cleanup();

        public async Task Warmup()
        {
            Assert.IsFalse(_isWarmedUp, "Factory is already warmed up. It should be cleanedUp before next warmup");
            _isWarmedUp = true;

            _spawnerPrefab = await _assetProvider.LoadAsync<GameObject>(Constants.AssetAddress.EnemySpawner);
        }

        private void Cleanup()
        {
            if (!_isWarmedUp) return;
            _isWarmedUp = false;

            _assetProvider.PendForRelease(_spawnerPrefab);
            _spawnerPrefab = null;
        }

        public void CreateSpawner(EnemySpawnerData spawnerData, Transform parent)
        {
            var spawner = _instantiator.InstantiatePrefabForComponent<SpawnPoint>(_spawnerPrefab, spawnerData.Position, Quaternion.identity,
                parent);

            spawner.Construct(spawnerData.Id, spawnerData.MonsterTypeId);
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);

            GameObject prefab = await _assetProvider.LoadAsync(monsterData.PrefabReference);
            GameObject monsterGo = _instantiator.InstantiatePrefab(prefab, parent.position, Quaternion.identity, parent);

            monsterGo
                .GetComponent<AddressableReleaser>()
                .Construct(_assetProvider, prefab);

            var health = monsterGo.GetComponent<EnemyHealth>();
            health.Construct(monsterData.HealthData);

            monsterGo
                .GetComponent<ActorUI>()
                .Construct(health);

            monsterGo
                .GetComponent<NavMeshAgent>()
                .speed = monsterData.MoveSpeed;

            monsterGo
                .GetComponent<AttackTarget>()
                .Construct(monsterData.AttackData);

            monsterGo
                .GetComponentInChildren<LootSpawner>()
                .Construct(monsterData.LootData);

            return monsterGo;
        }
    }
}