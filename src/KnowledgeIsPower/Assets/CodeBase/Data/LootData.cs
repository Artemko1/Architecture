﻿using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;
        public event Action Changed;

        public void Collect(LootItem loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
    }
}