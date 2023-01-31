using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldState
    {
        public KillData KillData;

        public WorldState()
        {
            KillData = new KillData();
        }

        public WorldState(KillData killData)
        {
            KillData = killData;
        }
    }
}