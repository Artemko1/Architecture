using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Spawner
{
    public class SpawnPoint : MonoBehaviour, ISavedProgressReader, ISavedProgressWriter
    {
        private IGameFactory _factory;
        private bool _slain;

        public MonsterTypeId MonsterTypeId { get; set; }
        public string ID { get; set; }

        public void ReadFromProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawnersIds.Contains(ID))
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
                progress.KillData.ClearedSpawnersIds.Add(ID);
            }
        }

        public void Construct(IGameFactory gameFactory) =>
            _factory = gameFactory;

        private void Spawn()
        {
            GameObject monster = _factory.CreateMonster(MonsterTypeId, transform);
            monster.GetComponent<EnemyDeath>().Happend += Slay;
        }

        private void Slay() =>
            _slain = true;
    }
}