using System.Threading.Tasks;
using CodeBase.Logic;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Logic.Enemy.Spawner;
using CodeBase.Logic.Enemy.Targets;
using CodeBase.Logic.Hero;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Hero;
using CodeBase.StaticData.Monsters;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProviderService _assetProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly IRandomService _randomService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataProviderService _staticData;
        private readonly IWindowService _windowService;
        private bool _isWarmedUp;

        private GameObject _lootPrefab;
        private GameObject _spawnerPrefab;

        public GameFactory(IAssetProviderService assetProviderService, IStaticDataProviderService staticData, IRandomService randomService,
            IPersistentProgressService progressService, ISaveLoadService saveLoadService, IWindowService windowService)
        {
            _assetProvider = assetProviderService;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _windowService = windowService;
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

        public async Task<GameObject> CreateHero(Vector3 initialHeroPosition)
        {
            var prefab = await _assetProvider.LoadAsync<GameObject>(AssetAddress.HeroPath);
            GameObject heroGameObject = Object.Instantiate(prefab, initialHeroPosition, Quaternion.identity);
            HeroStaticData heroStaticData = _staticData.ForHero();

            heroGameObject
                .GetComponent<HeroHealth>()
                .Construct(heroStaticData.HealthData);
            heroGameObject
                .GetComponent<HeroAttack>()
                .Construct(heroStaticData.AttackData);

            ActivateProgressReaders(heroGameObject);
            return heroGameObject;
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

            ActivateProgressReaders(hud);
            return hud;
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);

            //todo нужно держать все ссылки на созданных монстров и релизить их, подписавшись на DeathComponent. Либо даже добавить CleanupComponent
            GameObject prefab = await _assetProvider.LoadAsync(monsterData.PrefabReference);
            GameObject monsterGo = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

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

            ActivateProgressReaders(lootPiece.gameObject);

            return lootPiece;
        }

        public void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            var spawner = Object.Instantiate(_spawnerPrefab, at, Quaternion.identity)
                .GetComponent<SpawnPoint>();

            spawner.Construct(this, _saveLoadService, spawnerId, monsterTypeId);

            ActivateProgressReaders(spawner.gameObject);
        }

        private void ActivateProgressReaders(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                progressReader.ReadFromProgress(_progressService.Progress);
            }
        }
    }
}