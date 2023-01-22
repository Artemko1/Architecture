using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgressReader, ISavedProgressWriter
    {
        [SerializeField] private MonsterTypeId _monsterTypeId;

        [SerializeField] private bool _slain;
        private IGameFactory _factory;

        private string _id;

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _factory = AllServices.Container.Single<IGameFactory>();
        }

        public void ReadFromProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawnersIds.Contains(_id))
            {
                _slain = true;
            }
            else
            {
                Spawn();
            }
        }

        public void WriteToProgress(PlayerProgress progress)
        {
            if (_slain)
            {
                progress.KillData.ClearedSpawnersIds.Add(_id);
            }
        }

        private void Spawn()
        {
            GameObject monster = _factory.CreateMonster(_monsterTypeId, transform);
            monster.GetComponent<EnemyDeath>().Happend += Slay;
        }

        private void Slay() =>
            _slain = true;
    }
}