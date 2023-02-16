using System.Threading.Tasks;
using CodeBase.Logic.Enemy.Loot;
using CodeBase.Services;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Task Warmup();
        Task<GameObject> CreateHero(Vector3 initialHeroPosition);
        Task<GameObject> CreateHud();

        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot(Vector3 at);
        void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        void Cleanup();
    }
}