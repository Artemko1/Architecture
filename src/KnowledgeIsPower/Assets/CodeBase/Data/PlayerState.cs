using System;
using CodeBase.Data.Loot;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerState
    {
        public PositionOnLevel PositionOnLevel;
        public LootData LootData;

        public float CurrentHP;


        public PlayerState(PositionOnLevel positionOnLevel, float currentHP)
        {
            LootData = new LootData();
            PositionOnLevel = positionOnLevel;
            CurrentHP = currentHP;
        }
    }
}