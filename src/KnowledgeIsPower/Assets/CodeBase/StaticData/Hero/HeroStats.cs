using System;
using CodeBase.StaticData.ForComponents;

namespace CodeBase.StaticData.Hero
{
    [Serializable]
    public class HeroStats // todo probably should inline class
    {
        public AttackData AttackData;
        public HealthData HealthData;
    }
}