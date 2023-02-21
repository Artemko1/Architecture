using CodeBase.Logic.Hero;
using CodeBase.StaticData.ForComponents;
using UnityEngine;
using Zenject;

namespace CodeBase.StaticData.Hero
{
    [CreateAssetMenu(fileName = "HeroStaticDataInstaller", menuName = "Installers/HeroStaticDataInstaller", order = 0)]
    public class HeroStaticDataInstaller : ScriptableObjectInstaller<HeroStaticDataInstaller>
    {
        public AttackData AttackData;
        public HealthData HealthData;

        public override void InstallBindings()
        {
            Container.BindInstance(AttackData).WhenInjectedInto<HeroAttack>();
            Container.BindInstance(HealthData).WhenInjectedInto<HeroHealth>();
        }
    }
}