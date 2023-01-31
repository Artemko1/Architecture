using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Spawner
{
    public class SpawnPoint : MonoBehaviour, ISavedProgressReader
    {
        private IGameFactory _factory;
        private ISaveLoadService _saveLoadService;
        private bool _slain;

        public MonsterTypeId MonsterTypeId { get; set; }
        public string ID { get; set; }

        private void Start() =>
            _saveLoadService.OnSave += WriteToProgress;

        private void OnDestroy() =>
            _saveLoadService.OnSave -= WriteToProgress;

        public void ReadFromProgress(PlayerProgress progress)
        {
            if (progress.WorldState.KillData.ClearedSpawnersIds.Contains(ID))
            {
                _slain = true;
            }
            else
            {
                Spawn();
            }
        }

        public void Construct(IGameFactory gameFactory)
        {
            _factory = gameFactory;
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void WriteToProgress(PlayerProgress progress)
        {
            if (_slain)
            {
                progress.WorldState.KillData.ClearedSpawnersIds.Add(ID);
            }
        }

        private void Spawn()
        {
            GameObject monster = _factory.CreateMonster(MonsterTypeId, transform);
            monster.GetComponent<EnemyDeath>().Happened += Slay;
        }

        private void Slay() =>
            _slain = true;
    }
}