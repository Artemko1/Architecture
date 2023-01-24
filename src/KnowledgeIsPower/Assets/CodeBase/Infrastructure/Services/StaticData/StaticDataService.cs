using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

        public void LoadMonsters() =>
            _monsters = Resources
                .LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(data => data.MonsterTypeId, data => data);

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters[typeId];
    }
}