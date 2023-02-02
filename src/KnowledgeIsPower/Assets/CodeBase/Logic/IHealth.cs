using System;
using CodeBase.StaticData.ForComponents;

namespace CodeBase.Logic
{
    public interface IHealth
    {
        float Current { get; }
        float Max { get; }
        event Action HealthChanged;
        event Action Died;
        void TakeDamage(float amount);
        void Construct(HealthData healthData);
    }
}