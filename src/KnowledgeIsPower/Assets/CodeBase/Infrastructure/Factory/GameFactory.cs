using CodeBase.Logic;
using CodeBase.Logic.Enemy;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Logic.Enemy.Spawner;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Monsters;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProviderService _assetProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly IRandomService _randomService;
        private readonly IStaticDataProviderService _staticData;

        private GameObject _heroGameObject;

        public GameFactory(IAssetProviderService assetProviderService, IStaticDataProviderService staticData, IRandomService randomService,
            IPersistentProgressService progressService)
        {
            _assetProvider = assetProviderService;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
        }

        public GameObject CreateHero(Vector3 initialPoint)
        {
            _heroGameObject = InstantiateRegistered(AssetPath.HeroPath, initialPoint);
            ActivateProgressReaders(_heroGameObject);
            return _heroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);

            ActivateProgressReaders(hud);
            return hud;
        }

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            GameObject monsterGo = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

            {
                var health = monsterGo.GetComponent<IHealth>();
                health.Max = monsterData.Hp;
                health.Current = monsterData.Hp;
                monsterGo.GetComponent<ActorUI>().Construct(health);
            }


            monsterGo.GetComponent<AgentMoveToHero>().Construct(_heroGameObject.transform);
            monsterGo.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            {
                var attack = monsterGo.GetComponent<Attack>();
                attack.Construct(_heroGameObject.transform);
                attack.Damage = monsterData.Damage;
                attack.Cleavage = monsterData.AttackCleavage;
                attack.EffectiveDistance = monsterData.AttackEffectiveDistance;
            }

            monsterGo.GetComponent<RotateToHero>()?.Construct(_heroGameObject.transform);

            var lootSpawner = monsterGo.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);

            return monsterGo;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegistered(AssetPath.Loot)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);

            ActivateProgressReaders(lootPiece.gameObject);

            return lootPiece;
        }

        public SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            var spawner = InstantiateRegistered(AssetPath.EnemySpawner, at)
                .GetComponent<SpawnPoint>();

            spawner.Construct(this);
            spawner.ID = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;

            ActivateProgressReaders(spawner.gameObject);
            return spawner;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assetProvider.Instantiate(prefabPath);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assetProvider.Instantiate(prefabPath, at);
            return gameObject;
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