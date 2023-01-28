using System.Collections.Generic;
using CodeBase.Enemy.Loot;
using CodeBase.Enemy.Spawner;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgressWriter> ProgressWriters { get; }

        GameObject CreateHero(Vector3 initialPoint);
        GameObject CreateHud();

        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot();
        SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);

        void Cleanup();
    }
}