using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private GameObject _heroGameObject;

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticData)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
        }

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgressWriter> ProgressWriters { get; } = new List<ISavedProgressWriter>();

        public GameObject CreateHero(Vector3 initialPoint)
        {
            _heroGameObject = InstantiateRegistered(AssetPath.HeroPath, initialPoint);
            return _heroGameObject;
        }

        public GameObject CreateHud() =>
            InstantiateRegistered(AssetPath.HudPath);

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

            return monsterGo;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void RegisterWriter(ISavedProgressWriter progressUpdater) =>
            ProgressWriters.Add(progressUpdater);

        public void RegisterReader(ISavedProgressReader progressReader) =>
            ProgressReaders.Add(progressReader);


        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assetProvider.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            ISavedProgressReader[] progressReaders = gameObject.GetComponentsInChildren<ISavedProgressReader>();
            foreach (ISavedProgressReader progressReader in progressReaders)
            {
                RegisterReader(progressReader);
            }

            ISavedProgressWriter[] progressUpdaters = gameObject.GetComponentsInChildren<ISavedProgressWriter>();

            foreach (ISavedProgressWriter progressUpdater in progressUpdaters)
            {
                RegisterWriter(progressUpdater);
            }
        }
    }
}