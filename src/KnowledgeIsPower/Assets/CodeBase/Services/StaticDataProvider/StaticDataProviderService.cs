using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Services.StaticDataProvider
{
    public class StaticDataProviderService : IStaticDataProviderService
    {
        private HeroStaticData _heroStaticData;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private PlayerProgressStaticData _playerProgress;

        public void Load()
        {
            _monsters = Resources
                .LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(data => data.MonsterTypeId, data => data);
            _levels = Resources
                .LoadAll<LevelStaticData>("StaticData/Levels")
                .ToDictionary(data => data.LevelKey, data => data);
            _playerProgress = Resources
                .Load<PlayerProgressStaticData>("StaticData/DefaultProgress");
            _heroStaticData = Resources
                .Load<HeroStaticData>("StaticData/DefaultHeroStaticData");
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters[typeId];

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels[sceneKey];

        public PlayerProgressStaticData ForNewGame() =>
            _playerProgress;

        public HeroStaticData ForHero() =>
            _heroStaticData;
    }
}