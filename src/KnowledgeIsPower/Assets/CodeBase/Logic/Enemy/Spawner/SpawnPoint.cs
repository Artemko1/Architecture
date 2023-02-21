using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Spawner
{
    public class SpawnPoint : MonoBehaviour
    {
        private GameFactory _factory;
        private string _id;

        private MonsterTypeId _monsterTypeId;
        private PlayerProgress _progress;
        private ISaveLoadService _saveLoadService;

        private bool _slain;

        public void Construct(GameFactory gameFactory, ISaveLoadService saveLoadService, PersistentProgressService progressService,
            string id, MonsterTypeId monsterTypeId)
        {
            _factory = gameFactory;
            _saveLoadService = saveLoadService;
            _progress = progressService.Progress;
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
            GameObject monster = await _factory.CreateMonster(_monsterTypeId, transform);
            monster.GetComponent<IHealth>().Died += Slay;
        }

        private void Slay() =>
            _slain = true;
    }
}