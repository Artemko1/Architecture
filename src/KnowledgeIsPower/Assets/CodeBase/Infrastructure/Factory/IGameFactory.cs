using System.Collections.Generic;
using CodeBase.Enemy.Loot;
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
        void Cleanup();


        void RegisterWriter(ISavedProgressWriter progressUpdater);
        void RegisterReader(ISavedProgressReader progressReader);
        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot();
    }
}