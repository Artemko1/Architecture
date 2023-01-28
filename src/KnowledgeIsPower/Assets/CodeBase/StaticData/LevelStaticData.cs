using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Level", fileName = "LevelData", order = 0)]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;

        public List<EnemySpawnerData> EnemySpawners;
    }
}