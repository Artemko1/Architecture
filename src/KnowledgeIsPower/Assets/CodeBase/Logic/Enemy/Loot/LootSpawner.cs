using CodeBase.Data.Loot;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Randomizer;
using UnityEngine;
using Zenject;
using LootData = CodeBase.StaticData.ForComponents.LootData;

namespace CodeBase.Logic.Enemy.Loot
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;

        private LootData _lootData;

        private LootFactory _lootFactory;
        private RandomService _random;

        [Inject]
        private void Construct(LootFactory lootFactory, RandomService random)
        {
            _lootFactory = lootFactory;
            _random = random;
        }

        public void Construct(LootData monsterDataLootData) =>
            _lootData = monsterDataLootData;

        private void Start() =>
            _enemyHealth.Died += SpawnLoot;

        private void SpawnLoot()
        {
            LootPiece lootPiece = _lootFactory.CreateLoot(transform.position);

            lootPiece.Initialize(GenerateLoot());
        }

        private LootItem GenerateLoot() =>
            new LootItem(_random.Next(_lootData.MinLoot, _lootData.MaxLoot));
    }
}