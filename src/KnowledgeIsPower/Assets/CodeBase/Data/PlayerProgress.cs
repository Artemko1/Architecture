using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public State HeroState = new State();
        public Stats HeroStats = new Stats();
        public KillData KillData = new KillData();

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
        }
    }
}