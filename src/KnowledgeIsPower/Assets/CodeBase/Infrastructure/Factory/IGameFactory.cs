using System.Threading.Tasks;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Services;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(Vector3 initialHeroPosition);
        GameObject CreateHud();

        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        Task<LootPiece> CreateLoot(Vector3 at);
        Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        Task Warmup();
    }
}