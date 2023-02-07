using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Hero;
using CodeBase.StaticData.Monsters;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticDataProvider
{
    public class StaticDataProviderService : IStaticDataProviderService
    {
        private HeroStaticData _heroStaticData;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private PlayerProgressStaticData _playerProgress;
        private Dictionary<WindowId, WindowConfig> _windows;

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

            _windows = Resources
                .Load<WindowStaticData>("StaticData/UI/WindowStaticData")
                .Configs
                .ToDictionary(config => config.WindowId, config => config);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters[typeId];

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels[sceneKey];

        public PlayerProgressStaticData ForNewGame() =>
            _playerProgress;

        public HeroStaticData ForHero() =>
            _heroStaticData;

        public WindowConfig ForWindow(WindowId id) =>
            _windows[id];
    }
}