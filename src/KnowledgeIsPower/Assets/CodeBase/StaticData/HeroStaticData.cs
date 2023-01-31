using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "HeroStaticData", menuName = "StaticData/Hero", order = 0)]
    public class HeroStaticData : ScriptableObject
    {
        [SerializeField] private Stats _stats;
        public Stats Stats => _stats;
    }
}