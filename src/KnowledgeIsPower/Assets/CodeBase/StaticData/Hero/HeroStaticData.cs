using UnityEngine;

namespace CodeBase.StaticData.Hero
{
    [CreateAssetMenu(fileName = "HeroStaticData", menuName = "StaticData/Hero", order = 0)]
    public class HeroStaticData : ScriptableObject
    {
        [SerializeField] private HeroStats _stats;
        public HeroStats Stats => _stats;
    }
}