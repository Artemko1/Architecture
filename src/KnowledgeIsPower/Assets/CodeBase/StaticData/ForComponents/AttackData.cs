using System;
using UnityEngine;

namespace CodeBase.StaticData.ForComponents
{
    [Serializable]
    public class AttackData
    {
        [field: Range(1, 100), SerializeField]
        public float Damage { get; private set; } = 10;

        [field: Range(0.5f, 1f), SerializeField]
        public float Radius { get; private set; } = 1;

        [field: Range(0.5f, 1f), SerializeField]
        public float Distance { get; private set; } = 1;
    }
}