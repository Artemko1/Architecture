using System.Collections.Generic;
using CodeBase.StaticData.Monsters;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Level", fileName = "LevelData", order = 0)]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;

        public Vector3 InitialHeroPosition;

        public List<EnemySpawnerData> EnemySpawners;
    }
}