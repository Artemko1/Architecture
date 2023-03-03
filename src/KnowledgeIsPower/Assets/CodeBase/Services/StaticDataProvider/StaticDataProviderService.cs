using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.Assertions;

namespace CodeBase.Services.StaticDataProvider
{
    public class StaticDataProviderService : IStaticDataProviderService
    {
        private bool _isLoaded;

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<WindowId, WindowConfig> _windows;

        public Task Load()
        {
            _monsters = Resources
                .LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(data => data.MonsterTypeId, data => data);

            _levels = Resources
                .LoadAll<LevelStaticData>("StaticData/Levels")
                .ToDictionary(data => data.LevelKey, data => data);

            _windows = Resources
                .Load<WindowStaticData>("StaticData/UI/WindowStaticData")
                .Configs
                .ToDictionary(config => config.WindowId, config => config);

            _isLoaded = true;
            return Task.CompletedTask;
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId)
        {
            Assert.IsTrue(_isLoaded);
            return _monsters[typeId];
        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            Assert.IsTrue(_isLoaded);
            return _levels[sceneKey];
        }

        public LevelStaticData ForDefaultLevel()
        {
            Assert.IsTrue(_isLoaded);
            return _levels[Constants.SceneNames.Graveyard];
        }

        public WindowConfig ForWindow(WindowId id)
        {
            Assert.IsTrue(_isLoaded);
            return _windows[id];
        }
    }
}