﻿using CodeBase.Data.Loot;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Loot
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;
        private IGameFactory _gameFactory;

        private int _lootMax;
        private int _lootMin;
        private IRandomService _random;

        private void Start() =>
            EnemyDeath.Happened += SpawnLoot;

        public void Construct(IGameFactory gameFactory, IRandomService random)
        {
            _gameFactory = gameFactory;
            _random = random;
        }

        private void SpawnLoot()
        {
            LootPiece lootPiece = _gameFactory.CreateLoot();
            lootPiece.transform.position = transform.position;

            lootPiece.Initialize(GenerateLoot());
        }

        private LootItem GenerateLoot() =>
            new LootItem(_random.Next(_lootMin, _lootMax));

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
    }
}