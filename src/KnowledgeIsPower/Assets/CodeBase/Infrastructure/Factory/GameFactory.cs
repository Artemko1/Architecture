using System.Threading.Tasks;
using CodeBase.Logic;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Logic.Enemy.Spawner;
using CodeBase.Logic.Enemy.Targets;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Monsters;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory
    {
        private readonly AssetProviderService _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly PersistentProgressService _progressService;
        private readonly RandomService _randomService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataProviderService _staticData;
        private readonly WindowService _windowService;
        private bool _isWarmedUp;

        private GameObject _lootPrefab;
        private GameObject _spawnerPrefab;

        [Inject]
        public GameFactory(AssetProviderService assetProviderService, IStaticDataProviderService staticData, RandomService randomService,
            PersistentProgressService progressService, ISaveLoadService saveLoadService, WindowService windowService,
            IInstantiator instantiator)
        {
            _assetProvider = assetProviderService;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _windowService = windowService;
            _instantiator = instantiator;
        }

        public async Task Warmup()
        {
            Assert.IsFalse(_isWarmedUp, "Factory is already warmed up. It should be cleanedUp before next warmup");

            _isWarmedUp = true;

            Task<GameObject> loadLoot = _assetProvider.LoadAsync<GameObject>(AssetAddress.Loot, false);
            Task<GameObject> loadSpawner = _assetProvider.LoadAsync<GameObject>(AssetAddress.EnemySpawner, false);
            await Task.WhenAll(loadLoot, loadSpawner);

            _lootPrefab = loadLoot.Result;
            _spawnerPrefab = loadSpawner.Result;
        }

        public void Cleanup()
        {
            if (!_isWarmedUp) return;

            _isWarmedUp = false;

            Addressables.Release(_lootPrefab); // Работает отлично
            _lootPrefab = null;
            Addressables.Release(_spawnerPrefab);
            _spawnerPrefab = null;
        }

        public async Task<GameObject> CreateHero(Vector3 initialHeroPosition, Transform parent)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(AssetAddress.HeroPath);

            return _instantiator.InstantiatePrefab(prefab, initialHeroPosition, Quaternion.identity, parent);
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

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);

            //todo нужно держать все ссылки на созданных монстров и релизить их, подписавшись на DeathComponent. Либо даже добавить CleanupComponent
            GameObject prefab = await _assetProvider.LoadAsync(monsterData.PrefabReference);
            GameObject monsterGo = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            // todo передалать на спаун из контейреа, чтобы убрать из IHealth метод Construct
            {
                var health = monsterGo.GetComponent<IHealth>();
                health.Construct(monsterData.HealthData);

                monsterGo.GetComponent<ActorUI>().Construct(health);
            }

            monsterGo.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            {
                var attack = monsterGo.GetComponent<AttackTarget>();
                attack.Construct(monsterData.AttackData);
            }

            var lootSpawner = monsterGo.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService, monsterData.LootData);

            return monsterGo;
        }

        public LootPiece CreateLoot(Vector3 at)
        {
            var lootPiece = Object.Instantiate(_lootPrefab, at, Quaternion.identity)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.PlayerState.LootData);

            return lootPiece;
        }

        public void CreateSpawner(EnemySpawnerData spawnerData)
        {
            var spawner = Object.Instantiate(_spawnerPrefab, spawnerData.Position, Quaternion.identity)
                .GetComponent<SpawnPoint>();

            spawner.Construct(this, _saveLoadService, _progressService, spawnerData.Id, spawnerData.MonsterTypeId);
        }
    }
}