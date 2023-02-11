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
using UnityEngine.AI;

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

        public GameObject CreateHero(Vector3 initialHeroPosition)
        {
            GameObject heroGameObject = Instantiate(AssetPath.HeroPath, initialHeroPosition);
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

        public GameObject CreateHud()
        {
            GameObject hud = Instantiate(AssetPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.PlayerState.LootData);

            foreach (OpenWindowButton windowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                windowButton.Construct(_windowService);
            }

            ActivateProgressReaders(hud);
            return hud;
        }

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            GameObject monsterGo = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

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
            var lootPiece = Instantiate(AssetPath.Loot, at)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.PlayerState.LootData);

            ActivateProgressReaders(lootPiece.gameObject);

            return lootPiece;
        }

        public SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            var spawner = Instantiate(AssetPath.EnemySpawner, at)
                .GetComponent<SpawnPoint>();

            spawner.Construct(this, _saveLoadService, spawnerId, monsterTypeId);

            ActivateProgressReaders(spawner.gameObject);
            return spawner;
        }

        private GameObject Instantiate(string prefabPath) =>
            _assetProvider.Instantiate(prefabPath);

        private GameObject Instantiate(string prefabPath, Vector3 at) =>
            _assetProvider.Instantiate(prefabPath, at);

        private void ActivateProgressReaders(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                progressReader.ReadFromProgress(_progressService.Progress);
            }
        }
    }
}