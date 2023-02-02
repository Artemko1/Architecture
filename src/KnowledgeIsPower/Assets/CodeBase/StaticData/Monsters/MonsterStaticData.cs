using CodeBase.StaticData.ForComponents;
using UnityEngine;

namespace CodeBase.StaticData.Monsters
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster", order = 0)]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        public HealthData HealthData = new HealthData();
        public AttackData AttackData = new AttackData();

        [Range(1f, 10f)] public float MoveSpeed = 4f;

        public LootData LootData = new LootData();

        public GameObject Prefab;
    }
}