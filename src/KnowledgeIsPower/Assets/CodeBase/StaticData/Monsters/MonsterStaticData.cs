using UnityEngine;

namespace CodeBase.StaticData.Monsters
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster", order = 0)]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        public HealthData HealthData = new HealthData();

        [Range(1, 100)] public float Damage = 5;

        [Range(0.5f, 1f)] public float AttackEffectiveDistance = 0.75f;
        [Range(0.5f, 1f)] public float AttackCleavage = 0.75f;

        [Range(1f, 10f)] public float MoveSpeed = 4f;

        public int MinLoot;
        public int MaxLoot;

        public GameObject Prefab;
    }
}