using CodeBase.Logic.Enemy.Loot;
using CodeBase.Logic.Enemy.Spawner;
using CodeBase.Services;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(Vector3 initialHeroPosition);
        GameObject CreateHud();

        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot(Vector3 at);
        SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
    }
}