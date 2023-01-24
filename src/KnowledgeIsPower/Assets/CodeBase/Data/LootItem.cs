using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootItem
    {
        public int Value;

        public LootItem(int value)
        {
            Value = value;
        }
    }
}