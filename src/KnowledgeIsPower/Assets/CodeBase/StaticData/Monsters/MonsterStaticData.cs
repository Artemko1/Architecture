using CodeBase.StaticData.ForComponents;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Monsters
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster", order = 0)]
    public class MonsterStaticData : ScriptableObject
    {
        [field: SerializeField] public MonsterTypeId MonsterTypeId { get; private set; }

        [field: SerializeField] public HealthData HealthData { get; private set; } = new HealthData();

        [field: SerializeField] public AttackData AttackData { get; private set; } = new AttackData();

        [field: SerializeField, Range(1f, 10f)]
        public float MoveSpeed { get; private set; } = 4f;

        [field: SerializeField] public LootData LootData { get; private set; } = new LootData();

        [field: SerializeField] public AssetReferenceGameObject PrefabReference { get; private set; }
    }
}