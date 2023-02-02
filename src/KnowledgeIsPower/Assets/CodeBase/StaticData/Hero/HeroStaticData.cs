using CodeBase.StaticData.ForComponents;
using UnityEngine;

namespace CodeBase.StaticData.Hero
{
    [CreateAssetMenu(fileName = "HeroStaticData", menuName = "StaticData/Hero", order = 0)]
    public class HeroStaticData : ScriptableObject
    {
        public AttackData AttackData;
        public HealthData HealthData;
    }
}