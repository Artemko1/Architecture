using System.Collections.Generic;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Logic.Enemy.Spawner;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressWriter> ProgressWriters { get; }

        GameObject CreateHero(Vector3 initialPoint);
        GameObject CreateHud();

        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot();
        SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);

        void Cleanup();
    }
}