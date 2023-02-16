using CodeBase.Data.Loot;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Randomizer;
using UnityEngine;
using LootData = CodeBase.StaticData.ForComponents.LootData;

namespace CodeBase.Logic.Enemy.Loot
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;

        private IGameFactory _gameFactory;
        private LootData _lootData;

        private IRandomService _random;

        private void Start() =>
            _enemyHealth.Died += SpawnLoot;

        public void Construct(IGameFactory gameFactory, IRandomService random, LootData monsterDataLootData)
        {
            _gameFactory = gameFactory;
            _random = random;

            _lootData = monsterDataLootData;
        }

        private void SpawnLoot()
        {
            LootPiece lootPiece = _gameFactory.CreateLoot(transform.position);

            lootPiece.Initialize(GenerateLoot());
        }

        private LootItem GenerateLoot() =>
            new LootItem(_random.Next(_lootData.MinLoot, _lootData.MaxLoot));
    }
}