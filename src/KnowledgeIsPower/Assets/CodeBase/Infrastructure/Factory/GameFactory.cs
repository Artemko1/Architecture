﻿using CodeBase.Logic;
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
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataProviderService _staticData;

        public GameFactory(IAssetProviderService assetProviderService, IStaticDataProviderService staticData, IRandomService randomService,
            IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _assetProvider = assetProviderService;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public GameObject CreateHero()
        {
            GameObject heroGameObject = InstantiateRegistered(AssetPath.HeroPath);
            HeroStaticData heroStaticData = _staticData.ForHero();

            heroGameObject
                .GetComponent<HeroHealth>()
                .Construct(heroStaticData.Stats.HealthData);
            heroGameObject
                .GetComponent<HeroAttack>()
                .Construct(heroStaticData.Stats);

            ActivateProgressReaders(heroGameObject);
            return heroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.PlayerState.LootData);

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
                // attack.Construct(monsterData.AttackData);
                attack.Damage = monsterData.AttackData.Damage;
                attack.Cleavage = monsterData.AttackData.Radius;
                attack.EffectiveDistance = monsterData.AttackData.Distance;
            }

            var lootSpawner = monsterGo.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);

            return monsterGo;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegistered(AssetPath.Loot)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.PlayerState.LootData);

            ActivateProgressReaders(lootPiece.gameObject);

            return lootPiece;
        }

        public SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            var spawner = InstantiateRegistered(AssetPath.EnemySpawner, at)
                .GetComponent<SpawnPoint>();

            spawner.Construct(this, _saveLoadService);
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