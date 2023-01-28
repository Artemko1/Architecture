using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Services.StaticDataProvider
{
    public class StaticDataProviderService : IStaticDataProviderService
    {
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

        public void Load()
        {
            _monsters = Resources
                .LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(data => data.MonsterTypeId, data => data);
            _levels = Resources
                .LoadAll<LevelStaticData>("StaticData/Levels")
                .ToDictionary(data => data.LevelKey, data => data);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters[typeId];

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels[sceneKey];
    }
}