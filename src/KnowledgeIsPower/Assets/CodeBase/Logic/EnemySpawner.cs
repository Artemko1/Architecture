using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgressReader, ISavedProgressWriter
    {
        [SerializeField] private MonsterTypeId _monsterTypeId;

        [SerializeField] private bool _slain;

        private string _id;

        private void Awake() =>
            _id = GetComponent<UniqueId>().Id;

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
            Debug.Log("Spawn!", this);
        }
    }
}