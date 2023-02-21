using System;
using System.Threading.Tasks;
using CodeBase.Logic.Enemy;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Logic.Enemy.Spawner;
using CodeBase.Logic.Enemy.Targets;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Monsters;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class EnemyFactory : IInitializable, IDisposable
    {
        private readonly AssetProviderService _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataProviderService _staticData;

        private bool _isWarmedUp;

        private GameObject _spawnerPrefab;

        [Inject]
        public EnemyFactory(AssetProviderService assetProviderService, IStaticDataProviderService staticData, IInstantiator instantiator,
            SceneLoader sceneLoader)
        {
            _assetProvider = assetProviderService;
            _staticData = staticData;
            _instantiator = instantiator;
            _sceneLoader = sceneLoader;
        }

        public void Dispose() =>
            Cleanup();

        public async void Initialize()
        {
            _sceneLoader.RegisterLoading();
            await Warmup();
            _sceneLoader.UnregisterLoading();
        }

        private async Task Warmup()
        {
            Assert.IsFalse(_isWarmedUp, "Factory is already warmed up. It should be cleanedUp before next warmup");
            _isWarmedUp = true;

            _spawnerPrefab = await _assetProvider.LoadAsync<GameObject>(AssetAddress.EnemySpawner, false);
        }

        private void Cleanup()
        {
            if (!_isWarmedUp) return;
            _isWarmedUp = false;

            Addressables.Release(_spawnerPrefab);
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

            //todo нужно держать все ссылки на созданных монстров и релизить их, подписавшись на DeathComponent. Либо даже добавить CleanupComponent
            GameObject prefab = await _assetProvider.LoadAsync(monsterData.PrefabReference);
            GameObject monsterGo = _instantiator.InstantiatePrefab(prefab, parent.position, Quaternion.identity, parent);

            var health = monsterGo.GetComponent<EnemyHealth>();
            health.Construct(monsterData.HealthData);

            monsterGo.GetComponent<ActorUI>().Construct(health);

            monsterGo.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            var attack = monsterGo.GetComponent<AttackTarget>();
            attack.Construct(monsterData.AttackData);

            var lootSpawner = monsterGo.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(monsterData.LootData);

            return monsterGo;
        }
    }
}