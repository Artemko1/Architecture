using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Spawner
{
    public class SpawnPoint : MonoBehaviour, ISavedProgressReader
    {
        private IGameFactory _factory;
        private string _id;

        private MonsterTypeId _monsterTypeId;
        private ISaveLoadService _saveLoadService;

        private bool _slain;

        private void Start() =>
            _saveLoadService.OnSave += WriteToProgress;

        private void OnEnable()
        {
            if (_saveLoadService != null)
            {
                _saveLoadService.OnSave += WriteToProgress;
            }
        }

        private void OnDisable() =>
            _saveLoadService.OnSave -= WriteToProgress;

        public void ReadFromProgress(PlayerProgress progress)
        {
            if (progress.WorldState.KillData.ClearedSpawnersIds.Contains(_id))
            {
                _slain = true;
            }
            else
            {
                Spawn();
            }
        }

        public void Construct(IGameFactory gameFactory, ISaveLoadService saveLoadService, string id, MonsterTypeId monsterTypeId)
        {
            _factory = gameFactory;
            _saveLoadService = saveLoadService;
            _id = id;
            _monsterTypeId = monsterTypeId;
        }

        private void WriteToProgress(PlayerProgress progress)
        {
            if (_slain)
            {
                progress.WorldState.KillData.ClearedSpawnersIds.Add(_id);
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