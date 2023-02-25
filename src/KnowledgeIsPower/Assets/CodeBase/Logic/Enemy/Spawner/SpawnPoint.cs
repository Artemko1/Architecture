using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Logic.Enemy.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData.Monsters;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Enemy.Spawner
{
    public class SpawnPoint : MonoBehaviour
    {
        private EnemyFactory _enemyFactory;
        private string _id;

        private MonsterTypeId _monsterTypeId;
        private PlayerProgress _progress;
        private ISaveLoadService _saveLoadService;

        private bool _slain;

        [Inject]
        private void Construct(EnemyFactory enemyFactory, ISaveLoadService saveLoadService, PersistentProgressService progressService)
        {
            _enemyFactory = enemyFactory;
            _saveLoadService = saveLoadService;
            _progress = progressService.Progress;
        }

        public void Construct(string id, MonsterTypeId monsterTypeId)
        {
            _id = id;
            _monsterTypeId = monsterTypeId;
        }

        private void Start()
        {
            _saveLoadService.OnSave += WriteToProgress;

            ReadFromProgress();
        }

        private void OnDestroy() =>
            _saveLoadService.OnSave -= WriteToProgress;

        private void ReadFromProgress()
        {
            if (_progress.WorldState.KillData.ClearedSpawnersIds.Contains(_id))
            {
                _slain = true;
            }
            else
            {
                Spawn();
            }
        }

        private void WriteToProgress(PlayerProgress progress)
        {
            List<string> clearedSpawnersIds = progress.WorldState.KillData.ClearedSpawnersIds;
            if (_slain && !clearedSpawnersIds.Contains(_id))
            {
                clearedSpawnersIds.Add(_id);
            }
        }

        private async void Spawn()
        {
            GameObject monster = await _enemyFactory.CreateMonster(_monsterTypeId, transform);
            monster.GetComponent<IHealth>().Died += Slay;
        }

        private void Slay() =>
            _slain = true;
    }
}