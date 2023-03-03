using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using Zenject;

namespace CodeBase.Services.StaticDataProvider
{
    public class StaticDataProviderService
    {
        private readonly StaticDataLabels _labels;

        private readonly Dictionary<string, LevelStaticData> _levels = new Dictionary<string, LevelStaticData>();
        private readonly Dictionary<MonsterTypeId, MonsterStaticData> _monsters = new Dictionary<MonsterTypeId, MonsterStaticData>();
        private bool _isLoaded;
        private Dictionary<WindowId, WindowConfig> _windows;

        [Inject]
        public StaticDataProviderService(StaticDataLabels labels)
        {
            _labels = labels;
        }

        public async Task Load()
        {
            Assert.IsFalse(_isLoaded);

            Task<IList<MonsterStaticData>> loadMonstersTask =
                Addressables.LoadAssetsAsync<MonsterStaticData>(_labels.Monsters, data => _monsters.Add(data.MonsterTypeId, data)).Task;

            Task<IList<LevelStaticData>> loadLevelsTask =
                Addressables.LoadAssetsAsync<LevelStaticData>(_labels.Levels, data => _levels.Add(data.LevelKey, data)).Task;

            _windows = (await Addressables.LoadAssetAsync<WindowStaticData>("WindowStaticData").Task)
                .Configs
                .ToDictionary(config => config.WindowId, config => config);

            await loadMonstersTask;
            await loadLevelsTask;

            _isLoaded = true;
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