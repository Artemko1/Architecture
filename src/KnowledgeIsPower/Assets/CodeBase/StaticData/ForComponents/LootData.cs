using System;
using UnityEngine;

namespace CodeBase.StaticData.ForComponents
{
    [Serializable]
    public class LootData
    {
        [field: Range(1, 10), SerializeField]
        public int MinLoot { get; private set; } = 1;

        [field: Range(1, 10), SerializeField]
        public int MaxLoot { get; private set; } = 10;
    }
}