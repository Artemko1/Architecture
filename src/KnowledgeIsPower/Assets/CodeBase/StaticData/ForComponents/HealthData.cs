using System;
using UnityEngine;

namespace CodeBase.StaticData.ForComponents
{
    [Serializable]
    public class HealthData
    {
        [field: Range(1, 100), SerializeField] public int MaxHp { get; private set; } = 100;
    }
}